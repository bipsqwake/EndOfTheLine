using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
public class AITrainDefencePlaner
{

    private readonly List<Threat> threats = new();
    private Train train;
    private TrainAI trainAI;
    private AITrainManipulator manipulator;

    //Move to more tweakable place
    private float moveStep = 0.1f;
    private float projectileAccuracy = 0.2f;
    private float stepCost = 2f;
    private float shieldCost = 7f;
    private float shieldRelay = 0.5f;
    public AITrainDefencePlaner(Train train, TrainAI trainAI, AITrainManipulator trainManipulator)
    {
        ThreatManager.registerProjectileThreat += RegisterProjectileThreat;
        this.train = train;
        this.trainAI = trainAI;
        this.manipulator = trainManipulator;
    }

    private void RegisterProjectileThreat(Projectile projectile)
    {
        ProjectileThreat threat = new(projectile);
        float projectileFallTime = projectile.CalculateFallTime(train.GetHeigthPoint());
        threat.SetTTI(projectileFallTime);
        threat.SetTargetX(projectile.CalculateRealX(projectileFallTime));
        DebugGizmos.AddDot(new Vector3(threat.GetTargetX(), train.GetHeigthPoint(), 0.0f));
        threats.Add(threat);
        trainAI.SetDefencePlan(UpdatePlan());
    } 

    public void UpdateTTI(float delta)
    {
        int threatsCount = threats.Count;
        foreach (Threat threat in threats)
        {
            threat.ReduceTTI(delta);
        }
        threats.RemoveAll(threat => threat.Finished());
        if (threatsCount != threats.Count)
        {
            trainAI.SetDefencePlan(UpdatePlan());
            DebugGizmos.Clear();
        }
    }

    private DefencePlan UpdatePlan()
    {
        if (threats.Count == 0)
        {
            Debug.Log("Default Plan");
            return GetDefaultPlan();
        }
        List<DefencePlan> defencePlans = GenerateDefenceCandidates();
        DefencePlan result = null;
        defencePlans.ForEach(plan =>
        {
            plan.Impact = 0.0f;
            foreach(Threat threat in threats)
            {
                plan.Impact += CalculateThreatImpact(threat, plan);
            }
            if (result == null || plan.GetScore(trainAI.GetImpactWeigth(), trainAI.GetCostWeigth()) < result.GetScore(trainAI.GetImpactWeigth(), trainAI.GetCostWeigth()))
            {
                result = plan;
            }
            Debug.Log(plan + " " + plan.GetScore(trainAI.GetImpactWeigth(), trainAI.GetCostWeigth()));
        });
        if (result != null)
        {
            Debug.Log("Selected plan: " + result + " Score: " + result.GetScore(trainAI.GetImpactWeigth(), trainAI.GetCostWeigth()));
        }
        return result == null ? GetDefaultPlan() : result;
    }

    private float CalculateThreatImpact(Threat threat, DefencePlan plan)
    {
        if (threat.GetType() == typeof(ProjectileThreat))
        {
            return CalculateProjectileThreatImpact((ProjectileThreat) threat, plan);
        }
        return 0.0f;
    }

    private float CalculateProjectileThreatImpact(ProjectileThreat threat, DefencePlan defencePlan)
    {   
        float shift = manipulator.GetPosition() - defencePlan.Position;
        float max = 0.0f;
        for (int i = -1; i <= 1; i++)
        {
            float target = threat.GetTargetX() + shift + projectileAccuracy * i;
            bool shieldProtection = false;
            foreach (var shieldActivation in defencePlan.ShieldActivations)
            {
                if (shieldActivation.Delay > threat.GetTTI())
                {
                    continue;
                }
                AIArmorCart cart = shieldActivation.Cart;
                if (IsProtectedByShield(target, cart))
                {
                    shieldProtection = true;
                    break;
                }
            }
            foreach (AIArmorCart cart in trainAI.GetShieldCarts())
            {
                if (cart.GetActiveTimeLeft() > threat.GetTTI() && IsProtectedByShield(target, cart))
                {
                    shieldProtection = true;
                    break;
                } 
            }
            if (shieldProtection)
            {
                continue;
            }
            int cartIndex = train.GetPartPositionByX(target); 
            defencePlan.CartNum = cartIndex;
            if (cartIndex == -1)
            {
               continue;
            }
            AICarriage aiCarriage = trainAI.GetAICarriageByIndex(cartIndex);
            TrainPart trainPart = aiCarriage.GetInstance();
            max = Mathf.Max(CalculateTrainPartImpact(aiCarriage, threat.GetProjectile().GetDamage(), trainPart.IsCarriageDestroyed()), max);
        }
        return max;
    }

