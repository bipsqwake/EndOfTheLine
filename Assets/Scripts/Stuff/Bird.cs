using UnityEngine;

public class Bird : MonoBehaviour
{
    private static string OFFSET_PARAM = "Offset";
    private static string EAT_TRIGGER = "Eat";
    private static string FLY_BOOL = "Fly";
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 0.15f;
    [SerializeField] private bool flyToPosition = false;
    [SerializeField] private Transform flyPosition;
    [SerializeField] private float flyToPositionTime = 2f;
    private bool flyingToDirection = false;
    
    private Vector3 flyDirection;

    void Awake()
    {
        animator.SetFloat(OFFSET_PARAM, Random.Range(0.0f, 1.0f));
        if (flyToPosition)
        {
            SetFlyToPosition();
        }
    }

    void Update()
    {
        if (flyingToDirection)
        {
            MoveTowardsDirection();
            if (PositionUtils.IsOutOfScreen(transform))
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetFlyToPosition()
    {
        flyToPosition = true;
        spriteRenderer.flipX = transform.position.x > flyPosition.position.x;
        animator.SetBool(FLY_BOOL, true);
        LeanTween.move(gameObject, flyPosition, 2).setEaseOutQuad().setOnComplete(() => Seat());
    }
    public void MaybeChangeDirection()
    {
        if (Random.Range(0, 2) == 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    public void MaybeEat()
    {
        if (Random.Range(0, 3) == 0)
        {
            animator.SetTrigger(EAT_TRIGGER);
        }
    }

    public void FlyAway()
    {
        flyToPosition = false;
        flyingToDirection = true;
        animator.SetBool(FLY_BOOL, true);
        flyDirection = new Vector3(Random.Range(0, 2) == 0 ? 1 : -1 * Random.Range(0.5f, 1f), 1.0f, 0.0f).normalized;
        spriteRenderer.flipX = flyDirection.x < 0;
    }

    public void MoveTowardsDirection()
    {
        transform.position += flyDirection * speed;
    }

    public void Seat()
    {
        flyToPosition = false;
        animator.SetBool(FLY_BOOL, false);
    }
    

}
