using Spine;
using Spine.Unity;
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

    AmmoCache ammoMenu;

    public void EnableTempTower() { tempTower = true; }

    [SerializeField] SkeletonAnimation skeletonAnimation;
    Spine.AnimationState spineAnimationState;
    [SerializeField] string leftAnim, rightAnim, shootAnim, frontSkin, backSkin, magnetAnim;
    private void Start()
    {
        spineAnimationState = skeletonAnimation.AnimationState;
        foreach(AmmoCache menu in FindObjectsOfType<AmmoCache>())
        {
            if(menu.ammo == bulletPrefab.ammoType)
            {
                ammoMenu = menu;
            }
        }

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
        if (currentTarget != null && !targetTiles.Contains(currentTarget.GetCurrentTile())) // if current target no longer in range
        {
            currentTarget = null;
        }
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
    public Coin GetCoinTarget()
    {
        Coin coin = null;
        bool coinFound = false;
        foreach (MapTile targetTile in targetTiles)
        {
            foreach (Coin possible in targetTile.coins)
            {
                if (coinFound) { break; }
                targetTile.coins.Remove(coin);
                coinFound = true;
            }
        }
        return coin;
    }
    public void UpdateSpineAsset()
    {
        StartCoroutine(UpdateSpineAssetRoutine());
    }
    public IEnumerator UpdateSpineAssetRoutine()
    {
        yield return new WaitForEndOfFrameUnit();
        if (GetTarget() != null)
        {
            if (GetTarget().transform.position.y > transform.position.y) { skeletonAnimation.skeleton.SetSkin(backSkin); Debug.Log("facing back"); } //update skin
            else if(GetTarget().transform.position.y < transform.position.y) { skeletonAnimation.skeleton.SetSkin(frontSkin); Debug.Log("facing front"); } //update skin
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();

            if (GetTarget().transform.position.x < transform.position.x) { spineAnimationState.SetAnimation(0, leftAnim, true); }
            else if (GetTarget().transform.position.x > transform.position.x) { spineAnimationState.SetAnimation(0, rightAnim, true); }


            Bone bone = skeletonAnimation.skeleton.FindBone("target");
            bone.SetLocalPosition(skeletonAnimation.transform.InverseTransformDirection(currentTarget.transform.position)); 
        }
    }

    public void CollectCoins()
    {
        if (tempTower) { return; } //check this tower is not a temp display tower.
        StartCoroutine(CollectCoinsRoutine());
    }
    private IEnumerator CollectCoinsRoutine()
    {
        yield return new WaitForEndOfFrameUnit();
        LeanTweenController.MoveObject(GetCoinTarget().gameObject, transform.position);
        yield return new WaitForSeconds(0.5f);
        GetCoinTarget().Collect();
    }
    public void Shoot()
    {
        if (tempTower) { return; } //check this tower is not a temp display tower.
        if(ammoMenu.GetAmmoCount() <= 0) { return; } //check player has enough of correct ammo
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
                spineAnimationState.SetAnimation(1, shootAnim, false);
                ammoMenu.UseAmmo();
                Bullet newBullet = Instantiate(bulletPrefab, targetTile.transform.position, Quaternion.identity).GetComponent<Bullet>();
            }
        }
    }    



}
