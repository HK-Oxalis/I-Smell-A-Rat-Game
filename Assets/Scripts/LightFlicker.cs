using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light light;

    float intensity;
    public int maxflickerAmount = 1;
    public float flickerSpeed = 0.1f;
    public Vector2 flickerDelay = Vector2.one;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();
        intensity = light.intensity;

        StartCoroutine("Flicker");
    }

    IEnumerator Flicker ()
    {
        float delayRnd = Random.Range(flickerDelay.x, flickerDelay.y);
        int flickerAmount = Random.Range(1, maxflickerAmount);
        yield return new WaitForSeconds(delayRnd);
        for (int i = 0; i < flickerAmount; i++)
        {
            light.intensity = 0;
            yield return new WaitForSeconds(flickerSpeed);
            light.intensity = intensity;
            yield return new WaitForSeconds(flickerSpeed);
        }
        StartCoroutine("Flicker");
    }
}
