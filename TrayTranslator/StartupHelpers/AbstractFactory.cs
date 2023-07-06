using System;

namespace TrayTranslator.StartupHelpers;

public class AbstractFactory<T> : IAbstractFactory<T>
{
    private readonly Func<T> factory;

    public AbstractFactory(Func<T> factory)
    {
        this.factory = factory;
    }

    public T Create()
    {
        return factory();
    }
}
