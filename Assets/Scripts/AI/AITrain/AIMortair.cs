using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIMortair", menuName = "AICarriage/AIMortair")]
public class AIMortair : AICarriage
{
    [SerializeField] private float startDelayMin;
    [SerializeField] private float startDelayMax;
    [SerializeField] private float reloadMin;
    [SerializeField] private float reloadMax;
    [SerializeField] private PlayerTargetSelectorStrategies.Strategy strategy;
    public override IEnumerator CoroutineAction()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(startDelayMin, startDelayMax));
        while (!instance.GetActor().IsDestroyed())
        {
            // AttackCycle();
            yield return new WaitForSeconds(UnityEngine.Random.Range(reloadMin, reloadMax));
        }
    }

    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] { AIActionType.COROUTINE };
    }

    private void AttackCycle()
    {
        Actor target = SelectTarget();
        Vector3 targetCoord = SelectTargetCoord(target);
        Mortair instMortair = (Mortair)instance.GetCarriagePayload();
        instMortair.PerformTargetAttack(targetCoord);
    }

    private Actor SelectTarget()
    {
        Dictionary<Actor, int> playerWeigth = PlayerTargetSelectorStrategies.GetSelector(strategy).GetPlayerTrainWeght();
        int sum = 0;
        foreach (var part in playerWeigth)
        {
            sum += part.Value;
        }
        int rand = UnityEngine.Random.Range(0, sum);
        foreach (var part in playerWeigth)
        {
            if (rand < part.Value)
            {
                return part.Key;
            }
            rand -= part.Value;
        }
        return null;
    }

    private Vector3 SelectTargetCoord(Actor target)
    {
        if (target == null)
        {
            throw new ArgumentNullException("Target should not be null");
        }
        return target.transform.position;
    }
}
