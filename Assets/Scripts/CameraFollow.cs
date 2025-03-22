using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;

    private void Update()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.LookRotation(-offset, Vector3.up);
    }
}
