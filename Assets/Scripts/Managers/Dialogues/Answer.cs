using System.Collections.Generic;
using Newtonsoft.Json;

public class Answer
{
    [JsonProperty]
    private string nextPhrase;
    [JsonProperty]
    private string text;

    [JsonProperty]
    private HashSet<string> addMarker;
    [JsonProperty]
    private HashSet<string> removeMarker;

    public string GetNextPhrase()
    {
        return nextPhrase;
    }

    public HashSet<string> GetAddMarker()
    {
        return addMarker;
    }

    public HashSet<string> GetRemoveMarker()
    {
        return removeMarker;
    }

    public string GetText()
    {
        return text;
    }
}