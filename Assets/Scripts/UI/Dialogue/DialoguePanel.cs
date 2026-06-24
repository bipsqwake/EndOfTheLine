using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public static DialoguePanel instance;
    [SerializeField] TextAsset dialoguesFile;
    [SerializeField] private DialogueView textView;
    [SerializeField] private DialogueView requestView;

    private Dictionary<string, Dialogue> dialogues = new();

    private Dialogue currentDialogue;
    private Phrase currentPhrase;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        Init();
    }

    void Start()
    {
        // requestView.gameObject.SetActive(false);
        // textView.gameObject.SetActive(false);
    }

    private void Init()
    {
        dialogues = JsonConvert.DeserializeObject<Dictionary<string, Dialogue>>(dialoguesFile.text);
    }

    public void StartDialogue(string id)
    {
        if (!dialogues.ContainsKey(id))
        {
            Debug.LogWarning("No dialogue with id " + id);
            return;
        }
        currentDialogue = dialogues[id];
        ShowPhrase(currentDialogue.GetInitPhrase());
    }

    private void ShowPhrase(string id)
    {
        Phrase phrase = currentDialogue.GetPhrases()[id];
        if (phrase == null)
        {
            Debug.LogWarning("Need to create error handler here");
            return;
        }
        currentPhrase = phrase;
        if (phrase.GetAnswers() == null || phrase.GetAnswers().Count == 0)
        {
            ShowText();
        } else
        {
            ShowRequest();
        }
    }

    private void ShowText()
    {
        requestView.gameObject.SetActive(false);
        textView.SetSpeaker(currentPhrase.GetSpeaker());
        textView.gameObject.SetActive(true);
        textView.SetText(currentPhrase.GetText());
        
        textView.StartTypewriter(() => textView.SetTriangle(true));
    }

    private void ShowRequest()
    {
        textView.gameObject.SetActive(false);
        requestView.SetSpeaker(currentPhrase.GetSpeaker());
        requestView.gameObject.SetActive(true);
        requestView.SetText(currentPhrase.GetText() );
        // requestView.SetAnswers(currentPhrase.GetAnswers(), this);
        requestView.StartTypewriter(() => requestView.SetAnswers(currentPhrase.GetAnswers(), this));
    }

    private void HideAll()
    {
        requestView.gameObject.SetActive(false);
        textView.gameObject.SetActive(false);
    }

    public void NextPhrase()
    {
        if (currentPhrase.IsFinal())
        {
            HideAll();
            PlotEventManager.instance.NextEvent();
        } else
        {
            ShowPhrase(currentPhrase.GetNextPhrase());
        }
    }

    public void NextPhrase(string response)
    {
        Answer answer = currentPhrase.GetAnswers()[response];
        if (answer == null)
        {
            Debug.LogWarning("No answer with id " + response);
        }
        PlotEventManager.instance.AddMarkers(answer.GetAddMarker());
        PlotEventManager.instance.RemoveMarkers(answer.GetRemoveMarker());
        ShowPhrase(answer.GetNextPhrase());
    }

}
