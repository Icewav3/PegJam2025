using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int _maxEnemies;

    [SerializeField]
    private float _arenaWidth;

    [SerializeField]
    private List<SpawnableEnemy> _enemiesToSpawn = new List<SpawnableEnemy>();

    private List<EnemyBrain> _enemies = new List<EnemyBrain>();

    private void Update()
    {
        if(_enemies.Count < _maxEnemies)
        {
            Vector2 spawnPos = Random.insideUnitCircle * _arenaWidth;
            if (Vector2.Distance(spawnPos, SceneGod.SInstance.player.transform.position) < 15) return;
            EnemyBrain enemy = Instantiate(_enemiesToSpawn[Random.Range(0,_enemiesToSpawn.Count)].EnemyBrain, spawnPos, Quaternion.identity);
            _enemies.Add(enemy);

            enemy.OnDeath += HandleDeath;
        }
    }

    private void HandleDeath(EnemyBrain brain, DamageReceiver dr, DamageEvent dmgEvent)
    {
        brain.OnDeath -= HandleDeath;

        _enemies.Remove(brain);

        SceneGod.SInstance.IncrementScore(brain.ScoreValue);

        Destroy(brain.gameObject);
    }
}

[System.Serializable]
public class SpawnableEnemy
{
    [SerializeField]
    private EnemyBrain _enemyBrain;
    public EnemyBrain EnemyBrain => _enemyBrain;

    [SerializeField]
    private int _minScore;
    public int MinScore => _minScore; 
}