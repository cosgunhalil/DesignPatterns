using BurgerShop.DesignPattern.Builder;

namespace BurgerShop 
{
    public class VeggieBurgerBuilder : IBurgerBuilder
    {
        private Burger burger;

        public void AddBread()
        {
            burger.SetBread("Veggie Bread");
        }

        public void AddMainElement()
        {
            burger.SetMainElement("Veggie Burger");
        }

        public void AddSalad()
        {
            burger.SetSalad("Veggie Salad");
        }

        public void AddSauce()
        {
            burger.SetSauce("Veggie Sauce");
        }

        public Burger GetBurger()
        {
            return burger;
        }

        public void Reset()
        {
            burger = new Burger();
        }
    }
}


