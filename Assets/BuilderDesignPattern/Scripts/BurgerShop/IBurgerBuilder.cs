namespace BurgerShop.DesignPattern.Builder 
{
    public interface IBurgerBuilder
    {
        void Reset();
        void AddBread();
        void AddSauce();
        void AddSalad();
        void AddMainElement();
        Burger GetBurger();
    }
}


