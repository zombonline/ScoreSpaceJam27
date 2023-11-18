using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
 public class MapTile : MonoBehaviour
{
    PlacementManager placementManager;


    [SerializeField] SpriteRenderer hoverSprite, rangeSprite, selectSprite;

    [Header("Tower")]
    [SerializeField] bool ableToHoldTower = false;
    public bool AbleToHoldTower { get { return ableToHoldTower; } private set { ableToHoldTower = value; }  }
    public GameObject placedTower { get; private set; }

    GameObject tempTower;
    public int placedTowerRefundValue { get; private set; }


    [Header("Enemy")]
    [SerializeField] bool ableToHoldEnemy = false;
    public bool AbleToHoldEnemy { get { return ableToHoldTower; } private set { ableToHoldTower = value; } }
    public List<EnemyMovement> enemies { get; private set; } = new List<EnemyMovement>();

    public List<Coin> coins;

    private void Awake()
    {
        this.name = "Map Tile (" + transform.position.x.ToString() + ", " + transform.position.y.ToString() + ")";

        placementManager = FindObjectOfType<PlacementManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(tempTower!= null)
            {
                Destroy(tempTower);
            }
        }
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

    public void SetRefundValue(int val)
    {
        this.placedTowerRefundValue = val;
    }

    public void ReceiveEnemy(EnemyMovement enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyMovement enemy)
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
        placementManager.SetLastSelectedTile(this);

    }


    #region Methods for enabling sprites
    public void EnableRangeSprite() { rangeSprite.enabled = true; }
    public void DisableRangeSprite() { rangeSprite.enabled = false; }
    public void EnableSelectSprite() { selectSprite.enabled = true; }
    public void DisableSelectSprite() { selectSprite.enabled = false; }
    private void OnMouseEnter()
    {
        hoverSprite.enabled = true;
        if(placedTower != null)
        {
            placedTower.GetComponent<TargetRange>().EnableTargetTileRangeSprites();
        }
        
        if(placementManager.tempTower != null)
        {
            ReceiveTempTower(placementManager.tempTower.towerPrefab);
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
