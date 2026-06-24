using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent speaker;
    [SerializeField] private LocalizeStringEvent text;
    [SerializeField] private TextMeshProUGUI textTmp;
    [SerializeField] private Blinking triangle;
    [SerializeField] private Transform answersHolder;
    [SerializeField] private AnswerHolder answerPrefab;
    [SerializeField] private float tickTime;
    [SerializeField] private int charsInTick;

    public void SetSpeaker(string speaker)
    {
        this.speaker.SetEntry(speaker);
    }

    public void SetText(string text)
    {
        this.text.SetEntry(text);
        StartCoroutine(Typewriter(null));
    }

    public void SetTriangle(bool on)
    {
        if (triangle != null)
        {
            triangle.SetOn(on);   
        }
    }

    public void StartTypewriter(Action callback)
    {
        StartCoroutine(Typewriter(callback));
    }

    public void SetText(string text, Dictionary<string, Answer> answers, DialoguePanel rootPanel)
    {
        foreach (Transform child in answersHolder)
        {
            Destroy(child.gameObject);
        }
        this.text.SetEntry(text);
        StartCoroutine(Typewriter(() => SetAnswers(answers, rootPanel)));

    }
    public void SetAnswers(Dictionary<string, Answer> answers, DialoguePanel rootPanel)
    {
        foreach (string key in answers.Keys)
        {
            AnswerHolder holder = Instantiate(answerPrefab, answersHolder);
            holder.SetOnClickAction(() => rootPanel.NextPhrase(key));
            holder.SetText(answers[key].GetText());
        }
    }

    private IEnumerator Typewriter(Action callback)
    {
        WaitForSeconds wfs = new WaitForSeconds(tickTime);
        textTmp.maxVisibleCharacters = 0;
        // textTmp.ForceMeshUpdate();
        while (textTmp.maxVisibleCharacters < 117)
        {
            textTmp.maxVisibleCharacters += charsInTick;
            yield return wfs;
        }
        callback?.Invoke();
    }
}
