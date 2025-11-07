using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageView : MonoBehaviour
{
    public enum DamageAction
    {
        SMOKE,
        EXPLOTION_SERIES,
        SMOKE_OFF
    }

    [SerializeField] private GameObject[] smoke;
    [SerializeField] private Transform[] explotionSeries;

    private int smokeIterator = 0;
    private int explotionSeriesIterator = 0;
    private float explotionSeriesTiming = 0.3f;

    [SerializeField] private List<DamageActionPair> damageActions;

    public void Activate(float previousDamage, float currentDamage)
    {
        foreach (var actionPair in damageActions)
        {
            if (actionPair.health <= previousDamage && actionPair.health >= currentDamage)
            {
                PerformAction(actionPair.action);
            }
        }
    }

    private void PerformAction(DamageAction action)
    {
        switch (action)
        {
            case DamageAction.SMOKE:
                Smoke();
                break;
            case DamageAction.EXPLOTION_SERIES:
                ExplotionSeries();
                break;
            case DamageAction.SMOKE_OFF:
                SmokeOff();
                break;
        }
    }

    private void Smoke()
    {
        if (smokeIterator >= smoke.Length)
        {
            return;
        }
        smoke[smokeIterator++].SetActive(true);
    }

    private void ExplotionSeries()
    {
        if (explotionSeriesIterator >= explotionSeries.Length)
        {
            return;
        }
        Queue<GameObject> explotions = new Queue<GameObject>();
        foreach (Transform explotion in explotionSeries[explotionSeriesIterator]) {
            explotions.Enqueue(explotion.gameObject);
        }
        explotionSeriesIterator++;
        StartCoroutine(NextExplotion(explotions));
    }

    private IEnumerator NextExplotion(Queue<GameObject> explotions)
    {
        GameObject next;
        while (explotions.TryDequeue(out next))
        {
            next.SetActive(true);
            SFXCollection.instance.Explotion();
            yield return new WaitForSeconds(explotionSeriesTiming);
        }
        yield return null;
    }

    private void SmokeOff()
    {
        foreach (var s in smoke)
        {
            s.SetActive(false);
        }
    }
    
    [Serializable]
    public class DamageActionPair
    {
        public float health;
        public DamageAction action;
    }

}
