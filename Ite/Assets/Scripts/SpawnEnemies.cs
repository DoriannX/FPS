using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] int _initialNumberEnemies;
    [SerializeField] GameObject _enemy;

    List<Transform> _spawnPositionEnemyTransform = new List<Transform>();
    Transform _transform;

    GameObject _enemyParent;
    bool _spawned;
    public int _deadEnemies = 0;

    private void Start()
    {
        _transform = transform;
        foreach(Transform child in _transform)
        {
            _spawnPositionEnemyTransform.Add(child);
        }
        _enemyParent = GameObject.Find("Enemies");
        for(int i = 0; i < _initialNumberEnemies ; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        if (_deadEnemies != 0 && _deadEnemies % 5 == 0)
        {
            _deadEnemies++;
            print("new enemy sawned");
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Transform currentPosition = _spawnPositionEnemyTransform[Random.Range(0, _spawnPositionEnemyTransform.Count)];
        Instantiate(_enemy, currentPosition.position, Quaternion.identity, _enemyParent.transform);
    }

    


}
