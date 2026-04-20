using System;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerTargetSelector : PlayerTargetSelector
{
    private readonly Dictionary<CarriageType, int> typeWeigths = new()
    {
        {CarriageType.LOCOMOTIVE, 15},
        {CarriageType.COAL, 5},
        {CarriageType.MORTAIR, 20},
        {CarriageType.SHIELD, 20},
        {CarriageType.PASSANGER, 10},
        {CarriageType.CART, 10}
    };

    private float damageK = 1;
    private float cartCarryK = 4;
    private float carriageCarryK = 1;
    public Dictionary<Actor, int> GetPlayerTrainWeght()
    {
        Dictionary<Actor, int> result = new();

        Train playerTrain = State.instance.GetPlayerTrain();
        List<TrainPart> playerTrainParts = playerTrain.GetParts();
        for (int i = 0; i < playerTrainParts.Count; i++)
        {
            TrainPart current = playerTrainParts[i];
            int currentWeigth = 0;
            CarriageType currentType = current.IsCarriageDestroyed() ? CarriageType.CART : current.GetCarriageType();
            Actor actor = CarriageType.CART.Equals(currentType) ? current.GetCart() : current.GetActor();
            if (!typeWeigths.ContainsKey(currentType))
            {
                throw new ArgumentOutOfRangeException("Type " + currentType + " is unknown to strategy");
            }
            currentWeigth += typeWeigths[currentType];
            currentWeigth += (int)(current.GetDamage() * damageK);
            if (CarriageType.CART.Equals(currentType))
            {
                currentWeigth += (int)(i * cartCarryK);
            } else
            {
                currentWeigth += (int)(i * carriageCarryK);
            }
            result.Add(actor, currentWeigth);
        }

        return result;
    }
}