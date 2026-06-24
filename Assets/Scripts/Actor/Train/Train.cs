using System;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private List<TrainPart> carriages;
    [SerializeField] private ReleasedPart releasedPartPrefab;
    [SerializeField] private bool playerControl;
    private TrainManipulator trainManipulator;
    private Locomotive locomotive;
    private List<TrainPart> parts = new List<TrainPart>();

    private float jumperThreshold = 5f;

    public void Awake()
    {
        trainManipulator = GetComponent<TrainManipulator>();
    }

    public void InitializeFromConfiguration(TrainConfiguration trainConfiguration) 
    {
        foreach (TrainPartConfiguration partConfig in trainConfiguration.GetConfigList())
        {
            TrainPart instance = InstantiatePart(CarriagePrefabHolder.instance.GetPrefab(partConfig.GetCarriageType()));
            instance.SetConfiguration(partConfig);
        }
        trainManipulator.Init();
    }

    public void FixPosition()
    {
        trainManipulator.FixPosition();
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

    public TrainPart GetTrainPartByIndex(int index) 
    {
        if (index < 0 || index >= parts.Count)
        {
            return null;
        }
        return parts[index];
    }

    public List<ArmorCart> GetShieldCarts()
    {
        return parts.FindAll(c => c.GetCarriageType() == CarriageType.SHIELD).ConvertAll(c => (ArmorCart)c.GetCarriagePayload());
    }

    public void SetSpeed(float speed)
    {
        locomotive.SetSpeed(speed);
        SetJumpersActive(speed > jumperThreshold);

    }

    public void SetMoving(bool moving)
    {
        locomotive.SetSteamActive(moving);
    }

    private void SetJumpersActive(bool active)
    {
        parts.ForEach(p => p.GetComponent<Jumper>()?.SetJumperActive(active));
    }
}
