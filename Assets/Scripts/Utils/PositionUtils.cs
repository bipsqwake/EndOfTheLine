using UnityEngine;

public class PositionUtils
{
    public static bool IsOutOfScreen(Transform position) 
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        return position.position.x > screenBounds.x
        || position.position.x < screenBounds.x - Screen.width
        || position.position.y > screenBounds.y
        || position.position.y < screenBounds.y - Screen.height;
    }
}