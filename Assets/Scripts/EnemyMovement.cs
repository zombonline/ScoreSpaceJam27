using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    MapTile[] path;
    public int currentTile { get; private set; } = 0;

    bool move = false;

    [SerializeField] float speed;
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
        //transform.position = path[currentTile].transform.position;
        StartCoroutine(MovementRoutine());
    }

    IEnumerator MovementRoutine()
    {
        yield return new WaitForSeconds(.25f);
        move = true;
    }

    private void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentTile].transform.position, speed * Time.deltaTime); ;
        }
        if(transform.position == path[currentTile].transform.position)
        {
            move = false;
        }
    }

    public MapTile GetCurrentTile()
    {
        return path[currentTile];
    }

    public void SetPath(MapTile[] newPath) { path = newPath; }


    private void OnDestroy()
    {
        path[currentTile].RemoveEnemy(this);
    }

}
