using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainManipulator : MonoBehaviour
{
    private Steam steam;
    private BreakSparks sparks;
    [SerializeField] private float delta;
    [SerializeField] private float speed;
    private int acceleration;
    private float defaultX;

    private static Dictionary<int, float> steamSpeed = new Dictionary<int, float>() { { -1, -2 }, { 0, -3 }, { 1, -4 } };
    public void Start()
    {
        SidesInputController.instance.leftEvent += Left;
        SidesInputController.instance.rightEvent += Right;
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
        float targetX = defaultX + delta * acceleration;
        if (!Mathf.Approximately(targetX, transform.position.x))
        {
            int direction = (int)Mathf.Sign(targetX - transform.position.x);
            float newX = transform.position.x + direction * speed * Time.deltaTime;
            newX = direction > 0 ? Mathf.Min(targetX, newX) : Math.Max(targetX, newX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    public void Right(bool pressed)
    {
        if (pressed && acceleration == 0)
        {
            acceleration = 1;
        }
        else if (!pressed && acceleration == 1)
        {
            acceleration = 0;
        }
        SetSteam();
    }

    public void Left(bool pressed)
    {
        if (pressed && acceleration == 0)
        {
            acceleration = -1;
            sparks.Activate(true);
        }
        else if (!pressed && acceleration == -1)
        {
            acceleration = 0;
            sparks.Activate(false);
        }
        SetSteam();
    }

    private void SetSteam()
    {
        steam.SetForce(steamSpeed[acceleration]);
    }
}
