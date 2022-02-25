using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Entity
{
	
	public GameObject playerCamera;
	
    new void Start()
    {

        base.Start();

        velocity = new Vector2(0.0f, 0.0f);

        tilemapCenter = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        //tilemapCenter += new Vector3(0.5f, 0.5f, 0.0f);

        //print(tilemapCenter);

    }

    private bool jumped = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {

            jumped = true;
            
        }
		
		playerCamera.transform.position = new Vector3(this.position.x, this.position.y, playerCamera.transform.position.z);
		
    }

    void FixedUpdate()
    {

        if (jumped)
        {

            velocity.y = Mathf.Max(velocity.y,JUMP_SPEED);

            jumped = false;

        }
        else
        {

            velocity.y -= GRAVITY;

        }

        if (velocity.y < -MAX_FALL_SPEED)
        {

            velocity.y = -MAX_FALL_SPEED;

        }

        //this.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + velocity.y, this.gameObject.transform.position.z);

        position.y += velocity.y;

        rectifyCollision(false);

        float usedHorizontalAcceleration;

        if (isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION, BOUNDS_THRESHOLD_EPSILION, 0.0f, -BOUNDS_THRESHOLD_EPSILION*2.0f)))
        {

            usedHorizontalAcceleration = HORIZONTAL_ACCEL_GROUND;

        }
        else {

            usedHorizontalAcceleration = HORIZONTAL_ACCEL_AIR;


        }

        if (Input.GetKey(KeyCode.RightArrow))
        {

            velocity.x = Mathf.Min(velocity.x + usedHorizontalAcceleration, MAX_HORIZONTAL_SPEED);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {

            velocity.x = Mathf.Max(velocity.x - usedHorizontalAcceleration, -MAX_HORIZONTAL_SPEED);

        }
        else
        {

            velocity.x = Mathf.Sign(velocity.x) * Mathf.Max(Mathf.Abs(velocity.x) - usedHorizontalAcceleration, 0.0f);

        }

        //this.transform.position = new Vector3(this.gameObject.transform.position.x + velocity.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

        position.x += velocity.x;

        rectifyCollision(true);

        setVisualPosition();

    }
	
	void OnTriggerEnter2D(Collider2D other){
		
		//print(other);
		
		if(other.gameObject.CompareTag("Cloud")){
			
			this.velocity.y = FORCED_JUMP_SPEED;
            Destroy(other.gameObject);
			
		}
		
	}
	
}
