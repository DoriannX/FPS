using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    Rigidbody _rb;
    GameObject _player;
    Transform _transform;
    [SerializeField] float hitForce;
    Collider _collider;
    private void Start()
    {
        _transform = transform;
        _player = GameObject.Find("Player");
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        GetChildren(_transform, true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 9)
        {
            Destroy(_rb);
            _collider.enabled = false;
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
                }
            }
            GetChildren(child, state);
        }
    }
}
