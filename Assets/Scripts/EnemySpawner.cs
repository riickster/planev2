using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float minDistanceBetweenEnemies = 1.0f;
    public float minYOffset = -2f;
    public float maxYOffset = 2f;
    public int enemiesToSpawn = 1;

    private Camera mainCamera;
    private float halfWidth;

    private float timer = 0f;

    void Start(){
        mainCamera = Camera.main;
        halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        SpawnEnemy();
    }

    void Update(){
        timer += Time.deltaTime;

        if (timer >= spawnInterval){
            for (int i = 0; i < enemiesToSpawn; i++){
                SpawnEnemy();
            }
            timer = 0f;
        }
    }

    void SpawnEnemy(){
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomYOffset = Random.Range(minYOffset, maxYOffset);

        Vector3 spawnPosition = new Vector3(randomX, transform.position.y + randomYOffset, transform.position.z);

        if (!IsTooCloseToExistingEnemies(spawnPosition)){
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        } else{
            SpawnEnemy();
        }
    }

    bool IsTooCloseToExistingEnemies(Vector3 newPosition){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemigos");

        foreach (GameObject enemy in enemies){
            Vector3 existingPosition = enemy.transform.position;
            if (Vector3.Distance(newPosition, existingPosition) < minDistanceBetweenEnemies){
                return true;
            }
        }
        return false;
    }
}
