using System.Collections.Generic;
using UnityEngine;

public class CarriagePrefabHolder : MonoBehaviour
{
    public static CarriagePrefabHolder instance;

    [SerializeField] private TrainPart locomotive;
    [SerializeField] private TrainPart coalCart;
    [SerializeField] private TrainPart passangerCart;
    [SerializeField] private TrainPart mortait;
    [SerializeField] private TrainPart shield;

    private Dictionary<CarriageType, TrainPart> prefabsMap = new();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }    
        instance = this;
        prefabsMap = new ()
        {
            {CarriageType.LOCOMOTIVE, locomotive},
            {CarriageType.COAL, coalCart},
            {CarriageType.PASSANGER, passangerCart},
            {CarriageType.MORTAIR, mortait},
            {CarriageType.SHIELD, shield}
        };
    }

    public TrainPart GetPrefab(CarriageType type)
    {
        return prefabsMap[type];
    }


}
