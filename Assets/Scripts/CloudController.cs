using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : Entity
{

	[HideInInspector]
    public float aliveTimer = 1.5f;

    // Update is called once per frame
    public void FixedUpdate()
    {

        Vector2 deltaPosition = Time.deltaTime* this.velocity;

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

		float angle = this.transform.eulerAngles.z * (Mathf.PI / 180.0f);
		
        float absSin = Mathf.Abs(Mathf.Sin(angle));
        float absCos = Mathf.Abs(Mathf.Cos(angle));

        float newHeight = Mathf.Max(absSin * this.width, absCos * this.height);
        float newWidth = Mathf.Max(absSin * this.height, absCos * this.width);

		this.height = newHeight;
		this.width = newWidth;
		
    }


}
