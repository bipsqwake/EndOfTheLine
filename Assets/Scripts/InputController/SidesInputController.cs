using System;
using System.Collections.Generic;
using UnityEngine;

public class SidesInputController : MonoBehaviour
{
    public static SidesInputController instance;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    public event Action<bool> leftEvent;
    public event Action<bool> rightEvent;

//AI Generated
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                HandleTouch(touch.position, touch.phase);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(Input.mousePosition, TouchPhase.Began);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(Input.mousePosition, TouchPhase.Ended);
            }
        }
    }

    void HandleTouch(Vector2 position, TouchPhase phase)
    {
        float screenWidth = Screen.width;
        bool isLeft = position.x < screenWidth / 2f;

        if (phase == TouchPhase.Began)
        {
            if (isLeft)
                leftEvent?.Invoke(true);
            else
                rightEvent?.Invoke(true);
        }
        else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
        {
            if (isLeft)
                leftEvent?.Invoke(false);
            else
                rightEvent?.Invoke(false);
        }
    }


}
