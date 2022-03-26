using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : Entity
{

    float aliveTimer = 1.0f;

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

        float absSin = Mathf.Abs(Mathf.Sin(this.transform.eulerAngles.z * (Mathf.PI / 180.0f)));
        float absCos = Mathf.Abs(Mathf.Cos(this.transform.eulerAngles.z * (Mathf.PI / 180.0f)));

        this.height = Mathf.Max(absSin * this.width, absCos * this.height);
        this.width = Mathf.Max(absSin * this.height, absCos * this.width);

        print(this.width);
        print(this.height);

    }


}
