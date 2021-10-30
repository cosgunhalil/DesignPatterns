namespace BurgerShop 
{
    using BurgerShop.DesignPattern.Builder;
    using System;

    public class BeefBurgerBuilder : IBurgerBuilder
    {
        private Burger burger;

        public void AddBread()
        {
            burger.SetBread("Beef Burger Bread");
        }

        public void AddMainElement()
        {
            burger.SetMainElement("Beef");
        }

        public void AddSalad()
        {
            burger.SetSalad("Green Salad");
        }

        public void AddSauce()
        {
            burger.SetSauce("mustard");
        }

        public void Reset()
        {
            burger = new Burger();
        }

        public Burger GetBurger()
        {
            return burger;
        }
    }
}


