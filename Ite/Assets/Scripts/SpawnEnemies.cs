using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    int _initialNumberEnemies;
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
        switch (PlayerPrefs.GetInt("difficulty"))
        {
            case 0: _initialNumberEnemies = 5; break;
            case 1: _initialNumberEnemies = 11; break;
            case 2: _initialNumberEnemies = 20; break;
            default: _initialNumberEnemies = 50; break;
        }
        for (int i = 0; i < _initialNumberEnemies; i++)
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
