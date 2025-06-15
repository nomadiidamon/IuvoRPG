
public static class SemiBehaviorSystem
{
    public static T Create<T>(SemiBehaviorManager manager) where T : SemiBehavior, new()
    {
        T instance = new T();
        manager.Register(instance);
        return instance;
    }

    public static void Destroy(SemiBehavior behavior)
    {
        behavior.parentManager.Unregister(behavior);
    }
}

