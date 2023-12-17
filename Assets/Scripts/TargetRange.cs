using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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

    [SerializeField] string shootSFX;
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
                else if(newPossibleTarget.currentTile/newPossibleTarget.path.Length > currentTarget.currentTile/currentTarget.path.Length) //if this enemy is further ahead than current target, assign it this one.
                {
                    currentTarget = newPossibleTarget;
                }
            }
        }
        return currentTarget;
    }
    public void UpdateSpineAsset()
    {
        StartCoroutine(UpdateSpineAssetRoutine());
    }
    public IEnumerator UpdateSpineAssetRoutine()
    {
        yield return new WaitForEndOfFrame();
        GetTarget();
        if (currentTarget != null)
        {
            Bone bone = skeletonAnimation.skeleton.FindBone("target");
            var localPositon = transform.InverseTransformPoint(currentTarget.GetCurrentTile().transform.position);
            bone.SetLocalPosition(localPositon);



            if (GetTarget().transform.position.y > transform.position.y) { skeletonAnimation.skeleton.SetSkin(backSkin); Debug.Log("facing back"); } //update skin
            else if (GetTarget().transform.position.y <= transform.position.y) { skeletonAnimation.skeleton.SetSkin(frontSkin); Debug.Log("facing front"); } //update skin
            skeletonAnimation.skeleton.SetSlotsToSetupPose();
            skeletonAnimation.LateUpdate();

            if (GetTarget().transform.position.x < transform.position.x) { spineAnimationState.SetAnimation(0, leftAnim, true); }
            else if (GetTarget().transform.position.x >= transform.position.x) { spineAnimationState.SetAnimation(0, rightAnim, true); }

        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(skeletonAnimation.skeleton.FindBone("target").GetWorldPosition(skeletonAnimation.transform), "Hello!");
    }

    public void CollectCoins()
    {
        if (tempTower) { return; } //check this tower is not a temp display tower.
        bool coinFound = false;
        foreach (MapTile targetTile in targetTiles)
        {
            if (coinFound) { break; }
            foreach (Coin coin in targetTile.coins)
            {
                if (!coinFound)
                {
                    Bone bone = skeletonAnimation.skeleton.FindBone("target");
                    var localPositon = transform.InverseTransformPoint(targetTile.transform.position);
                    bone.SetLocalPosition(localPositon);

                    if (targetTile.transform.position.y > transform.position.y) { skeletonAnimation.skeleton.SetSkin(backSkin); } //update skin
                    else if (targetTile.transform.position.y <= transform.position.y) { skeletonAnimation.skeleton.SetSkin(frontSkin); } //update skin
                    skeletonAnimation.skeleton.SetSlotsToSetupPose();
                    skeletonAnimation.LateUpdate();

                    if (targetTile.transform.position.x < transform.position.x) { spineAnimationState.SetAnimation(0, leftAnim, true); }
                    else if (targetTile.transform.position.x >= transform.position.x) { spineAnimationState.SetAnimation(0, rightAnim, true); }



                    spineAnimationState.SetAnimation(1, magnetAnim, false);

                }
                FMODController.PlaySFX(shootSFX);
                StartCoroutine(CollectCoinsRoutine(coin));
                targetTile.coins.Remove(coin);
                coinFound = true;
            }
        }
    }
    private IEnumerator CollectCoinsRoutine(Coin coin)
    {
        yield return new WaitForEndOfFrameUnit();
        LeanTween.move(coin.gameObject, (Vector2)transform.position, .5f).setEaseInOutElastic();
        yield return new WaitForSeconds(0.5f);
        coin.Collect();
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
        yield return new WaitForEndOfFrameUnit();
        if (GetTarget() != null)
        {
            FMODController.PlaySFX(shootSFX);
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
