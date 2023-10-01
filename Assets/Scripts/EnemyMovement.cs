using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    MapTile[] path;
    int currentTile = 0;
    public void MoveAlongPath()
    {
        currentTile++;
        if(currentTile >= path.Length)
        {
            FindObjectOfType<Base>().AdjustHitPoints(-1);
            Destroy(gameObject);
            return;
        } 
        path[currentTile-1].RemoveEnemy(this.gameObject);
        path[currentTile].ReceiveEnemy(this.gameObject);
        transform.position = path[currentTile].transform.position;

    }

    public void SetPath(MapTile[] newPath) { path = newPath; }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MoveAlongPath();
        }
    }

}
