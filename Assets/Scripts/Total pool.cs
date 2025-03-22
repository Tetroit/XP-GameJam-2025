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
}
