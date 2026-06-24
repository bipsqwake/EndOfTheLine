using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Vector3 posDelta;
    [SerializeField] private float timeDelta;
    [SerializeField] private float periodRangeFrom;
    [SerializeField] private float periodRangeTo;

    private float nextJumpIn = 0;
    private bool jump = false;
    private bool active = true;
    private Vector3 defaultPosition;

    public void Start()
    {
        nextJumpIn = Random.Range(periodRangeFrom, periodRangeTo);
        defaultPosition = transform.localPosition;
    }

    public void Update()
    {
        if (!active)
        {
            return;
        }
        nextJumpIn -= Time.deltaTime;
        if (!jump)
        {
            if (nextJumpIn < 0)
            {
                jump = true;
                nextJumpIn = timeDelta;
                transform.localPosition = defaultPosition + posDelta;
            }
        }
        else
        {
            if (nextJumpIn < 0)
            {
                jump = false;
                nextJumpIn = Random.Range(periodRangeFrom, periodRangeTo);
                transform.localPosition = defaultPosition;
            }
        }
        
    }

    public void SetJumperActive(bool active)
    {
        if (!active)
        {
            transform.localPosition = defaultPosition;
        } else
        {
            nextJumpIn = Random.Range(periodRangeFrom, periodRangeTo);
        }
        this.active = active;
    }


}
