using UnityEngine;

public class DestroyChair : MonoBehaviour
{

    MeshCollider _collider;
    Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Enemy")
        {
            //transform.SetParent(collision.transform, true);
            //_rb.isKinematic = true;
            _rb.AddForce(-_rb.velocity, ForceMode.Impulse);
        }
        Destroy(gameObject, 5);
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
