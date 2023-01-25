using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float m_moveSpeed;
    
    private void Update()
    {
        if (isLocalPlayer)
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            Vector3 movementDir = new Vector3(horizontalAxis, 0, verticalAxis);

            transform.position += movementDir * m_moveSpeed * Time.deltaTime;
        }
    }
    
    private void HandleMovement()
    {
        HandleMovement();
    }
}
