using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Vector3 posDelta;
    [SerializeField] private float timeDelta;
    [SerializeField] private float periodRangeFrom;
    [SerializeField] private float periodRangeTo;

    private float nextJumpIn = 0;
    private bool jump = false;

    public void Start()
    {
        nextJumpIn = Random.Range(periodRangeFrom, periodRangeTo);
    }

    public void Update()
    {
        nextJumpIn -= Time.deltaTime;
        if (!jump)
        {
            if (nextJumpIn < 0)
            {
                jump = true;
                nextJumpIn = timeDelta;
                transform.position = transform.position + posDelta;
            }
        }
        else
        {
            if (nextJumpIn < 0)
            {
                jump = false;
                nextJumpIn = Random.Range(periodRangeFrom, periodRangeTo);
                transform.position = transform.position - posDelta;
            }
        }
        
    }


}
