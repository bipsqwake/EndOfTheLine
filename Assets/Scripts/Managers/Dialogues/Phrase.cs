using System.Collections.Generic;
using Newtonsoft.Json;

public class Phrase
{
    [JsonProperty]
    private string speaker;
    [JsonProperty]
    private string nextPhrase;
    [JsonProperty]
    private Dictionary<string, Answer> answers;
    [JsonProperty]
    private bool final;
    [JsonProperty]
    private string text;

    public string GetSpeaker()
    {
        return speaker;
    }

    public string GetNextPhrase()
    {
        return nextPhrase;
    }

    public Dictionary<string, Answer> GetAnswers()
    {
        return answers;
    }

    public bool IsFinal()
    {
        return final;
    }

    public string GetText()
    {
        return text;
    }
}