using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    [SerializeField]
    private HealthPickup _pickup;

    [SerializeField]
    private int _maxPickups = 5;

    [SerializeField]
    private float _arenaWidth = 25;

    private List<HealthPickup> _pickups = new List<HealthPickup>();

    private void Update()
    {
        if (_pickups.Count < _maxPickups)
        {
            Vector2 spawnPos = Random.insideUnitCircle * _arenaWidth;

            if (Vector2.Distance(spawnPos, SceneGod.SInstance.player.transform.position) < 15) return;

            HealthPickup hp = Instantiate(_pickup, spawnPos, Quaternion.identity);

            _pickups.Add(hp);
        }

        _pickups = _pickups.Where(_pickup => _pickup != null).ToList();
    }
}
