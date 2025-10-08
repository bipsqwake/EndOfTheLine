using System;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private GameObject aimArrow;
    [SerializeField] private GameObject aimCurve;
    [SerializeField] private float upDistance;
    [SerializeField] private float arrowAngleAmp;
    [SerializeField] private float resetTime;

    private Transform aimArrowTransform;
    private SpriteRenderer aimArrowRenderer;
    private SpriteRenderer aimCurveRenderer;
    private float defaultY;

    public void Awake()
    {
        if (aimArrow == null)
        {
            throw new ArgumentNullException("Aim Arrow is null");
        }
        if (aimCurve == null)
        {
            throw new ArgumentNullException("Aim Curve is null");
        }
        aimArrowTransform = aimArrow.GetComponent<Transform>();
        if (aimArrowTransform == null)
        {
            throw new ArgumentNullException("Aim arrow should have transform");
        }
        aimArrowRenderer = aimArrow.GetComponent<SpriteRenderer>();
        if (aimArrowRenderer == null)
        {
            throw new ArgumentNullException("Aim arrow should have sprite renderer");
        }
        aimCurveRenderer = aimCurve.GetComponent<SpriteRenderer>();
        if (aimCurveRenderer == null)
        {
            throw new ArgumentNullException("Aim curve should have sprite renderer");
        }
        defaultY = transform.localPosition.y;
        SetVisible(0.0f);
    }

    public void Apply(Vector2 aim)
    {
        ApplyY(aim.y);
        SetAngle(aim.x);
    }

    public void Reset()
    {
        Vector3 target = new(0.0f, defaultY, 0.0f);
        LeanTween.moveLocal(gameObject, target, resetTime);
        LeanTween.alpha(aimArrow, 0.0f, resetTime);
        LeanTween.alpha(aimCurve, 0.0f, resetTime);
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
            aimArrow.SetActive(false);
            aimCurve.SetActive(false);
        }
        else
        {
            aimArrow.SetActive(true);
            aimCurve.SetActive(true);

            Color arrowColor = aimArrowRenderer.color;
            arrowColor.a = value;
            aimArrowRenderer.color = arrowColor;

            Color curveColor = aimCurveRenderer.color;
            curveColor.a = value;
            aimCurveRenderer.color = curveColor;
        }
    }

    private void SetY(float value)
    {
        transform.localPosition = new Vector3(0.0f, defaultY + value * upDistance, 0.0f);
    }

    private void SetAngle(float value)
    {
        aimArrowTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f - value * arrowAngleAmp);
    }
}
