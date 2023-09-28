using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
 public class MapTile : MonoBehaviour
{
    PlacementManager placementManager;


    [SerializeField] SpriteRenderer hoverSprite, rangeSprite;

    [Header("Tower")]
    [SerializeField] bool ableToHoldTower = false;
    public bool AbleToHoldTower { get { return ableToHoldTower; } private set { ableToHoldTower = value; }  }
    public GameObject placedTower { get; private set; }

    GameObject tempTower;

    [Header("Enemy")]
    [SerializeField] bool ableToHoldEnemy = false;
    public bool AbleToHoldEnemy { get { return ableToHoldTower; } private set { ableToHoldTower = value; } }
    public List<GameObject> enemies { get; private set; }

    private void Awake()
    {
        this.name = "Map Tile (" + transform.position.x.ToString() + ", " + transform.position.y.ToString() + ")";

        placementManager = FindObjectOfType<PlacementManager>();
    }

    public void ReceiveTower(GameObject tower)
    {
        placedTower = Instantiate(tower, transform.position, Quaternion.identity);
    }    

    public void RemoveTower() 
    {
        Destroy(placedTower);
        placedTower = null; 
    }

    public void ReceiveTempTower(GameObject tower)
    {
        tempTower = Instantiate(tower, transform.position, Quaternion.identity);
        if (ableToHoldTower)
        {
            tempTower.GetComponent<TargetRange>().EnableTempTower();
        }
    }

    public void RemoveTempTower()
    {
        tempTower.GetComponent<TargetRange>().DisableTargetTileRangeSprites();
        Destroy(tempTower);
        tempTower = null;
    }

    public void ReceiveEnemy(GameObject enemy) { enemies.Add(enemy); }

    public void RemoveEnemy(GameObject enemy)
    {
        if(enemies.Contains(enemy))
        {
            enemies.Remove(enemy);  
        }
    }

    public void OnMouseDown()
    {
        if(ableToHoldTower)
        {
            placementManager.Place(this);
        }
    }


    #region Methods for enabling sprites
    public void EnableRangeSprite() { rangeSprite.enabled = true; }

    public void DisableRangeSprite() { rangeSprite.enabled = false; }

    private void OnMouseEnter()
    {
        hoverSprite.enabled = true;
        if(placedTower != null)
        {
            placedTower.GetComponent<TargetRange>().EnableTargetTileRangeSprites();
        }
        
        if(placementManager.tempTower != null)
        {
            ReceiveTempTower(placementManager.tempTower);
        }
    }

    private void OnMouseExit() 
    {
        hoverSprite.enabled = false;
        if (placedTower != null)
        {
            placedTower.GetComponent<TargetRange>().DisableTargetTileRangeSprites();
        }
        if (tempTower != null)
        {
            RemoveTempTower();
        }

    }
    #endregion

}
