using UnityEngine;

public class DestroyChair : MonoBehaviour
{

    BoxCollider _collider;
    [SerializeField] LayerMask _layerToStopExclude;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 15);
        }
    }
    private void Awake()
    {
        Invoke(nameof(TouchPlayer), 1);
    }

    void TouchPlayer()
    {
        _collider.excludeLayers = _layerToStopExclude;
    }

}
