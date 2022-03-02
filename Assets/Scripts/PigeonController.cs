using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonController : Entity
{

    public Vector2 direction;
    float signDirection;

    new void Start()
    {
        base.Start();
        this.width = 1.0f;
        this.height = 1.0f;

        //direction.Normalize();

        signDirection = 1.0f;

    }

    void FixedUpdate()
    {

        Vector2 deltaPosition = direction * signDirection * Time.deltaTime;

        this.velocity = deltaPosition;

        updatePosition();

        if (this.hitTile) {

            signDirection *= -1;
            
        }

    }
}
