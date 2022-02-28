using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : Entity
{

    float aliveTimer = 3.0f;

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 deltaPosition = Time.deltaTime* velocity;

        this.position += new Vector2(deltaPosition.x, deltaPosition.y);

        setVisualPosition();

        aliveTimer -= Time.deltaTime;

        if (aliveTimer <= 0.0f && isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION, BOUNDS_THRESHOLD_EPSILION))) {

            Destroy(this.gameObject);
            
        }

    }

    new void Start()
    {

        base.Start();

        this.width = 1.5f;
        this.height = 0.5f;

    }


}
