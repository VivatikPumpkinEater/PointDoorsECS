using Leopotam.EcsLite;

public static class Utility
{
    public static ref T GetOrAddComponent<T>(int entity, EcsPool<T> pool) where T : struct
    {
        if (pool.Has(entity))
            return ref pool.Get(entity);

        return ref pool.Add(entity);
    }
}