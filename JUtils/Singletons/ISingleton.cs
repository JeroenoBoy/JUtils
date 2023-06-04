namespace JUtils.Singletons
{
    public interface ISingleton<T>
        where T : ISingleton<T>
    {
        public static T instance { get; private set; }
    }
}
