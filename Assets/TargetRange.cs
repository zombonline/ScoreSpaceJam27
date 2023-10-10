using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TargetRange : MonoBehaviour
{
    List<MapTile> mapTilesInsideTrigger= new List<MapTile>();

    public List<MapTile> targetTiles { get; private set; } = new List<MapTile>();
    bool tempTower = false;

    [SerializeField] string[] possibleTargets;

    [SerializeField] Bullet bulletPrefab;

    EnemyMovement currentTarget = null;

    public void EnableTempTower() { tempTower = true; }

    private void Start()
    {
        AssignTargetTiles();
    }
    public void AssignTargetTiles()
    {
        if (tempTower) { return; } //Do not target if tower is temporary
        targetTiles.Clear();
        targetTiles = mapTilesInsideTrigger;
    }

    public void EnableTargetTileRangeSprites()
    {
        foreach(MapTile tile in targetTiles)
        {
            tile.EnableRangeSprite();
        }
    }

    public void DisableTargetTileRangeSprites()
    {
        foreach(MapTile tile in targetTiles)
        {
            tile.DisableRangeSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MapTile>() != null)
        {
            mapTilesInsideTrigger.Add(collision.GetComponent<MapTile>());
            if(tempTower) { collision.GetComponent<MapTile>().EnableRangeSprite(); }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<MapTile>() != null)
        {
            mapTilesInsideTrigger.Remove(collision.GetComponent<MapTile>());
            if (tempTower) { collision.GetComponent<MapTile>().DisableRangeSprite(); }

        }
    }

    public EnemyMovement GetTarget()
    {

        foreach (MapTile tile in targetTiles)
        {
            if (tile.enemies.Count <= 0) { continue;  } //come out of loop if current tile has no enemies
            
            foreach(EnemyMovement newPossibleTarget in tile.enemies)
            {

                if (!possibleTargets.Contains(newPossibleTarget.tag)) { break; } //come out of loop if enemy is not a possible target

                if (currentTarget == null) { currentTarget = newPossibleTarget; } //if no current target assigned, assign this one
                else if(newPossibleTarget.currentTile > currentTarget.currentTile) //if this enemy is further ahead than current target, assign it this one.
                {
                    currentTarget = newPossibleTarget;
                }
            }
        }
        return currentTarget;
    }

    public void Shoot()
    {
        if (tempTower) { return; }
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        yield return new WaitForEndOfFrameUnit();
        if (GetTarget() != null)
        {
            MapTile targetTile = GetTarget().GetCurrentTile();
            if (targetTile != null)
            {
                Bullet newBullet = Instantiate(bulletPrefab, targetTile.transform.position, Quaternion.identity).GetComponent<Bullet>();
            }
        }
    }    



}
