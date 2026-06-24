using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private TextAsset enemiesFile;
    [SerializeField] private TextMeshProUGUI enemyIntroText;
    [SerializeField] private CanvasGroup enemyIntroGroup;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private Transform gameRoot;
    [SerializeField] private TrainAI enemyTrainPrefab;
    [SerializeField] private float entryTime;

    private Dictionary<string, EnemyInfo> enemies = new();

    private int nextAction = -1;
    private float startTime;
    private List<EnemyAppearanceAction> enemyAppearanceActions = new();

    private EnemyInfo currentEnemy;

    private TrainAI currentEnemyTrain;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Init();
    }

    public void Update()
    {
        if (nextAction < 0)
        {
            return;
        }
        while (nextAction < enemyAppearanceActions.Count && enemyAppearanceActions[nextAction].time < Time.time - startTime)
        {
            enemyAppearanceActions[nextAction].action.Invoke();
            nextAction++;
        }
        if (nextAction >= enemyAppearanceActions.Count)
        {
            nextAction = -1;
        }
    }

    private void Init()
    {
        AITrainConfiguration config = new ();
        AILocomotiveConfiguration locomotiveConfiguration = new();
        AIPassangerCartConfiguration passangerCartConfiguration = new();
        config.Add(locomotiveConfiguration);
        config.Add(passangerCartConfiguration);
        Debug.Log(JsonConvert.SerializeObject(config, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
        enemies = JsonConvert.DeserializeObject<Dictionary<string, EnemyInfo>>(enemiesFile.text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }

    public void StartEnemy(string enemyId)
    {
        if (!enemies.ContainsKey(enemyId))
        {
            Debug.LogWarning("No enemy with id " + enemyId);
        }
        currentEnemy = enemies[enemyId];
        PrepareActions();
        nextAction = 0;
        startTime = Time.time;
    }

    private void PrepareActions()
    {
        enemyAppearanceActions = new()
        {
            new(1.0f, () => ShowTitle()),
            new(2.0f, () => StartEnemy()),
            new(4.0f, () => FadeTitle())
        };

    }

    private class EnemyAppearanceAction
    {
        public Action action;
        public float time;

        public EnemyAppearanceAction(float time, Action action)
        {
            this.action = action;
            this.time = time;
        }
    }

    private void ShowTitle()
    {
        enemyIntroGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(enemyIntroGroup, 1.0f, 1.0f);
        enemyIntroText.text = currentEnemy.GetName();
    }

    private void StartEnemy()
    {
        currentEnemyTrain = Instantiate(enemyTrainPrefab, GlobalSettings.instance.enemyOutPosition.position, Quaternion.identity, gameRoot);
        currentEnemyTrain.Initialize(currentEnemy.GetTrainConfiguration());
        LeanTween.move(currentEnemyTrain.gameObject, enemyPosition, entryTime).setEaseInOutQuint().setOnComplete(() => StartBattle());
    }

    private void FadeTitle()
    {
        LeanTween.alphaCanvas(enemyIntroGroup, 0.0f, 3.0f);
    }

    private void StartBattle()
    {
        State.instance.SetPlayerControl(true);
        currentEnemyTrain.FixPosition();
    }

    public void EndBattle()
    {
        Destroy(currentEnemyTrain.gameObject);
        PlotEventManager.instance.NextEvent();
    }

}
