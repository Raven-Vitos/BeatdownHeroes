using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; // Префаб врага

    [SerializeField]
    private Transform ground;

    [SerializeField]
    private float SpawnWidth;
    [SerializeField]
    private float SpawnHeight;

    [SerializeField]
    private float spawnInterval = 10.0f; // Интервал спавна врагов

    [SerializeField]
    private int countEnemy = 4;

    [SerializeField]
    private Text countDeadEnemies;

    private Camera mainCamera;
    private int EnemyDeadCount = 0;

    void Start()
    {
        mainCamera = Camera.main;
        for (var i = 0; i < countEnemy; i++)
        {
            Invoke("SpawnEnemy", spawnInterval * i);
        }

        GameData.playerScore = 0;
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Подписаться на события смерти и респавна
        enemy.GetComponent<EnemyAI>().eventEnemyDied = EnemyDead;
        enemy.GetComponent<EnemyAI>().eventEnemyDestroy = SpawnEnemy;
    }

    public void EnemyDead() 
    {
        EnemyDeadCount++;
        countDeadEnemies.text = EnemyDeadCount.ToString();

        GameData.playerScore = EnemyDeadCount;
    }

    Vector3 GetSpawnPosition()
    {

        // Рассчитываем возможные координаты для спавна. За пределами экрана
        float randomX, randomZ;
        randomX = Random.Range(ground.position.x - SpawnHeight / 2, ground.position.x + SpawnHeight / 2);
        randomZ = Random.Range(ground.position.z - SpawnWidth / 2, ground.position.z + SpawnWidth / 2);
        Vector3 pos = new Vector3(randomX, 0, randomZ);

        while (IsObjectInCameraView(pos))
        {
            randomX = Random.Range(ground.position.x - SpawnHeight / 2, ground.position.x + SpawnHeight / 2);
            randomZ = Random.Range(ground.position.z - SpawnWidth / 2, ground.position.z + SpawnWidth / 2);

            pos = new Vector3(randomX, 0, randomZ);
        }

        return pos;
    }

    bool IsObjectInCameraView(Vector3 worldPosition)
    {
        // Преобразуем мировые координаты в экранные
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPosition);

        // Проверяем, находятся ли координаты внутри видимой области камеры (от 0 до 1 по x и y)
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
