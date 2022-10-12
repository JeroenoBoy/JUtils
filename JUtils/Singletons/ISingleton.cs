namespace JUtils.Singletons
{
    public interface ISingleton<T>
        where T : ISingleton<T>
    {
        public static T Instance { get; private set; }
    }
}
