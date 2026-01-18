using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float cooldownDecraseRate = 0.05f;
    [SerializeField] private float cooldownCap = 0.7f;
    private float timer;
    private Transform player;

    private void Awake() {
        player = FindFirstObjectByType<Player>().transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = cooldown;
            CreateNewEnemy();
            cooldown = Mathf.Max(cooldown - cooldownDecraseRate, cooldownCap);
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if ( createdOnTheRight ){
            newEnemy.GetComponent<Enemy>().Flip();
        }
    }
}
