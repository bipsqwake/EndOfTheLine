using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Transform shieldCollider;
    [SerializeField] private SpriteRenderer shieldView;
    [SerializeField] private float activationTime = 0.2f;
    [SerializeField] private float activationWidth = 0.2f;
    [SerializeField] private AudioSource sfx;

    bool active = false;
    float currentWidth;

    public void Activate(float duration, float width)
    {
        if (active)
        {
            return;
        }
        active = true;
        currentWidth = width;
        SetActive(true);

        LeanTween.value(activationWidth, width, activationTime)
        .setOnUpdate(SetWidth)
        .setOnComplete(StartCountdown, duration);
    }

    private void SetActive(bool activate)
    {
        shieldCollider.gameObject.SetActive(activate);
        shieldView.gameObject.SetActive(activate);
        if (activate)
        {
            sfx.Play();
        } else
        {
            sfx.Stop();
        }
    }

    private void Close()
    {
        active = false;
        SetActive(false);
    }
    private void SetWidth(float value)
    {
        SetColliderWidth(value);
        SetViewWidth(value);
    }

    private void SetColliderWidth(float value)
    {
        Vector3 colliderScale = shieldCollider.localScale;
        colliderScale.x = value;
        shieldCollider.localScale = colliderScale;
    }

    private void SetViewWidth(float value)
    {
        Vector2 shieldSize = shieldView.size;
        shieldSize.x = value;
        shieldView.size = shieldSize;
    }

    private void StartCountdown(object time)
    {
        StartCoroutine(Finish((float) time));
    }
    
    private IEnumerator Finish(float time)
    {
        yield return new WaitForSeconds(time);
        LeanTween.value(currentWidth, activationWidth, activationTime)
        .setOnUpdate(SetWidth)
        .setOnComplete(Close);
    }

}
