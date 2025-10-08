using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float defaultDuration = 0.25f;
    [SerializeField] private float defaultMagnitude = 0.4f;

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(
            duration < 0 ? defaultDuration : duration,
            magnitude < 0 ? defaultMagnitude : magnitude
        ));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 startPos = transform.localPosition;
        float t = 0f;

        while (t < duration)
        {
            float damper = 1f - (t / duration);
            Vector3 offset = Random.insideUnitCircle * (magnitude * damper);
            transform.localPosition = startPos + offset;

            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
