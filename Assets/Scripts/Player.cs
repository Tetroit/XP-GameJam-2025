using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera cam;
    public float acceleration = 4f;
    public float maxSpeed = 5f;
    public float soupFriction = 1f;
    Vector3 acc;

    bool shouldMove = false;

    public bool isLocked = false;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        shouldMove = false;
        velocity = rb.velocity;
        if (velocity.magnitude > maxSpeed)
        {
            rb.velocity = velocity.normalized * maxSpeed;
        }
    }
    private void Update()
    {
        float depth = Mathf.Clamp01(-transform.position.y + 0.5f);
        if (Input.GetMouseButton(0) && !isLocked)
        {
            shouldMove = true;
            Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePoint = mouseCast.GetPoint(
                -Vector3.Dot(mouseCast.origin - Vector3.zero, Vector3.up)/
                Vector3.Dot(mouseCast.direction, Vector3.up));
            Vector3 direction = (mousePoint - transform.position).normalized;
            rb.AddForce(direction * acceleration);
        }
        rb.AddForce(new Vector3(0, -depth * 1.2f * Physics.gravity.y, 0));
        rb.AddForce(-velocity * soupFriction);
    }
    private void OnDrawGizmos()
    {
        Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(mouseCast);
        Vector3 mousePoint = mouseCast.GetPoint(Vector3.Dot(mouseCast.direction, Vector3.up));
        Gizmos.DrawSphere(mousePoint, 0.1f);
    }
}
