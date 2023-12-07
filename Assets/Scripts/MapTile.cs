using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
 public class MapTile : MonoBehaviour
{
    PlacementManager placementManager;


    [SerializeField] SpriteRenderer hoverSprite, rangeSprite, selectSprite;
    [SerializeField] MeshRenderer hoverMesh, rangeMesh, selectMesh, invalidMesh;

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
        placedTower.GetComponentInChildren<MeshRenderer>().sortingOrder = -(int)transform.position.y;
    }    

    public void RemoveTower() 
    {
        Destroy(placedTower);
        placedTower = null; 
    }

    public void ReceiveTempTower(GameObject tower)
    {
        if (ableToHoldTower)
        {
            tempTower = Instantiate(tower, transform.position, Quaternion.identity);
            tempTower.GetComponent<TargetRange>().EnableTempTower();
        }
        else
        {
            EnableInvalidSprite();
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
    public void EnableRangeSprite() { rangeMesh.enabled = true; }
    public void DisableRangeSprite() { rangeMesh.enabled = false; }
    public void EnableSelectSprite() { selectMesh.enabled = true; }
    public void DisableSelectSprite() { selectMesh.enabled = false; }
    public void EnableInvalidSprite() { invalidMesh.enabled = true; }
    public void DisableInvalidSprite() { invalidMesh.enabled = false; }
    private void OnMouseEnter()
    {
        hoverMesh.enabled = true;
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
        hoverMesh.enabled = false;
        invalidMesh.enabled = false;
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
