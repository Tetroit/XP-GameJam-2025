using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public Vector3 center;
    public float radius;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
    }
}
