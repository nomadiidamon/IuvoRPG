
public static class SemiBehaviorSystem
{
    public static T Create<T>() where T : SemiBehavior, new()
    {
        T instance = new T();
        SemiBehaviorManager.Instance.Register(instance);
        return instance;
    }

    public static void Destroy(SemiBehavior behavior)
    {
        SemiBehaviorManager.Instance.Unregister(behavior);
    }
}