    private bool IsProtectedByShield(float target, AIArmorCart cart)
    {
        return target < cart.GetInstance().transform.position.x + cart.GetShieldWidth() / 2 && 
            target > cart.GetInstance().transform.position.x - cart.GetShieldWidth() / 2;
    }

    private float CalculateTrainPartImpact(AICarriage aiCarriage, float damage, bool cart)
    {
        if (aiCarriage == null)
        {
            return 0.0f;
        }
        TrainPart trainPart = aiCarriage.GetInstance();
        return (cart ? 30.0f : aiCarriage.GetImportance()) * damage * CustomMath.ExpDec(2.0f, 0.5f, trainPart.GetHealthPercent());
    }

    private List<DefencePlan> GenerateDefenceCandidates()
    {

        List<DefencePlan> result = new();
        float current = manipulator.GetPosition();
        float nextStep = -manipulator.GetDelta();
        List<List<DefencePlan.ShieldActivation>> shieldActivations = GenerateShieldActivations(trainAI.GetShieldCarts());
        while (nextStep < manipulator.GetDelta() + moveStep) 
        {
            float moveCost = Mathf.Abs(current - nextStep) * stepCost / moveStep;
            // result.Add(new(nextStep, moveCost));
            foreach (var shieldActivation in shieldActivations)
            {
                result.Add(new(nextStep, moveCost + shieldCost * (shieldActivation.Count == 0 ? 0 : 1), shieldActivation));
            }
            nextStep += moveStep;
        }
        Debug.Log(trainAI.GetShieldCarts().Count);
        return result;
    }

    private List<List<DefencePlan.ShieldActivation>> GenerateShieldActivations(List<AIArmorCart> carts)
    {
        if (carts.Count == 0)
        {
            return new();
        }
        AIArmorCart currentCart = carts[0];
        carts.Remove(currentCart);
        List<List<DefencePlan.ShieldActivation>> result = new();
        List<List<DefencePlan.ShieldActivation>> next = GenerateShieldActivations(carts);
        List<List<DefencePlan.ShieldActivation>> current = GenerateCartShieldActivations(currentCart);
        if (next.Count == 0)
        {
            return current;
        }
        foreach (var left in current)
        {
            foreach (var right in next)
            {
                List<DefencePlan.ShieldActivation> toAdd = new();
                toAdd.AddRange(left);
                toAdd.AddRange(right);
                result.Add(toAdd);
            }
        }
        return result;

    }

    private List<List<DefencePlan.ShieldActivation>> GenerateCartShieldActivations(AIArmorCart cart)
    {
        List<List<DefencePlan.ShieldActivation>> result = new()
        {
            new()
        };
        foreach (Threat threat in threats)
        {
            if (threat.GetType() != typeof(ProjectileThreat))
            {
                continue;
            }
            ProjectileThreat projectileThreat = (ProjectileThreat) threat;
            float timeToActivate = projectileThreat.GetTTI() - shieldRelay;
            Debug.Log("reload " + cart.GetReloadTimeLeft());
            if (timeToActivate < 0 || cart.GetReloadTimeLeft() > timeToActivate)
            {
                continue;
            }
            result.Add(new(){new DefencePlan.ShieldActivation(cart, timeToActivate)});
        }
        return result;

    }

    private DefencePlan GetDefaultPlan()
    {
        return new DefencePlan(0.0f, 0.0f);
    }
}