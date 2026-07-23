using UnityEngine;

namespace DesignPatterns.Decorator.Sample
{
    /// <summary>
    /// Entry point. Press Play — it builds a base weapon, wraps it in stacked
    /// enchantments, and logs the composed description and damage. It then shows
    /// the two things subclassing can't give you cleanly: any combination stacks,
    /// and the order of decorators changes the outcome.
    /// </summary>
    public sealed class DecoratorPatternDemo : MonoBehaviour
    {
        private void Start()
        {
            IDamageSource sword = new Weapon("Sword", 10);
            Debug.Log($"<color=grey>Base:</color> {sword.Describe()} = {sword.GetDamage()}");

            // Stack enchantments: (10 + 3) + 5, then x2 = 36.
            IDamageSource enchanted = new CriticalStrike(new FireEnchantment(new SharpenEnchantment(sword)));
            Debug.Log($"<color=cyan>Enchanted:</color> {enchanted.Describe()} = {enchanted.GetDamage()}");

            // Order matters: Crit multiplies whatever is below it.
            IDamageSource critThenSharpen = new SharpenEnchantment(new CriticalStrike(new Weapon("Axe", 10)));
            IDamageSource sharpenThenCrit = new CriticalStrike(new SharpenEnchantment(new Weapon("Axe", 10)));
            Debug.Log($"<color=orange>Order matters:</color> " +
                      $"Sharpen(Crit(Axe)) = {critThenSharpen.GetDamage()}  vs  " +
                      $"Crit(Sharpen(Axe)) = {sharpenThenCrit.GetDamage()}");

            // The same decorator can appear more than once.
            IDamageSource doubleFire = new FireEnchantment(new FireEnchantment(new Weapon("Torch", 4)));
            Debug.Log($"<color=lime>Stacked twice:</color> {doubleFire.Describe()} = {doubleFire.GetDamage()}");
        }
    }
}
