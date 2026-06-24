using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class Condition
{
    [JsonProperty]
    private HashSet<string> allOf;
    [JsonProperty]
    private HashSet<string> anyOf;
    [JsonProperty]
    private HashSet<string> notAny;
    [JsonProperty]
    private bool fallback;
    [JsonProperty]
    private string nextEvent;

    public string GetNextEvent()
    {
        return nextEvent;
    }

    public bool Resolve(HashSet<string> markers)
    {
        if (fallback)
        {
            return true;
        }
        bool allOfResolve = (allOf == null) || (allOf.Count == 0) || allOf.IsSubsetOf(markers);
        bool anyOfResolve = (anyOf == null) || (anyOf.Count == 0) || anyOf.Overlaps(markers);
        bool notAnyResolve = (notAny == null) || (notAny.Count == 0) || !notAny.Overlaps(markers);
        return allOfResolve && anyOfResolve && notAnyResolve;
    }
}