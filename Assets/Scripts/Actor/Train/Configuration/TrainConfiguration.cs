using System.Collections.Generic;
using Newtonsoft.Json;

public class TrainConfiguration
{
    [JsonProperty]
    private List<TrainPartConfiguration> configList = new();

    public List<TrainPartConfiguration> GetConfigList()
    {
        return configList;
    }

    public void AddConfig(TrainPartConfiguration configuration)
    {
        configList.Add(configuration);
    }
}