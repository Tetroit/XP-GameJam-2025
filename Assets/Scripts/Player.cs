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

    float height = 3;
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
        float depth = Mathf.Clamp01(-transform.position.y + height/2);
        if (shouldMove)
        {
            shouldMove = false;
            rb.AddForce(acc);
        }
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        rb.AddForce(new Vector3(0, -depth * 2.2f * Physics.gravity.y, 0));
        rb.AddForce(-rb.linearVelocity * soupFriction);
        rb.AddTorque(Random.insideUnitSphere * 10);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && !isLocked)
        {
            shouldMove = true;
            Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePoint = mouseCast.GetPoint(
                -Vector3.Dot(mouseCast.origin - Vector3.zero, Vector3.up)/
                Vector3.Dot(mouseCast.direction, Vector3.up));
            Vector3 direction = (mousePoint - transform.position).normalized;
            acc = (direction * acceleration);
        }
    }
    private void OnDrawGizmos()
    {
        Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(mouseCast);
        Vector3 mousePoint = mouseCast.GetPoint(Vector3.Dot(mouseCast.direction, Vector3.up));
        Gizmos.DrawSphere(mousePoint, 0.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoupItem"))
        {
            ItemGenerator.instance.OnpickUp(other.GetComponent<SoupItem>());
        }
    }
    public void OnComplete()
    {
        Invoke(nameof(Respawn), 1f);
    }
    public void Respawn()
    {
        transform.position = Vector3.up;
    }
}
