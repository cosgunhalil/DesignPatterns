# Adapter Pattern — Ad-Network Analytics

> Three ad SDKs with three completely different callback shapes. One normalized analytics event. The client never learns which network it came from.

## Intent

A game monetizes through **Google Mobile Ads (AdMob)**, **AppLovin MAX**, and **Unity LevelPlay**. Each exposes different callback names, event structures, error types, reward types, and revenue representations. We want a single normalized analytics stream. Each provider gets an **adapter** that subscribes to its callbacks, translates them into one `AdAnalyticsEvent`, and forwards to a generic `IAnalyticsCollector`.

These are **analytics-only adapters**. They translate callbacks; they do **not** load, show, or check ads, retry, or grant rewards. That scope discipline is half the lesson.

## Structure

| Folder | Assembly | Contents |
|---|---|---|
| `Core/` | `DesignPatterns.Adapter` | Normalized domain (`AdAnalyticsEvent`, enums), `IAnalyticsCollector`, `AdAnalyticsContext`, and the shared `AdAnalyticsAdapterBase` (correlation, dedup, rewarded-state). Engine-free. |
| `Sample/` | `DesignPatterns.Adapter.Sample` | Three educational SDK stubs, three concrete adapters, the console collector, the Unity context provider, and a demo driver. |
| `Tests/` | `DesignPatterns.Adapter.Tests` | 25 EditMode tests (Window → General → Test Runner). |

## Capability matrix (formats actually supported per provider)

| Format | Google Mobile Ads | AppLovin MAX | Unity LevelPlay |
|---|---|---|---|
| Rewarded | ✅ | ✅ | ✅ |
| Interstitial | ✅ | ✅ | ✅ |
| Banner | ✅ | ✅ (+ MREC) | ✅ |
| App Open | ✅ | ✅ | ✅ |
| Native | ✅ *(NativeOverlayAd; asset-based native deprecated)* | ❌ *(no Unity native API)* | ⚠️ *(not modeled here)* |

The normalized `AdFormat` enum can represent all five formats regardless — the domain is not limited to what any one provider supports. The sample implements the **Rewarded** and **Interstitial** paths for each provider; the others are documented but not stubbed, to keep the example focused on Adapter.

### A note on SDK fidelity

- **Google Mobile Ads** and **AppLovin MAX** stubs mirror the current official Unity plugins (GMA v11.3.0; MAX 8.6.4), with event names, delegate signatures, and data shapes verified against the official docs and plugin source — see citations below.
- **Unity LevelPlay** is modeled *representatively* in the shape of its current instance-based ad-unit API (`LevelPlayRewardedAd` with instance events). Exact member names/values here are illustrative for teaching, not verified line-by-line. The architecture is the point.

All three stub sets are **educational stand-ins**, clearly labeled in-file, not vendor source code.

## Normalized lifecycle

`AdLifecycleEvent`: `Requested, Loaded, LoadFailed, Displayed, ImpressionRecorded, Clicked, RevenuePaid, Completed, Cancelled, Closed, DisplayFailed`.

Applicability:
- **Full-screen (Rewarded / Interstitial / App Open):** the full set. Only **Rewarded** uses `Completed` (reward callback fired) and `Cancelled` (finalized without a reward).
- **Banner / Native:** `Requested, Loaded, LoadFailed, ImpressionRecorded, Clicked, RevenuePaid`. No completion, cancellation, or close.

Key semantic rules baked into the code:
1. `Completed` **is** "reward granted by the provider" — there is deliberately no separate `RewardGranted` lifecycle event.
2. A rewarded ad emits `Completed` **only** when the provider fires its official reward callback.
3. A rewarded ad may emit **both** `Completed` and `Closed` — they are different facts.
4. `Cancelled` is emitted **only** at finalization, when the ad was closed **and** no reward callback ever arrived.
5. A normal interstitial close is **not** a cancellation.
6. `Requested` is emitted by the game via `RecordRequest(...)` (called just before requesting an ad), because provider SDKs don't reliably expose a request callback. `RecordRequest` also mints the local **interaction correlation id** used across the whole interaction, since providers lack a reliable cross-callback impression id.

## The normalized event model

`AdAnalyticsEvent` is immutable (`init`-only). Intentionally nullable fields:
- **Revenue / CurrencyCode** — only on `RevenuePaid`.
- **Reward*** — only meaningful for rewarded ads; `null` for every other format.
- **RewardEligible / RewardGranted** — set from the provider's reward callback. The provider is asserting the user *earned* the reward.
- **RewardDeliverySucceeded** — always `null` at the adapter boundary. Only the game economy or backend knows if the reward reached the player's inventory. **The adapter must never claim delivery.** This is the single most important honesty rule in the model.
- **Error*** — only on `LoadFailed` / `DisplayFailed`.

