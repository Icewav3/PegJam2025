using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<EnemyBrain> valid = _enemiesToSpawn.Where(e => e.MinScore <= SceneGod.SInstance.PlayerScore).Select(e => e.EnemyBrain).ToList();

            Vector2 spawnPos = Random.insideUnitCircle * _arenaWidth;

            if (Vector2.Distance(spawnPos, SceneGod.SInstance.player.transform.position) < 15) return;

            EnemyBrain enemy = Instantiate(valid[Random.Range(0,valid.Count)], spawnPos, Quaternion.identity);

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