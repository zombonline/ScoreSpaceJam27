using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public static GameMode gameMode { get; private set; } = GameMode.Build;

    [SerializeField] Paths[] pathsMouse, pathsBird, pathsDog;

    [SerializeField] GameObject mousePrefab, birdPrefab, dogPrefab, mouseLvl2Prefab, dogLvl2Prefab, mouseLvl3Prefab, dogLvl3Prefab;

    [SerializeField] UnityEvent onWaveEnd, onWaveStart;

    [SerializeField] TextMeshProUGUI textWaveCounter;

    float newWaveDelay = 5f;
    float newWaveDelayTimer;
    private void Awake()
    {
        LoadNewWave();
    }

    private void Update()
    {
        newWaveDelayTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) && gameMode == GameMode.Build && newWaveDelayTimer <= 0)
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
        SpawnEnemy(waveStep.micelvl2, pathsMouse, mouseLvl2Prefab);
        SpawnEnemy(waveStep.dogslvl2, pathsDog, dogLvl2Prefab);
        SpawnEnemy(waveStep.micelvl3, pathsMouse, mouseLvl3Prefab);
        SpawnEnemy(waveStep.dogslvl3, pathsDog, dogLvl3Prefab);
        currentWaveStep++;
    }
    //do every beat
    public void CheckForWaveEnd()
    {
        if (currentWaveStep < waves[currentWave].waveSteps.Length) { return; } //Wave still has unactioned steps. Wave not over.
        if (FindObjectsOfType<EnemyMovement>().Length > 0) { return; } //Wave still has active enemies. Wave not over.

        gameMode = GameMode.Build;
        newWaveDelayTimer = newWaveDelay;
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
        textWaveCounter.text = "Wave " + (currentWave+1).ToString();
        currentWaveStep = 0;
        if (currentWave > waves.Length) { return; } //No waves remain
        if (waves[currentWave].hint != null)
        {
            FindObjectOfType<Hintbar>().AssignHintText(waves[currentWave].hint, true);
        }
        GetComponent<ReactOnBeat>().SetBeatsToReactOn(waves[currentWave].beatsToSpawnOn);
    }

    public void SetGameMode(GameMode val)
    {
        gameMode = val;
    }
}
