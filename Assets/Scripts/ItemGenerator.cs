using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public Totalpool totalPool;
    public List<SoupItem> pool;
    public List<SoupItem> items;
    public Vector3 center;
    public float radius;

    public RectTransform collectList;
    public TextMeshProUGUI listItemPr;
    Dictionary<SoupItem, int> tasks = new();

    public int SpawnTestAmount = 100;

    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectPool(int n)
    {
        pool = totalPool.PickN(n);
    }
    public void Generate()
    {
        SelectPool(GameManager.instance.score + 3);
        Generatelist(GameManager.instance.score+2, 10);
        GenerateItems(100);
    }
    public void Generatelist(int items, int apxAmount = 10)
    {
        tasks.Clear();
        for (int i = 0; i < items; i++)
        {
            var itemUI = Instantiate(listItemPr, collectList);
            SoupItem item = PickRandom(pool);
            pool.Remove(item);
            int amount = Random.Range(apxAmount/items, apxAmount/items + 2);
            itemUI.text = item.itemName + " " + amount;
        }
        foreach (var task in tasks)
        {
            pool.Add(task.Key);
        }
    }
    public void GenerateItems(int num)
    {
        for (int i = 0; items.Count > 0;)
        {
            var item = items[i];
            items.RemoveAt(i);
            if (!Application.isPlaying)
                DestroyImmediate(item.gameObject);
            else
                Destroy(item.gameObject);
        }
        items.Clear();
        for (int i = 0; i < num; i++)
        {
            SpawnItem();
        }
    }
    public void SpawnItem()
    {
        SoupItem prefab = PickRandom(pool);
        float r = Random.Range(0f, 1f);
        r = Mathf.Sqrt(r) * radius;
        float phi = Random.Range(0f, Mathf.PI * 2);
        Vector3 pos = new Vector3(r * Mathf.Sin(phi), 0, r * Mathf.Cos(phi));
        
        var instance = Instantiate(prefab, transform);
        instance.transform.position = pos;

        items.Add(instance);
    }
    public T PickRandom<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(center + new Vector3(0,0,radius), center + new Vector3(0,0,-radius));
        Gizmos.DrawLine(center + new Vector3(radius, 0, 0), center + new Vector3(-radius, 0, 0));
    }
}
