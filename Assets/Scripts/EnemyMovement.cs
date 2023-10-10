using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    MapTile[] path;
    public int currentTile { get; private set; } = 0;
    public void MoveAlongPath()
    {
        currentTile++;
        if(currentTile >= path.Length)
        {
            FindObjectOfType<Base>().AdjustHitPoints(-1);
            Destroy(gameObject);
            return;
        } 
        path[currentTile-1].RemoveEnemy(this);
        path[currentTile].ReceiveEnemy(this);
        transform.position = path[currentTile].transform.position;

    }

    public MapTile GetCurrentTile()
    {
        return path[currentTile];
    }

    public void SetPath(MapTile[] newPath) { path = newPath; }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MoveAlongPath();
        }
    }


    private void OnDestroy()
    {
        path[currentTile].RemoveEnemy(this);
    }

}
