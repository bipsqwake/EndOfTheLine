using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private List<TrainPart> carriages;
    [SerializeField] private ReleasedPart releasedPartPrefab;
    private Locomotive locomotive;
    private Stack<TrainPart> parts = new Stack<TrainPart>();

    public void Awake()
    {
        float position = 0;
        foreach (var carriage in carriages)
        {
            TrainPart instantiated = Instantiate(carriage, transform, false);
            float halfLength = instantiated.GetCartView().size.x / 2;
            position += halfLength;
            instantiated.transform.localPosition = Vector3.left * position;
            position += halfLength;
            if (locomotive == null && instantiated.GetComponent<Locomotive>() != null)
            {
                locomotive = instantiated.GetComponent<Locomotive>();
            }
            instantiated.train = this;
            parts.Push(instantiated);
        }
    }

    public Locomotive GetLocomotive()
    {
        return locomotive;
    }

    public void ReleaseCarriage(TrainPart releasePart)
    {
        ReleasedPart releasedPart = Instantiate(releasedPartPrefab, transform);
        while (parts.Count > 0)
        {
            TrainPart next = parts.Pop();
            next.transform.SetParent(releasedPart.transform);
            if (next == releasePart)
            {
                break;
            }
        }
    }
}
