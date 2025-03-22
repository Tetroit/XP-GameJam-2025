using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Totalpool", menuName = "Soup/Totalpool")]
public class Totalpool : ScriptableObject
{
    public List<SoupItem> collection;
    public List<SoupItem> PickN(int amount)
    {
        if (amount > collection.Count)
        {
            Debug.LogWarning("Not enough items in the pool");
            return null;
        }
        List<SoupItem> picked = new List<SoupItem>();
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, collection.Count);
            picked.Add(collection[index]);
            collection.RemoveAt(index);
        }
        foreach (SoupItem item in picked)
        {
            collection.Add(item);
        }
        Debug.Log(picked.Count);
        return picked;
    }
    public void SortItems()
    {
        collection.Sort((a, b) => a.itemName.CompareTo(b.itemName));
        for (int i = 0; i < collection.Count - 1; i++)
        {
            if (collection[i].itemName == collection[i + 1].itemName)
            {
                Debug.LogWarning("Removing duplicate " + collection[i].itemName);
                collection.RemoveAt(i + 1);
            }
        }
    }
    public static List<T> PickN<T>(List<T> from, int amount)
    {
        if (amount > from.Count)
        {
            Debug.LogWarning("Not enough items in the pool");
            return null;
        }
        List<T> picked = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, from.Count);
            picked.Add(from[index]);
            from.RemoveAt(index);
        }
        foreach (T item in picked)
        {
            from.Add(item);
        }
        Debug.Log(picked.Count);
        return picked;
    }
}
