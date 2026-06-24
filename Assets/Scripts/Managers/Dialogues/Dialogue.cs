using System.Collections.Generic;
using Newtonsoft.Json;

public class Dialogue
{
    [JsonProperty]
    private Dictionary<string, Phrase> phrases;
    [JsonProperty]
    private string initPhrase;

    public string GetInitPhrase()
    {
        return initPhrase;
    }

    public Dictionary<string, Phrase> GetPhrases()
    {
        return phrases;
    } 
}