using System;
using System.Collections.Generic;

public class DataListener<T> where T : struct
{
    public static event Action<int> EntityUpdate;

    public static Dictionary<int, T> Components = new();

    public static void AddComponent(int entity, T component)
    {
        if (Components.ContainsKey(entity))
            return;
        
        Components.Add(entity, component);
    }

    public static void UpdateComponent(int entity, T component)
    {
        if (!Components.ContainsKey(entity))
            return;

        Components[entity] = component;
        
        EntityUpdate?.Invoke(entity);
    }

    public static void RemoveComponent(int entity)
    {
        if (!Components.ContainsKey(entity))
            return;

        Components.Remove(entity);
    }
}