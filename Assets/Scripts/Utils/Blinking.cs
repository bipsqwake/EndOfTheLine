using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blinking : MonoBehaviour
{
    private bool on;
    private Image image;
    [SerializeField] private float period;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetOn(bool on)
    {
        if (on)
        {
            StartCoroutine(Blink());
        } else
        {
            this.on = false;
        }
    }

    private IEnumerator Blink()
    {
        WaitForSeconds ttw = new WaitForSeconds(period);
        this.on = true;
        while (on)
        {
            Color current = image.color;
            current.a = 1.0f - current.a;
            image.color = current;
            yield return ttw;
        }
        Color off = image.color;
        off.a = 0.0f;
        image.color = off;
    }
}
