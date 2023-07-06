namespace TrayTranslator.StartupHelpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}