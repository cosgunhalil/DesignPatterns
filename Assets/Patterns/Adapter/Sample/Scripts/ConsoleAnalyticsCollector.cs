using System.Text;
using UnityEngine;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// Demo collector: prints each normalized event to the Unity Console.
    /// Note what is ABSENT here — provider names never appear in any condition.
    /// Events from all three providers arrive with the same shape; that
    /// uniformity is the adapters' entire job.
    /// </summary>
    public sealed class ConsoleAnalyticsCollector : IAnalyticsCollector
    {
        public void Collect(AdAnalyticsEvent analyticsEvent)
        {
            var line = new StringBuilder()
                .Append($"<b>[{analyticsEvent.Provider}]</b> ")
                .Append($"{analyticsEvent.AdFormat} <color=cyan>{analyticsEvent.EventType}</color>")
                .Append($" unit={analyticsEvent.AdUnitId}")
                .Append($" placement={analyticsEvent.PlacementId ?? "-"}")
                .Append($" interaction={Shorten(analyticsEvent.InteractionId)}");

            if (analyticsEvent.Revenue.HasValue)
            {
                line.Append($" revenue={analyticsEvent.Revenue.Value} {analyticsEvent.CurrencyCode}");
            }

            if (analyticsEvent.RewardAmount.HasValue)
            {
                line.Append($" reward={analyticsEvent.RewardAmount.Value} {analyticsEvent.RewardType}")
                    .Append($" eligible={analyticsEvent.RewardEligible} granted={analyticsEvent.RewardGranted}")
                    .Append($" delivered={(analyticsEvent.RewardDeliverySucceeded?.ToString() ?? "unknown")}");
            }

            if (analyticsEvent.ErrorCode != null)
            {
                line.Append($" <color=red>error={analyticsEvent.ErrorCode} \"{analyticsEvent.ErrorMessage}\"</color>");
            }

            line.Append($" user={analyticsEvent.UserId} session={Shorten(analyticsEvent.SessionId)}")
                .Append($" at={analyticsEvent.OccurredAtUtc:HH:mm:ss.fff}Z");

            Debug.Log(line.ToString());
        }

        private static string Shorten(string id) =>
            string.IsNullOrEmpty(id) ? "-" : id.Length <= 8 ? id : id.Substring(0, 8);
    }
}
