using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{

    [SerializeField] int value;
    [SerializeField] bool destroyOnPickup;
    [SerializeField] float timeToDespawn;
    float timer;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Vector2 collectPoint;

    bool coRoutineRunning = false;
    public MapTile tile;
    bool flashing = false;
    bool collected = false;
    private void Awake()
    {
        timer = timeToDespawn;
    }
    private void Start()
    {
        tile.coins.Add(this);

    }
    private void OnMouseDown()
    {
        if (!collected)
        {
            collected = true;
            StartCoroutine(ClickRoutine());
        }
    }

    IEnumerator ClickRoutine()
    {
        tile.coins.Remove(this);
        FMODController.PlaySFX("event:/SFX/Map/Coins/Coin_Click");
        flashing = false;
        meshRenderer.enabled = true;
        LeanTween.move(gameObject, collectPoint, .5f).setEaseInExpo();
        yield return new WaitForSeconds(.5f);
        FMODController.PlaySFX("event:/SFX/Map/Coins/Coin_Added");
        Collect();
    }

    public  void Collect()
    {
        Bank.AdjustCoins(value);
        if (destroyOnPickup) { Destroy(gameObject); }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 3f && !coRoutineRunning)
        {
            StartCoroutine(DespawnFlash());
        }

        if(timer < 0)
        {
            tile.coins.Remove(this);
            Destroy(gameObject);
        }
    }

    IEnumerator DespawnFlash()
    {
        coRoutineRunning = true;
        flashing = true;
        while(flashing)
        {
            var timeToWait = .1f * timer;
            if(timeToWait < .125f) { timeToWait = .125f; }
            if (timeToWait > .5f) { timeToWait = .5f; }
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(timeToWait);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
