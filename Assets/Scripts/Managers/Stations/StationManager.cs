using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public static StationManager instance;

    [SerializeField] private TextAsset stationsFile;
    [SerializeField] private List<StationPrefabHolder> stationPrefabs;
    [SerializeField] private Transform stationInitPosition;
    [SerializeField] private Transform stationTargetPosition;
    [SerializeField] private float arriveTime;
    [SerializeField] private Background background;

    private Dictionary<string, StationEvent> stationEvents = new();

    private StationEvent currentStationEvent;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        stationEvents = JsonConvert.DeserializeObject<Dictionary<string, StationEvent>>(stationsFile.text);
    }

    public void StartStationEvent(string key)
    {
        if (!stationEvents.ContainsKey(key))
        {
            Debug.LogWarning("No station with id " + key);
            return;
        }
        currentStationEvent = stationEvents[key];
        GameObject instance = InstantiateStation(currentStationEvent.GetPrefabKey());
        LeanTween.value(1.0f, 0.0f, arriveTime).setOnUpdate(a => ArriveProcess(a, instance.transform)).setEaseOutQuad().setOnComplete(() => ArriveFinish());
    }

    private GameObject InstantiateStation(string key)
    {
        StationPrefabHolder prefabHolder = stationPrefabs.Find(ph => ph.key.Equals(key));
        if (prefabHolder == null)
        {
            throw new ArgumentException("Failed to get station prefab with key " + key);
        }
        return Instantiate(prefabHolder.station, stationInitPosition.position, Quaternion.identity, GlobalSettings.instance.gameRoot);
    }

    [Serializable]
    public class StationPrefabHolder
    {
        [SerializeField] public string key;
        [SerializeField] public GameObject station;
    }

    private void ArriveProcess(float pos, Transform station)
    {
        station.position = stationTargetPosition.position + (stationInitPosition.position - stationTargetPosition.position) * pos;
        background.SetSpeed(10 * pos);
        State.instance.GetPlayerTrain().SetSpeed(10 * pos);
    }

    private void ArriveFinish()
    {
        State.instance.GetPlayerTrain().SetMoving(false);
    }





}
