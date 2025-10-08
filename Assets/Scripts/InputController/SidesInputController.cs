using System;
using System.Collections.Generic;
using UnityEngine;

public class SidesInputController : MonoBehaviour
{
    public static SidesInputController instance;
    [Range(0.0f, 1.0f)]
    public float sidesPart;

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
    //Oh, I see a problem here
    //If touch starts right and finished left something will happen
    //Fix later
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
        bool isLeft = position.x < screenWidth * sidesPart;
        bool isRight = position.x > screenWidth * (1 - sidesPart);

        if (phase == TouchPhase.Began)
        {
            if (isLeft)
                leftEvent?.Invoke(true);
            else if (isRight)
                rightEvent?.Invoke(true);
        }
        else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
        {
            if (isLeft)
                leftEvent?.Invoke(false);
            else if (isRight)
                rightEvent?.Invoke(false);
        }
    }


}
