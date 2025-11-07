using System;
using UnityEngine;

public class LineAim : MonoBehaviour
{
    [SerializeField] private GameObject aimArrow;
    [SerializeField] private GameObject aimCurve;
    [SerializeField] private float upDistance;
    [SerializeField] private float arrowAngleAmp;
    [SerializeField] private float resetTime;

    private Transform aimArrowTransform;
    private LineRenderer aimArrowRenderer;
    private LineRenderer aimCurveRenderer;
    private float defaultY;
    private float currentY;

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
        aimArrowRenderer = aimArrow.GetComponent<LineRenderer>();
        if (aimArrowRenderer == null)
        {
            throw new ArgumentNullException("Aim arrow should have line renderer");
        }
        aimCurveRenderer = aimCurve.GetComponent<LineRenderer>();
        if (aimCurveRenderer == null)
        {
            throw new ArgumentNullException("Aim curve should have line renderer");
        }
        defaultY = transform.localPosition.y;
        SetVisible(0.0f);
    }

    public void Apply(Vector2 aim)
    {
        ApplyY(aim.y);
        SetAngle(aim.x);
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
            aimArrow.SetActive(false);
            aimCurve.SetActive(false);
        }
        else
        {
            aimArrow.SetActive(true);
            aimCurve.SetActive(true);
            SetAlpha(value);
        }
    }

    private void SetAlpha(float value)
    {
        Color arrowColorS = aimArrowRenderer.startColor;
        Color arrowColorE = aimArrowRenderer.endColor;
        arrowColorS.a = value;
        arrowColorE.a = value;
        aimArrowRenderer.startColor = arrowColorS;
        aimArrowRenderer.endColor = arrowColorE;

        Color curveColorS = aimCurveRenderer.startColor;
        Color curveColorE = aimCurveRenderer.endColor;
        curveColorS.a = value;
        curveColorE.a = value;
        aimCurveRenderer.startColor = curveColorS;
        aimCurveRenderer.endColor = curveColorE;
    }

    private void SetY(float value)
    {
        transform.localPosition = new Vector3(0.0f, defaultY + value * upDistance, 0.0f);
    }

    private void SetAngle(float value)
    {
        aimArrowTransform.rotation = Quaternion.Euler(0.0f, 0.0f, -value * arrowAngleAmp);
    }
}
