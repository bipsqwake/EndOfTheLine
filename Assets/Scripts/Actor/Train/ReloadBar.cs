using System;
using System.Collections;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    [SerializeField] private float step = 0.065f;
    [SerializeField] private int maxValue;
    private float zeroX;

    private bool ready = false;

    public void Awake()
    {
        zeroX = bar.localPosition.x;
    }

    public void SetReady()
    {
        StopAllCoroutines();
        SetValue(maxValue);
    }

    public void Reload(float reloadTime)
    {
        ready = false;
        StopAllCoroutines();
        StartCoroutine(ReloadCoroutine(reloadTime));
    }
    
    public bool IsReady()
    {
        return ready;
    }

    private void SetValue(int value)
    {
        if (value < 0 || value > maxValue)
        {
            throw new ArgumentOutOfRangeException("reload bar value should be between 0 and " + maxValue);
        }
        bar.localPosition = new Vector3(zeroX + value * step, bar.localPosition.y, 0.0f);
        if (value == maxValue)
        {
            ready = true;
        }
    }

    private IEnumerator ReloadCoroutine(float time)
    {
        for (int i = 0; i < maxValue; i++)
        {
            SetValue(i);
            yield return new WaitForSeconds(time / maxValue);
        }
        SetValue(maxValue);
    }

    
    


}
