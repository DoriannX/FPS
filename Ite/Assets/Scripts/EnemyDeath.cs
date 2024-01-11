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
    private void Start()
    {
        _transform = transform;
        _player = GameObject.Find("Player");
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        GetChildren(_transform, true);
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 9)
        {
            _rb.isKinematic = false;
            _collider.enabled = false;
            _navMeshAgent.enabled = false;
            print("dead");
            GetChildren(transform, false);
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
