using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewGameDecorations : MonoBehaviour
{
    [SerializeField] private Transform tunnel1;
    [SerializeField] private Transform tunnel2;
    [SerializeField] private SpriteRenderer tunnel2Renderer;
    [SerializeField] private List<Bird> initialBirds;
    [SerializeField] private List<Bird> incomingBirds;
    [SerializeField] private AudioSource trainSound;
    [SerializeField] private AudioSource birdSound;
    [SerializeField] private Transform trainTarget;
    [SerializeField] private float trainTime;
    [SerializeField] private Background background;
    [SerializeField] private float backgroundSpeed;
    [SerializeField] private List<Text> text;
    [SerializeField] private bool full = false;

    private Train train;

    private List<DecorAction> decorActions = new();
    private int nextAction = -1;
    private float startTime;

    public void Update()
    {
        if (nextAction < 0)
        {
            return;
        }
        while (nextAction < decorActions.Count && decorActions[nextAction].time < Time.time - startTime)
        {
            decorActions[nextAction].action.Invoke();
            nextAction++;
        }
    }

    public void SetTrain(Train train)
    {
        this.train = train;
    }

    public void StartNewGameSequence()
    {
        birdSound.gameObject.SetActive(true);
        SetTunnel();
        SetDecorActions();
    }

    private void SetDecorActions()
    {
        if (full)
        {
            SetupFullIntro();    
        } else
        {
            SetupShortIntro();
        }
        nextAction = 0;
        startTime = Time.time;
    }

    private void SetupFullIntro()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Camera.main.transform.position = new(0.0f, -2.5f * screenBounds.y, -10f);
        decorActions.Add(new(3f, () => RaiseCamera()));
        decorActions.Add(new(6f, () => BirdsComeIn()));
        decorActions.Add(new(7f, () => RevealText(0)));
        decorActions.Add(new(11f, () => RevealText(1)));
        decorActions.Add(new(15f, () => RevealText(2)));
        decorActions.Add(new(15f, () => StartTrainSound()));
        decorActions.Add(new(17f, () => StartLigthFromTunnel()));
        decorActions.Add(new(24f, () => StartTrain()));
        decorActions.Add(new(24.5f, () => BirdsFlyAway()));
        decorActions.Add(new(24.5f, () => HideAllText()));
        decorActions.Add(new(25.5f, () => StartPlot()));

    }

    private void SetupShortIntro()
    {
        decorActions.Add(new(0f, () => BirdsFlyAway()));
        decorActions.Add(new(0f, () => StartTrain()));
        decorActions.Add(new(1.5f, () => StartPlot()));
    }

    private void SetTunnel()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        tunnel1.gameObject.SetActive(true);
        tunnel1.position = new Vector3(-screenBounds.x, tunnel1.localPosition.y, tunnel1.localPosition.z);
        tunnel2.gameObject.SetActive(true);
        tunnel2.position = new Vector3(-screenBounds.x, tunnel2.localPosition.y, tunnel2.localPosition.z);
    }

    private class DecorAction
    {
        public Action action;
        public float time;
        public bool finished = false;

        public DecorAction(float time, Action action)
        {
            this.time = time;
            this.action = action;
        }
    }

    //Decor Actions

    private void RaiseCamera()
    {
        LeanTween.moveY(Camera.main.gameObject, -0.6f, 8f).setEaseOutExpo();
    }

    private void BirdsComeIn()
    {
        foreach(Bird bird in incomingBirds)
        {
            bird.SetFlyToPosition();
        }
    }

    private void BirdsFlyAway()
    {
        foreach(Bird bird in initialBirds)
        {
            bird.FlyAway();
        }
        foreach(Bird bird in incomingBirds)
        {
            bird.FlyAway();
        }
        birdSound.Stop();
    }

    private void StartTrainSound()
    {
        trainSound.volume = 0.0f;
        trainSound.gameObject.SetActive(true);
        LeanTween.value(0.0f, 1.0f, 10f).setOnUpdate((float val) => trainSound.volume = val);
    }

    private void StartLigthFromTunnel()
    {
        Color currentColor = tunnel2Renderer.color;
        currentColor.a = 0.0f;
        tunnel2Renderer.color = currentColor;
        LeanTween.alpha(tunnel2.gameObject, 1.0f, 10f);
    }

    private void StartTrain()
    {
        if (full) {
            SFXCollection.instance.TrainHorn();    
        }
        LeanTween.move(train.gameObject, trainTarget, trainTime).setOnComplete(() => StartMoving());
    }

    private void StartMoving()
    {
        background.SetSpeed(backgroundSpeed);
        train.FixPosition();
        LeanTween.moveX(tunnel1.gameObject, tunnel1.position.x - 10, 2).setOnComplete(() => Destroy(tunnel1.gameObject));
        LeanTween.moveX(tunnel2.gameObject, tunnel2.position.x - 10, 2).setOnComplete(() => Destroy(tunnel2.gameObject));
    }

    private void RevealText(int textNum)
    {
        if (textNum < 0 || textNum > text.Count)
        {
            return;
        }
        LeanTween.alphaText(text[textNum].rectTransform, 1.0f, 2f);
    }

    private void HideAllText()
    {
        foreach(Text txt in text)
        {
            LeanTween.alphaText(txt.rectTransform, 0.0f, 0.3f);
        }
    }

    private void StartPlot()
    {
        PlotEventManager.instance.NewGameStart();
    }
}
