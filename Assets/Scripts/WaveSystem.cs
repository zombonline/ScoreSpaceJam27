using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameMode
{
    Battle,
    Build
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] SOWave[] waves;

    int currentWave, currentWaveStep;

    public GameMode gameMode { get; private set; }

    [SerializeField] MapTile[] spawnTilesMouse, spawnTilesBird, spawnTilesDog;

    [SerializeField] GameObject mousePrefab, birdPrefab, dogPrefab;

    GameObject[] activeEnemies;

    [SerializeField] UnityEvent onWaveEnd, onWaveStart;


    //do every beat
    public void SpawnWaveStep()
    {
        currentWaveStep++;
        if(currentWaveStep >= waves[currentWave].waveSteps.Length) { return; } //No more steps left in wave 

        var waveStep = waves[currentWave].waveSteps[currentWaveStep];
        SpawnEnemy(waveStep.mice, spawnTilesMouse, mousePrefab);
        SpawnEnemy(waveStep.birds, spawnTilesBird, birdPrefab);
        SpawnEnemy(waveStep.dogs, spawnTilesDog, dogPrefab);
    }
    //do every beat
    public void CheckForWaveEnd()
    {
        if (currentWaveStep < waves[currentWave].waveSteps.Length) { return; } //Wave still has unactioned steps. Wave not over.
        if (activeEnemies.Length > 0) { return; } //Wave still has active enemies. Wave not over.

        gameMode = GameMode.Build;
        onWaveEnd.Invoke();

    }

    private void SpawnEnemy(int enemyCount, MapTile[] spawnTiles, GameObject enemyPrefab)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            MapTile tileToSpawnOn = spawnTilesBird[Random.Range(0, spawnTiles.Length)];
            while (tileToSpawnOn.enemies.Count == 0)
            {
                tileToSpawnOn = spawnTilesBird[Random.Range(0, spawnTiles.Length)];
            }
            tileToSpawnOn.ReceiveEnemy(enemyPrefab);
        }
    }
}
