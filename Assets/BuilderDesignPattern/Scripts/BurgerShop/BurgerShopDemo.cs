namespace BurgerShop 
{
    using BurgerShop.DesignPattern.Builder;
    using UnityEngine;

    public class BurgerShopDemo : MonoBehaviour
    {
        private void Start()
        {
            var beefBurgerBuilder = new BeefBurgerBuilder();
            var burgerDirector = new BurgerDirector();
            burgerDirector.MakeBurger(beefBurgerBuilder);

            var beefBurger = beefBurgerBuilder.GetBurger();

            var veggieBurgerBuilder = new VeggieBurgerBuilder();
            burgerDirector.MakeBurger(veggieBurgerBuilder);
            var veggieBurger = veggieBurgerBuilder.GetBurger();

            Debug.Log("Making burger process completed!");
        }
    }
}

