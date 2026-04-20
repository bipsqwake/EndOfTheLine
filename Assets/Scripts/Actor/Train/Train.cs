using System;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private List<TrainPart> carriages;
    [SerializeField] private ReleasedPart releasedPartPrefab;
    [SerializeField] private bool playerControl;
    private Locomotive locomotive;
    private List<TrainPart> parts = new List<TrainPart>();

    public void Awake()
    {
        if (playerControl)
        {
            InstantiateParts(carriages);
        }
    }

    public void InstantiateParts(List<TrainPart> prefabs)
    {
        foreach (var carriage in carriages)
        {
            InstantiatePart(carriage);
        }
    }

    public TrainPart InstantiatePart(TrainPart prefab)
    {
        float position = parts.Count == 0 ? 0 : Math.Abs(parts[parts.Count - 1].transform.localPosition.x) + Math.Abs(parts[parts.Count - 1].GetCartView().size.x / 2);
        TrainPart instantiated = Instantiate(prefab, transform, false);

        float halfLength = instantiated.GetCartView().size.x / 2;
        position += halfLength;
        instantiated.transform.localPosition = Vector3.left * position;

        if (locomotive == null && instantiated.GetComponent<Locomotive>() != null)
        {
            locomotive = instantiated.GetComponent<Locomotive>();
            locomotive.SetPlayerControl(playerControl);
        }
        instantiated.train = this;
        instantiated.SetPlayerControl(playerControl);

        parts.Add(instantiated);
        return instantiated;
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
            TrainPart next = parts[parts.Count - 1];
            parts.Remove(next);
            next.transform.SetParent(releasedPart.transform);
            if (next == releasePart)
            {
                break;
            }
        }
    }

    public List<TrainPart> GetParts()
    {
        return parts;
    }

    public int GetPartPositionByX(float x)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            TrainPart trainPart = parts[i];
            float partWidth = Math.Abs(trainPart.GetCartView().size.x / 2) * transform.localScale.x;
            if (HitsPart(x, trainPart.transform.position.x, partWidth))
            {
                return i;
            }
        }
        return -1;
    }

    public void DebugDrawBordersWithShift(float shift)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            TrainPart trainPart = parts[i];
            float partWidth = Math.Abs(trainPart.GetCartView().size.x / 2) * transform.localScale.x;
            DebugGizmos.AddLine(new Vector3(trainPart.transform.position.x + partWidth + shift, GetHeigthPoint(), 0.0f));
            DebugGizmos.AddLine(new Vector3(trainPart.transform.position.x - partWidth + shift, GetHeigthPoint(), 0.0f));
        }
    }

    private bool HitsPart(float x, float partPos, float partWidth)
    {
        return x > partPos - partWidth && x < partPos + partWidth;
    }

    public float GetHeigthPoint()
    {
        float max = -Mathf.Infinity;
        foreach (TrainPart part in parts)
        {
            max = Mathf.Max(part.transform.position.y + part.GetCartView().size.y, max);
        }
        return max - 0.1f;
    }
}
