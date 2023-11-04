using System.Collections.Generic;
using UnityEngine;

public static class ListHelper
{
    public static void AddToList<T>(T item, List<T> list)
    {
        if (!list.Contains(item))
            list.Add(item);
    }
    
    public static void RemoveFromList<T>(T item, List<T> list)
    {
        if (list.Contains(item))
            list.Remove(item);
    }
}