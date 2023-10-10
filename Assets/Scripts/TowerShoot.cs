using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    TargetRange range;

    [SerializeField] GameObject bulletPrefab;


    public void Shoot()
    {
        MapTile targetTile = range.GetTarget().GetComponent<EnemyMovement>().GetCurrentTile();
        Instantiate(bulletPrefab, targetTile.transform.position, Quaternion.identity).GetComponent<Bullet>();

    }

}
