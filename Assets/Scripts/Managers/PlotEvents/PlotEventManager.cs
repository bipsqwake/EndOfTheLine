using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class PlotEventManager : MonoBehaviour
{
    private static string NEW_GAME_EVENT = "startGameDialogue";
    public static PlotEventManager instance;
    [SerializeField] private TextAsset eventsFile;
    private Dictionary<string, PlotEvent> plotEvents = new();

    private HashSet<string> markers = new();

    private PlotEvent currentEvent;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        Init();
    }

    private void Init()
    {
        plotEvents = JsonConvert.DeserializeObject<Dictionary<string, PlotEvent>>(eventsFile.text);
    }

    public void NewGameStart()
    {
        ResolveEvent(NEW_GAME_EVENT);
    }

    private void ResolveEvent(string eventId)
    {
        if (!plotEvents.ContainsKey(eventId))
        {
            Debug.LogWarning("No event with id " + eventId);
            return;
        }   
        currentEvent = plotEvents[eventId];
        if (currentEvent.GetAction() == PlotEvent.Action.DIALOGUE)
        {
            ResolveDialogueEvent(currentEvent);
        } else if (currentEvent.GetAction() == PlotEvent.Action.ENEMY)
        {
            ResolveEnemyEvent(currentEvent);
        } else if (currentEvent.GetAction() == PlotEvent.Action.STATION) {
            ResolveStationEvent(currentEvent);
        }
    }

    private void ResolveDialogueEvent(PlotEvent plotEvent)
    {
        DialoguePanel.instance.StartDialogue(plotEvent.GetValue());
    }

    private void ResolveEnemyEvent(PlotEvent plotEvent)
    {
        EnemyManager.instance.StartEnemy(plotEvent.GetValue());
    }

    private void ResolveStationEvent(PlotEvent plotEvent)
    {
        StationManager.instance.StartStationEvent(plotEvent.GetValue());
    } 

    public void NextEvent()
    {
        foreach (Condition cond in currentEvent.GetConditions())
        {
            if (cond.Resolve(markers))
            {
                ResolveEvent(cond.GetNextEvent());
                return;
            }
        }
    }

    public void AddMarkers(HashSet<string> markers)
    {
        if (markers == null)
        {
            return;
        }
        this.markers.AddRange(markers);
    }

    public void RemoveMarkers(HashSet<string> markers)
    {
        if (markers == null)
        {
            return;
        }
        foreach(string marker in markers)
        {
            this.markers.Remove(marker);
        }
    } 
}
