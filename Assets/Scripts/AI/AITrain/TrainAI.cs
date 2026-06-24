using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Train), typeof(AITrainManipulator))]
public class TrainAI : AI
{
    private float impactWeigth;
    private Train train;
    private AITrainManipulator trainManipulator;
    private AITrainDefencePlaner defencePlaner;
    private DefencePlan defencePlan;
    private RetreatDecisionStrategies.Strategy retreatStrategy;
    [SerializeField] private float retreatTime = 5.0f;
    public void Awake()
    {
        train = GetComponent<Train>();
        trainManipulator = GetComponent<AITrainManipulator>();
    }
    public void FixedUpdate()
    {
        defencePlaner.UpdateTTI(Time.fixedDeltaTime);
        defencePlan?.ReduceShieldDelay(Time.fixedDeltaTime);
    }

    public void Update()
    {
        PerformDefencePlan();
    }

    public void Initialize(AITrainConfiguration configuration)
    {
        defencePlaner = new AITrainDefencePlaner(train, this, trainManipulator);
        impactWeigth = configuration.GetImpactWeigth();
        retreatStrategy = configuration.GetRetreatStrategy();
        foreach (AITrainPartConfiguration partConfig in configuration.GetConfig())
        {
            TrainPart instance = train.InstantiatePart(CarriagePrefabHolder.instance.GetPrefab(partConfig.GetCarriageType()));
            instance.SetConfiguration(partConfig);
            instance.SetCartImportance(configuration.GetCartImportance());
            instance.trainAI = this;
            if (instance.GetCarriageType() != CarriageType.LOCOMOTIVE)
            {
                CarriagePayload carriagePayload = instance.GetCarriagePayload();
                if (carriagePayload.GetActionType().Contains(AIActionType.COROUTINE))
                {
                    StartCoroutine(carriagePayload.CoroutineAction());
                }
            }
        }
        trainManipulator.Init();
    }

    public void FixPosition()
    {
        trainManipulator.FixPosition();
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
                sa.Cart.EnemyActivate();
                sa.Activate();
            }
        }
    }

    public void SetDefencePlan(DefencePlan plan)
    {
        this.defencePlan = plan;
    }

    //Getters

    public float GetImpactWeigth()
    {
        return impactWeigth;
    }

    public float GetCostWeigth()
    {
        return 1.0f - impactWeigth;
    }

    public void DamageCallback()
    {
        if (IsRetreat())
        {
            Retreat();
        }
    }

    private bool IsRetreat()
    {
        RetreatDecision retreatDecision = RetreatDecisionStrategies.GetDecision(retreatStrategy);
        return train.GetLocomotive().IsDestroyed() || retreatDecision.ShouldRetreat(train);
    }

    private void Retreat()
    {
        LeanTween.move(gameObject, GlobalSettings.instance.enemyOutPosition.position, retreatTime)
        .setOnComplete(() => EnemyManager.instance.EndBattle());
    }
}
