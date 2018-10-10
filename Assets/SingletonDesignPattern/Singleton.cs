
public class Singleton<T> where T : class, new()
{
    private static readonly T instance = new T();

    static Singleton()
    {
    }

    protected Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
}
