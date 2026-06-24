using System;
using Newtonsoft.Json;

public class StationEvent
{
    [JsonProperty]
    string name;
    [JsonProperty]
    string prefabKey;

    public string GetName()
    {
        return name;
    }

    public string GetPrefabKey()
    {
        return prefabKey;
    }
}