using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam;
    public float acceleration = 4f;
    public float maxSpeed = 5f;
    public float soupFriction = 1f;
    Vector3 velocity;
    public Quaternion tilt;
    public float spin = 0f;
    public float spinSpeed;

    public bool isLocked = false;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        StartCoroutine(Spin());
        StartCoroutine(Tilt());
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && !isLocked)
        {
            Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePoint = mouseCast.GetPoint(
                -Vector3.Dot(mouseCast.origin - Vector3.zero, Vector3.up)/
                Vector3.Dot(mouseCast.direction, Vector3.up));
            Vector3 direction = (mousePoint - transform.position).normalized;
            velocity += direction * acceleration;
            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
        }
        velocity -= velocity * soupFriction * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        spin += spinSpeed;
        transform.rotation = tilt * Quaternion.Euler(0, spin, 0);
    }
    private void OnDrawGizmos()
    {
        Ray mouseCast = cam.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(mouseCast);
        Vector3 mousePoint = mouseCast.GetPoint(Vector3.Dot(mouseCast.direction, Vector3.up));
        Gizmos.DrawSphere(mousePoint, 0.1f);
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            float time = Random.Range(2f, 5f);
            StartCoroutine(InterpolateRotation(time));
            yield return new WaitForSeconds(time);
        }
    }
    public static Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    private IEnumerator Tilt()
    {
        Vector2 direction = Random.insideUnitCircle;
        while (true)
        {
            float time = Random.Range(1f, 2f);
            float angle = Random.Range(-0.3f, 0.3f);
            direction = Rotate(-direction, angle);
            StartCoroutine(InterpolateHorizontalRotation(time, direction));
            yield return new WaitForSeconds(time);
        }
    }

    float EaseInOutSine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

    private IEnumerator InterpolateRotation(float time)
    {
        float startSpinSpeed = spinSpeed; 
        float endSpinSpeed = Random.Range(-2f, 2f);

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float fac = t / time;
            spinSpeed = Mathf.Lerp(startSpinSpeed, endSpinSpeed, EaseInOutSine(fac));
            yield return null;
        }
    }
    private IEnumerator InterpolateHorizontalRotation(float time, Vector2 direction)
    {
        float intensity = velocity.magnitude * 100 + 2;

        Vector3 startRot = (tilt.eulerAngles);
        Vector3 endRot = new Vector3(Random.Range(0, 1f) * direction.x, 0, Random.Range(0, 1f) * direction.y);

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float fac = t / time;
            intensity = velocity.magnitude * 100 + 2;

            if (endRot.x * intensity > 180)
            {
                endRot.x = (endRot.x - 360) / intensity;
                direction.x = -direction.x;
            }
            if (endRot.x * intensity < -180)
            {
                endRot.x = (endRot.x + 360) / intensity;
                direction.x = -direction.x;
            }
            if (endRot.x * intensity > 180)
            {
                endRot.z = (endRot.x - 360) / intensity;
                direction.y = -direction.y;
            }
            if (endRot.z * intensity < -180)
            {
                endRot.z = (endRot.x + 360) / intensity;
                direction.y = -direction.y;
            }

            tilt = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot * intensity), EaseInOutSine(fac));
            yield return null;
        }
    }
}
