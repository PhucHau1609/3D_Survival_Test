using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public float respawnDelay = 10f;

    private GameObject currentEnemy;


    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if(enemyPrefabs == null)
        {
            Debug.LogWarning("No Enemy Prefabs in EnemySpawner !");
            return;
        }

        currentEnemy = Instantiate(enemyPrefabs,transform.position,transform.rotation);
        currentEnemy.GetComponent<Animal>().OnDestroyed += HandleOnEnemyDeath;


    }

    private void OnDestroy()
    {
        //currentEnemy.GetComponent<Animal>().OnDestroyed -= HandleOnEnemyDeath;
    }

    private void HandleOnEnemyDeath()
    {
        if (currentEnemy != null)
        {
            Animal animalComponent = currentEnemy.GetComponent<Animal>();
            if (animalComponent != null)
            {
                animalComponent.OnDestroyed -= HandleOnEnemyDeath;
            }
        }
        currentEnemy = null;
        Invoke(nameof(SpawnEnemy), respawnDelay);
    }
}