There is deliberately **no provider-specific metadata dictionary** — that would be an escape hatch for leaking vendor detail into the domain.

## Adapter participants

- **Client:** the game's ad manager (represented by `AdapterPatternDemo`) and the analytics backend consuming the collector.
- **Target interface:** `IAnalyticsCollector` + the normalized `AdAnalyticsEvent`.
- **Adapters:** `GoogleMobileAdsAnalyticsAdapter`, `AppLovinMaxAnalyticsAdapter`, `UnityLevelPlayAnalyticsAdapter` (all sharing `AdAnalyticsAdapterBase`).
- **Adaptees:** the three provider SDKs (`RewardedAd`/`InterstitialAd`, `MaxSdkCallbacks.*`, `LevelPlayRewardedAd`/`LevelPlayInterstitialAd`).
- **Normalized domain model:** `AdAnalyticsEvent`, `AdFormat`, `AdLifecycleEvent`, `AdProvider`, `AdAnalyticsContext`.

## Dependency flow

```text
Google Mobile Ads callbacks ─┐
AppLovin MAX callbacks ──────┼─> Provider Adapter ──> AdAnalyticsEvent ──> IAnalyticsCollector ──> Backend
Unity LevelPlay callbacks ───┘        (translate)        (normalized)        (generic)         (out of scope)
```

The backend is outside this example. `ConsoleAnalyticsCollector` stands in for it.

## Why this is Adapter (not just object mapping)

Object mapping converts one data structure to another. These adapters also translate **behaviour and lifecycle semantics**: MAX's static `OnAdHiddenEvent` and LevelPlay's `OnAdClosed` both become `Closed`; GMA's reward *callback argument* and LevelPlay's reward *event* both become `Completed`; the close-vs-reward ordering problem is absorbed so the client sees a coherent lifecycle. Adapting incompatible **interfaces and interaction models** — not just fields — is exactly the Adapter pattern.

## Design decisions

- **Gameplay never learns the provider.** It reads normalized events; swapping or adding a network changes only which adapter is constructed.
- **The collector has no provider conditionals.** All provider knowledge lives in the adapter; `ConsoleAnalyticsCollector` proves the shapes are already uniform.
- **Provider types don't leak.** `MaxSdkBase.AdInfo`, `AdValue`, `LevelPlayAdInfo` never appear in the domain — no metadata dictionary, no vendor enums.
- **Adapters don't load or show ads.** Mixing ad operations into analytics translation is the most common way these classes rot into god-objects.
- **Reward delivery is never confirmed by the adapter.** A provider reward callback means *eligible/earned*, so `RewardEligible`/`RewardGranted` are set but `RewardDeliverySucceeded` stays `null`.
- **Correlation is necessary** because no provider gives one stable id across all callbacks of an interaction; the adapter mints one at `RecordRequest`.
- **Close/reward ordering matters:** deciding cancellation eagerly on close would misreport completed rewarded ads whenever the reward callback trails the close.
- **Exact-duplicate suppression sits at the provider boundary** because that's where duplicates originate (SDKs re-fire callbacks). It's a client-side courtesy — **not** a replacement for backend idempotency.

## Common mistakes (all avoided here)

One mega-adapter with per-provider `switch`; provider conditionals in the collector; treating every close as cancellation; treating every displayed rewarded ad as completed; claiming inventory delivery from a provider callback; a separate schema per provider; mixing ad operations with translation; inventing unsupported formats/callbacks; forgetting to unsubscribe from **static** SDK events (a real leak with MAX); ignoring duplicate callbacks.

## Run it

- **Demo:** open `Sample/Scenes/AdapterSample.unity`, press Play, read the Console. Six scenarios run: GMA rewarded completed; MAX rewarded with a duplicate reward suppressed; LevelPlay rewarded with close-before-reward (still completed); LevelPlay rewarded cancelled; MAX interstitial (reward fields null); GMA load failure.
- **Tests:** Window → General → Test Runner → EditMode → 25 tests.

## Sources (Google Mobile Ads & AppLovin MAX)

- Google Mobile Ads Unity — rewarded, interstitial, banner, app open, native-overlay, and API reference: <https://developers.google.com/admob/unity/rewarded>, <https://developers.google.com/admob/unity/reference/class/google-mobile-ads/api/ad-value>
- Google Mobile Ads Unity plugin v11.3.0 release: <https://github.com/googleads/googleads-mobile-unity/releases>
- AppLovin MAX Unity — rewarded ads and impression-level revenue: <https://support.applovin.com/en/max/unity/ad-formats/rewarded-ads>, <https://support.applovin.com/en/max/unity/overview/advanced-settings>
- AppLovin MAX Unity plugin source (`MaxSdkCallbacks.cs`, `MaxSdkBase.cs`): <https://github.com/AppLovin/AppLovin-MAX-Unity-Plugin>
