using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameMode
{
    Battle,
    Build
}
[System.Serializable]
public struct Paths
{
    public MapTile[] path; 
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] SOWave[] waves;

    int currentWave = -1, currentWaveStep;

    public GameMode gameMode { get; private set; } = GameMode.Build;

    [SerializeField] Paths[] pathsMouse, pathsBird, pathsDog;

    [SerializeField] GameObject mousePrefab, birdPrefab, dogPrefab;

    [SerializeField] UnityEvent onWaveEnd, onWaveStart;

    private void Awake()
    {
        LoadNewWave();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameMode == GameMode.Build)
        {
            gameMode = GameMode.Battle;
            
            onWaveStart.Invoke();
        }
    }


    //do every beat
    public void SpawnWaveStep()
    {
        if(currentWaveStep >= waves[currentWave].waveSteps.Length) { return; } //No more steps left in wave 
        if(gameMode== GameMode.Build) { return; }
        var waveStep = waves[currentWave].waveSteps[currentWaveStep];
        SpawnEnemy(waveStep.mice, pathsMouse, mousePrefab);
        SpawnEnemy(waveStep.birds, pathsBird, birdPrefab);
        SpawnEnemy(waveStep.dogs, pathsDog, dogPrefab);

        currentWaveStep++;
    }
    //do every beat
    public void CheckForWaveEnd()
    {
        if (currentWaveStep < waves[currentWave].waveSteps.Length) { return; } //Wave still has unactioned steps. Wave not over.
        if (FindObjectsOfType<EnemyMovement>().Length > 0) { return; } //Wave still has active enemies. Wave not over.

        gameMode = GameMode.Build;
        onWaveEnd.Invoke();
        LoadNewWave();
    }

    private void SpawnEnemy(int enemyCount, Paths[] paths, GameObject enemyPrefab)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            //Allocate a random path if only on enemy is spawning on this step.
            //Allocate in order if two spawn.
            MapTile[] path;
            if (enemyCount == 1) {  path = paths[Random.Range(0, paths.Length)].path; }
            else { path = paths[i].path; }

            var newEnemy = Instantiate(enemyPrefab, path[0].transform.position, Quaternion.identity).GetComponent<EnemyMovement>();
            newEnemy.SetPath(path);
            path[0].ReceiveEnemy(newEnemy);
        }
    }

    public void LoadNewWave()
    {
        currentWave++;
        currentWaveStep = 0;
        if (currentWave > waves.Length) { return; } //No waves remain
        if (waves[currentWave].hint != null)
        {
            FindObjectOfType<Hintbar>().AssignHintText(waves[currentWave].hint, true);
        }
        GetComponent<ReactOnBeat>().SetBeatsToReactOn(waves[currentWave].beatsToSpawnOn);
    }


}
