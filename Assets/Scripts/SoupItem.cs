using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoupItem : MonoBehaviour
{
    [Tooltip("plese keep it lowercase LOL")]
    public string itemName = "new item";

    bool isUp = false;
    private void Start()
    {
        StartCoroutine(Wobble());
        StartCoroutine(Rotate());
    }
    float EaseInOutSine(float x) {
        return -(Mathf.Cos(Mathf.PI* x) - 1) / 2;
    }
    private IEnumerator Wobble()
    {
        while (true)
        {
            float time = Random.Range(1f, 2f);
            StartCoroutine(InterpolateHeight(time, transform.localPosition.y, Random.Range(-1f,0f)));
            yield return new WaitForSeconds(time);
            
            time = Random.Range(1f, 2f);
            StartCoroutine(InterpolateHeight(time, transform.localPosition.y, Random.Range(0f, 1f)));
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator Rotate()
    {
        while (true)
        {
            float time = Random.Range(2f, 10f);
            StartCoroutine(InterpolateRotation(time));
            yield return new WaitForSeconds(time);
        }
    }
    private IEnumerator InterpolateHeight(float time, float startPos, float endPos)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float fac = t / time;
            transform.localPosition = new Vector3(transform.position.x, Mathf.Lerp(startPos, endPos, EaseInOutSine(fac)), transform.position.z);
            yield return null;
        }
    }
    private IEnumerator InterpolateRotation(float time)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 360), Random.Range(0, 180));

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float fac = t / time;
            transform.rotation = Quaternion.Slerp(startRot, endRot, EaseInOutSine(fac));
            yield return null;
        }
    }
}
