namespace Core
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        public static T Instance = new T();

        protected Singleton()
        {
        }
    }
}