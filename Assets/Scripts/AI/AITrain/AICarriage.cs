using System.Collections;
using UnityEngine;



public abstract class AICarriage : ScriptableObject
{
    public TrainPart carriagePrefab;
    protected TrainPart instance;

    [SerializeField] private float importance;

    public virtual void Init()
    {
        
    }

    public TrainPart GetPrefab()
    {
        return carriagePrefab;
    }

    public void SetInstance(TrainPart instance)
    {
        this.instance = instance;
    }

    public TrainPart GetInstance()
    {
        return instance;
    }

    public virtual IEnumerator CoroutineAction()
    {
        yield return null;
    }

    public abstract AIActionType[] GetActionType();

    public float GetImportance()
    {
        return importance;
    }
}
