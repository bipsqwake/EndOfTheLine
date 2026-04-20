using System;
using System.Collections.Generic;
using UnityEngine;

public class AITrainManipulator : MonoBehaviour
{
    private Steam steam;
    private BreakSparks sparks;
    [SerializeField] private float delta;
    [SerializeField] private float speed;
    private int acceleration;
    private float defaultX;

    private float targetX;

    private static Dictionary<int, float> steamSpeed = new Dictionary<int, float>() { { -1, -2 }, { 0, -3 }, { 1, -4 } };
    public void Start()
    {
        defaultX = transform.position.x;

        Locomotive locomotive = GetComponent<Train>().GetLocomotive();
        if (locomotive == null)
        {
            throw new ArgumentException("No locomotive in train");
        }
        steam = locomotive.GetComponent<Steam>();
        if (steam == null)
        {
            throw new ArgumentException("No steam at locomotive");
        }
        sparks = locomotive.GetComponent<BreakSparks>();
        if (sparks == null)
        {
            throw new ArgumentException("No sparks at locomotive");
        }
    }

    public void Update()
    {
        float targetWorldX = defaultX + targetX;
        if (!Mathf.Approximately(targetWorldX, transform.position.x))
        {
            acceleration = (int)Mathf.Sign(targetWorldX - transform.position.x);
            float newX = transform.position.x + acceleration * speed * Time.deltaTime;
            newX = acceleration > 0 ? Mathf.Min(targetWorldX, newX) : Math.Max(targetWorldX, newX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        } 
        else
        {
            acceleration = 0;
        }
        sparks.Activate(acceleration == -1);
        
        SetSteam();
    }

    public void SetTargetX(float targetX)
    {
        this.targetX = targetX;
    }

    //Get info

    public float GetPosition()
    {
        return transform.position.x - defaultX;
    }

    public float GetDelta()
    {
        return delta;
    }

    private void SetSteam()
    {
        steam.SetForce(steamSpeed[acceleration]);
    }
}
