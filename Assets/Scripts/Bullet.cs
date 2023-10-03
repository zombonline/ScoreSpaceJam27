using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public List<MapTile> tiles = new List<MapTile>();
    [SerializeField] List<string> possibleTargets;
    [SerializeField] int damagePoints;
    public void Awake()
    {
        StartCoroutine(DamageRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MapTile>() != null)
        {
            if(tiles.Contains(collision.GetComponent<MapTile>())) { return; }
            tiles.Add(collision.GetComponent<MapTile>());
        }
    }

    private IEnumerator DamageRoutine()
    {
        yield return new WaitUntil(() => tiles.Count > 0);
        foreach(var tile in tiles)
        {
            if (tile.enemies.Count <= 0) { continue; }
            Debug.Log(tile.enemies.Count);
            foreach (GameObject enemy in tile.enemies)
            {
                if (!possibleTargets.Contains(enemy.tag)) { break; }

                enemy.GetComponent<Health>().AdjustHitPoints(-damagePoints);
            }

        }
        //Destroy(gameObject);
    }

}
