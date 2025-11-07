using UnityEngine;

public class ReleasedPart : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxDistance;

    private float speed = 0;
    public void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        speed += acceleration * Time.deltaTime;
        if (Mathf.Abs(transform.localPosition.x) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
