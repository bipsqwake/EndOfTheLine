using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class QuestView : MonoBehaviour
{
    [SerializeField] private Image face;
    [SerializeField] private LocalizeStringEvent characterNameLoc;
    [SerializeField] private LocalizeStringEvent characterRequestLoc;

    public void SetName(string name)
    {
        characterNameLoc.SetEntry(name);
    }

    public void SetRequest(string request)
    {
        characterRequestLoc.SetEntry(request);
    }
}
