using UnityEngine;

public class DestroyChair : MonoBehaviour
{

    MeshCollider _collider;
    Rigidbody _rb;
    bool stop = false;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Enemy")
        {
            //transform.SetParent(collision.transform, true);
            //_rb.isKinematic = true;
        }
        if (!stop)
        {
            stop = true;
            _rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
        {
            gameObject.layer = LayerMask.NameToLayer("whatIsGround");
        }
    }
    private void Awake()
    {
        //Invoke(nameof(TouchPlayer), 1);

    }

    void TouchPlayer()
    {
        gameObject.layer = 7;
    }

}
