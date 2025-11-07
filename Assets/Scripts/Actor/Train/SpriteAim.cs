using UnityEngine;

public class SpriteAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float upDistance;
    [SerializeField] private float resetTime;

    private float defaultY;
    private float currentY;

    public void Awake()
    {
        defaultY = transform.localPosition.y;
        SetVisible(0.0f);
    }

    public void Apply(Vector2 aim)
    {
        ApplyY(aim.y);
        currentY = aim.y;
    }

    public void Reset()
    {
        Vector3 target = new(0.0f, defaultY, 0.0f);
        LeanTween.value(currentY, 0.0f, resetTime)
        .setOnUpdate(ApplyY);
    }

    public void InstantClose()
    {
        Apply(Vector2.zero);
    }

    private void ApplyY(float y)
    {
        float clamped = Mathf.Clamp01(y);
        SetVisible(clamped);
        SetY(clamped);
    }

    private void SetVisible(float value)
    {
        if (Mathf.Approximately(value, 0.0f))
        {
            sprite.gameObject.SetActive(false);
        }
        else
        {
            sprite.gameObject.SetActive(true);
            SetAlpha(value);
        }
    }

    private void SetAlpha(float value)
    {
        Color spriteColor = sprite.color;
        spriteColor.a = value;
        sprite.color = spriteColor;
    }

    private void SetY(float value)
    {
        transform.localPosition = new Vector3(0.0f, defaultY + value * upDistance, 0.0f);
    }
}
