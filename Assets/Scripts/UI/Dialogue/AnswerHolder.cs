using System;
using UnityEngine;
using UnityEngine.Localization.Components;

public class AnswerHolder : MonoBehaviour
{

    [SerializeField] private LocalizeStringEvent text;
    private Action clickAction;

    public void SetText(string text)
    {
        this.text.SetEntry(text);
    }
    public void OnClick()
    {
        clickAction.Invoke();
    }

    public void SetOnClickAction(Action action)
    {
        this.clickAction = action;
    }
}
