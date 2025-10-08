using UnityEngine;
using UnityEngine.Events;

public class DragUpAction : MonoBehaviour
{
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private UnityEvent<Vector2> prepareEvent;
    [SerializeField] private UnityEvent<Vector2> actionEvent;
    private Vector2 startTouchPosition;
    private bool isTouching = false;

    void Update()
    {
        if (isTouching)
        {
            ProcessTouching();
        }
        DetectTouching();
    }

    private void ProcessTouching()
    {
        prepareEvent.Invoke(GetBoundedDistance());
    }

    private void ReleaseTouching()
    {
        actionEvent.Invoke(GetBoundedDistance());
    }

    //AI Generated
    private void DetectTouching()
    {
        // Для мобильных устройств
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchOnCollider(touchPos))
                    {
                        startTouchPosition = touchPos;
                        isTouching = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isTouching)
                    {
                        ReleaseTouching();
                        isTouching = false;
                    }
                    break;
            }
        }
        // Для ПК (мышь)
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (IsTouchOnCollider(mousePos))
                {
                    startTouchPosition = mousePos;
                    isTouching = true;
                }
            }
            if (Input.GetMouseButtonUp(0) && isTouching)
            {
                ReleaseTouching();
                isTouching = false;
            }
        }
    }


    private Vector2 GetPointerPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private bool IsTouchOnCollider(Vector2 pos)
    {
        Collider2D col = GetComponent<Collider2D>();
        return col == Physics2D.OverlapPoint(pos);
    }

    private Vector2 GetBoundedDistance()
    {
        Vector2 touchPos = GetPointerPosition();
        Vector2 distance = touchPos - startTouchPosition;
        return new Vector2(Mathf.Clamp(distance.x, -boundaries.x, boundaries.x) / boundaries.x, Mathf.Clamp(distance.y, -boundaries.y, boundaries.y) / boundaries.y);
    }
}
