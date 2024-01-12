using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    Rigidbody _rb;
    GameObject _player;
    Transform _transform;
    [SerializeField] float hitForce;
    Collider _collider;
    NavMeshAgent _navMeshAgent;
    SpawnEnemies _spawnEnemies;
    ScoreManager _scoreManager;
    public bool dead = false;
    Animator _animator;
    private void Start()
    {
        _transform = transform;
        _player = GameObject.Find("Player");
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        GetChildren(_transform, true);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _spawnEnemies = GameObject.Find("EnemiesLocations").GetComponent<SpawnEnemies>();
        _scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();
        _animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 9)
        {

            print("dead");
            Destroy(_animator);
            dead = true;
            GetChildren(transform, false);
            Destroy(_rb);
            Destroy(_collider);
            Destroy(GetComponent<AgentLinkMover>());
            Destroy(_navMeshAgent);
            _spawnEnemies.SpawnEnemy();
            _spawnEnemies._deadEnemies++;
            _scoreManager.score++;
        }
    }

    private void GetChildren(Transform parent, bool state)
    {
        foreach (Transform child in parent)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childBody))
            {
                childBody.interpolation = RigidbodyInterpolation.Interpolate;
                childBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                childBody.isKinematic = state;
                child.GetComponent<Collider>().enabled = !state;
                Vector3 forceDirection = (_transform.position - _player.transform.position).normalized * hitForce;
                if (!state)
                {
                    childBody.AddForce(forceDirection, ForceMode.Impulse);
                    childBody.mass *= .001f;
                    Destroy(_rb);
                }
            }
            GetChildren(child, state);
        }
    }
}
