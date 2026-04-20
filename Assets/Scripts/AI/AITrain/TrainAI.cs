using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Train), typeof(AITrainManipulator))]
public class TrainAI : AI
{
    [SerializeField] private List<AICarriage> carriages;
    [SerializeField] private float cartImportance;
    [Range(0, 1)]
    [SerializeField] private float impactWeigth;

    private Train train;
    private AITrainManipulator trainManipulator;
    private AITrainDefencePlaner defencePlaner;
    private DefencePlan defencePlan;
    public void Awake()
    {
        train = GetComponent<Train>();
        trainManipulator = GetComponent<AITrainManipulator>();
        Initialize();
    }
    public void FixedUpdate()
    {
        defencePlaner.UpdateTTI(Time.fixedDeltaTime);
        if (defencePlan != null)
        {
            defencePlan.ReduceShieldDelay(Time.fixedDeltaTime);
        }
    }

    public void Update()
    {
        PerformDefencePlan();
    }

    public void Initialize()
    {
        defencePlaner = new AITrainDefencePlaner(train, this, trainManipulator);
        foreach (AICarriage aICarriage in carriages)
        {
            TrainPart instance = train.InstantiatePart(aICarriage.carriagePrefab);
            aICarriage.SetInstance(instance);
        }
        foreach (AICarriage aICarriage in carriages)
        {
            aICarriage.Init();
            if (aICarriage.GetActionType().Contains(AIActionType.COROUTINE))
            {
                StartCoroutine(aICarriage.CoroutineAction());
            }
        }
    }

    //Defence Plan

    private void PerformDefencePlan()
    {
        PerformDefencePlanMovement();
        PerformDefencePlanShield();
    }

    private void PerformDefencePlanMovement()
    {
        if (defencePlan == null)
        {
            return;
        }
        trainManipulator.SetTargetX(defencePlan.Position);
    }

    private void PerformDefencePlanShield()
    {
        if (defencePlan == null)
        {
            return;
        }
        foreach (var sa in defencePlan.ShieldActivations)
        {
            if (!sa.Activated && sa.Delay < 0)
            {
                Debug.Log("Here");
                sa.Cart.Activate();
                sa.Activate();
            }
        }
    }

    public void SetDefencePlan(DefencePlan plan)
    {
        this.defencePlan = plan;
    }

    //Getters

    public AICarriage GetAICarriageByIndex(int index)
    {   
        if (index < 0 || index >= carriages.Count)
        {
            return null;
        }
        return carriages[index];
    }

    public List<AIArmorCart> GetShieldCarts()
    {
        return carriages.FindAll(c => c.GetType() == typeof(AIArmorCart)).ConvertAll(c => (AIArmorCart)c);
    }

    public float GetImpactWeigth()
    {
        return impactWeigth;
    }

    public float GetCostWeigth()
    {
        return 1.0f - impactWeigth;
    }
}
