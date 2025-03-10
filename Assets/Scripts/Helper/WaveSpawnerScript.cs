using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject[] EnemiesPrefabs;
    [SerializeField] Transform[] SpawnTransform;

    [SerializeField] float spawnInterval;
    [SerializeField] float spawnAmount;
    [SerializeField] float restTime;
    [SerializeField] AudioClip spawnSFX;
    public float countDown;
    [SerializeField] TextMeshProUGUI killCount;
    public int killCounter {get; private set;}
    public Transform Player;
    bool startCount;
    int waveCount;
    private void OnEnable()
    {
        EventsManager.instance.miscEvents.onEnemyKilled += UpdateKillCount;
    }

    private void OnDisable()
    {
        EventsManager.instance.miscEvents.onEnemyKilled -= UpdateKillCount;
    }
    void Start()
    {
        countDown = spawnInterval;
    }
    private void Update()
    {
        if(startCount)
        {
            countDown -= Time.deltaTime;
        }
        if(waveCount % 5 == 0 && waveCount < 4)
        {
            if(!resting)
            {
                StartCoroutine(PauseArena());
            }
        }
    }
    public void BeginArena()
    {
        startCount = true;
        InvokeRepeating("SpawnEnemies", spawnInterval, spawnInterval);
    }
    private bool resting;
    IEnumerator PauseArena()
    {
        resting = true;
        CancelInvoke("SpawnEnemies");
        
        yield return new WaitForSecondsRealtime(restTime);
        foreach(var enemy in EnemiesPrefabs)
        {  
            enemy.GetComponent<Health>().SetHealth(2f);
        }
        resting = false;
    }
    void UpdateKillCount()
    {
        killCounter++;
        if(killCounter % 10 != 0)
        {
            ///Play Animation and Sound;
        }
        killCount.SetText(killCounter.ToString());
    }
    void SpawnEnemies()
    {
        waveCount++;
        SoundFXManager.instance.PlaySingleSoundFXClip(spawnSFX, transform.position, 1f);
        for(int i = 0; i < spawnAmount; i++)
        {
            GameObject spawnedEnemy = Instantiate(EnemiesPrefabs[UnityEngine.Random.Range(0, EnemiesPrefabs.Length)], 
                    SpawnTransform[UnityEngine.Random.Range(0, SpawnTransform.Length - 1)]);
            spawnedEnemy.GetComponent<Enemy>().player = Player;
        }
        startCount = false;
    }

   
}
