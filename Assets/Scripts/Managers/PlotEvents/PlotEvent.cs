using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlotEvent
{
    [JsonProperty]
    private Action action;
    [JsonProperty]
    private string value;
    [JsonProperty]
    private List<string> addMarker;
    [JsonProperty]
    private List<string> removeMarker;
    [JsonProperty]
    private List<Condition> conditions;

    public Action GetAction()
    {
        return action;
    }

    public string GetValue()
    {
        return value;
    }

    public List<string> GetAddMerker()
    {
        return addMarker;
    }

    public List<string> GetRemoveMarker()
    {
        return removeMarker;
    }

    public List<Condition> GetConditions()
    {
        return conditions;
    }
    

    public enum Action
    {
        DIALOGUE,
        ENEMY,
        STATION,
        CONDITION
    }
}
