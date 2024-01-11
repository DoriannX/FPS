using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCollisionDetection : MonoBehaviour
{
    Transform _transform;
    public bool obstacleDetected { get; private set; } = false;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(_transform.position, _transform.forward, out hit))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
            {
                obstacleDetected = true;
                return;
            }
        }
        RaycastHit hitLeft;
        if(obstacleDetected)
        {
            if(Physics.Raycast(_transform.position, -_transform.right, out hitLeft))
            {
                if(hitLeft.collider == hit.collider)
                {
                    obstacleDetected = false;
                }
            }
            else
            {
                obstacleDetected = false;
            }
        }
    }
}
