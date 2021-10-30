namespace BurgerShop.DesignPattern.Builder 
{
    public class BurgerDirector
    {
        public void MakeBurger(IBurgerBuilder burgerBuilder) 
        {
            burgerBuilder.Reset();
            burgerBuilder.AddBread();
            burgerBuilder.AddSauce();
            burgerBuilder.AddSalad();
            burgerBuilder.AddMainElement();
        }
    }
}


