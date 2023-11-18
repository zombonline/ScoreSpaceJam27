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
    [SerializeField] SpriteRenderer spriteRenderer;

    bool coRoutineRunning = false;
    public MapTile tile;
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
        Collect();
    }

    public  void Collect()
    {
        FindObjectOfType<Bank>().AdjustCoins(value);
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
        var flashing = true;
        while(flashing)
        {
            var timeToWait = .1f * timer;
            if(timeToWait < .125f) { timeToWait = .125f; }
            if (timeToWait > .5f) { timeToWait = .5f; }
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, 0);
            yield return new WaitForSeconds(timeToWait);
            spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
