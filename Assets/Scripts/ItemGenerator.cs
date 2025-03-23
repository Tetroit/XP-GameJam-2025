using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;
    
    public Totalpool totalPool;
    public List<SoupItem> pool;
    public List<SoupItem> items;
    public Vector3 center;
    public float radius;

    bool levelClear = false;

    public RectTransform collectList;
    public TextMeshProUGUI listItemPr;
    Dictionary<string, int> tasks = new();

    public int SpawnTestAmount = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
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
        Debug.Log(pool.Count);
    }
    public void Generate()
    {
        levelClear = false;
        SelectPool(GameManager.instance.score + 3);
        Generatelist((GameManager.instance.score/2 + 2), GameManager.instance.score);
        GenerateItems(SpawnTestAmount);
    }
    public void Generatelist(int items, int apxAmount = 10)
    {
        tasks.Clear();
        List<SoupItem> newTasks = Totalpool.PickN(pool, items);
        foreach (var item in newTasks)
        {
            tasks.Add(item.itemName, Random.Range(apxAmount/items + 1, apxAmount/items + 3));
            var listItem = Instantiate(listItemPr, collectList);
            listItem.text = item.itemName + " " + tasks[item.itemName];
        }
    }
    public void ClearItems()
    {
        //remove all
        for (int i = 0; items.Count > 0;)
        {
            DestroyItem(items[i]);
        }
    }
    public void GenerateItems(int num)
    {
        ClearItems();
        //add new
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
    public void DestroyItem(SoupItem item)
    {
        items.Remove(item);
        if (!Application.isPlaying)
            DestroyImmediate(item.gameObject);
        else
            Destroy(item.gameObject);
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


    public void OnpickUp(SoupItem item)
    {
        DestroyItem(item);
        SpawnItem();
        if (tasks.ContainsKey(item.itemName))
        {
            tasks[item.itemName]--;
            if (tasks[item.itemName] == 0)
            {
                RemoveTask(item.itemName);
                return;
            }
            foreach (Transform child in collectList)
            {
                if (child.GetComponent<TextMeshProUGUI>().text.Contains(item.itemName))
                {
                    child.GetComponent<TextMeshProUGUI>().text = item.itemName + " " + tasks[item.itemName];
                    break;
                }
            }
        }
    }
    void RemoveTask(string name)
    {
        tasks.Remove(name);
        foreach (Transform child in collectList)
        {
            if (child.GetComponent<TextMeshProUGUI>().text.Contains(name))
            {
                Destroy(child.gameObject);
                break;
            }
        }
        if (tasks.Count == 0 && !levelClear)
        {
            levelClear = true;
            GameManager.instance.Completed();
        }   
    }
    public void OnReset()
    {
        ClearItems();
        Generate();
    }
    public void OnComplete()
    {
        ClearItems();
    }
    public void OnLose()
    {
        ClearItems();
        tasks.Clear(); 
        foreach (Transform child in collectList)
        {
            Destroy(child.gameObject);
        }
    }
}
