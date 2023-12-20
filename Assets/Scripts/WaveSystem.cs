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

    [SerializeField] TextMeshProUGUI textBeginPrompt,textWaveEnd;

    [SerializeField] string miceSpawnSFX, mice2SpawnSFX, mice3SpawnSFX, birdSpawnSFX, dogSpawnSFX, dog2SpawnSFX, dog3SpawnSFX;
    [SerializeField] List<string> playedSFX = new List<string>();   

    float newWaveDelay = 5f;
    float newWaveDelayTimer;
    private void Awake()
    {
        LoadNewWave();
    }

    private void Update()
    {
        newWaveDelayTimer -= Time.deltaTime;
        textBeginPrompt.enabled = (newWaveDelayTimer < -5f && gameMode == GameMode.Build);
        // prompt player to begin if they can.
        if(gameMode == GameMode.Battle) { return; } //already in a wave, don't start new one.
        if(newWaveDelayTimer > 0) { return; } //wave delay timer not finished.
        if (Base.gameOver) { return; } //game over, can't start new wave.
        if(Input.GetKeyDown(KeyCode.Space))
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
        SpawnEnemy(waveStep.mice, pathsMouse, mousePrefab,miceSpawnSFX);
        SpawnEnemy(waveStep.birds, pathsBird, birdPrefab, birdSpawnSFX);
        SpawnEnemy(waveStep.dogs, pathsDog, dogPrefab,dogSpawnSFX);
        SpawnEnemy(waveStep.micelvl2, pathsMouse, mouseLvl2Prefab, mice2SpawnSFX);
        SpawnEnemy(waveStep.dogslvl2, pathsDog, dogLvl2Prefab, dog2SpawnSFX);
        SpawnEnemy(waveStep.micelvl3, pathsMouse, mouseLvl3Prefab, mice3SpawnSFX);
        SpawnEnemy(waveStep.dogslvl3, pathsDog, dogLvl3Prefab, dog3SpawnSFX);
        currentWaveStep++;
    }
    //do every beat
    public void CheckForWaveEnd()
    {
        if (currentWaveStep < waves[currentWave].waveSteps.Length) { return; } //Wave still has unactioned steps. Wave not over.
        if (FindObjectsOfType<EnemyMovement>().Length > 0 || 
            FindObjectsOfType<Coin>().Length > 0) { return; }
        //Wave still has active enemies or coins to be collected. Wave not over.

        gameMode = GameMode.Build;
        newWaveDelayTimer = newWaveDelay;
        onWaveEnd.Invoke();
        LoadNewWave();
    }

    private void SpawnEnemy(int enemyCount, Paths[] paths, GameObject enemyPrefab, string sfxToPlay)
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

            if (i == 0 && !playedSFX.Contains(sfxToPlay)) 
            {
                FMODController.PlaySFX(sfxToPlay);
                playedSFX.Add(sfxToPlay);   
            }
        }
    }

    public void LoadNewWave()
    {
        currentWave++;
        textWaveCounter.text = "Wave " + (currentWave+1).ToString();
        textWaveEnd.text = "Waves complete: " + (currentWave + 1).ToString();
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
    private void OnDisable()
    {
        gameMode = GameMode.Build;
        Debug.Log("gamemode set to " + gameMode.ToString());
    }
}
