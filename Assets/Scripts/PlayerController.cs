using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Entity
{
	
	public GameObject playerCamera;

    float visualRotation;
    float visualRotationVelocity;

    float visualHorizontalDirection;

    const float EXAGERATE_VISUAL_ROTATION = 80.0f;

    protected float JUMP_SPEED = 0.07f;
    protected float FORCED_JUMP_SPEED = 0.13f;
    protected float HORIZONTAL_ACCEL_GROUND = 0.02f;
    protected float HORIZONTAL_ACCEL_AIR = 0.004f;

    new void Start()
    {

        base.Start();

        velocity = new Vector2(0.0f, 0.0f);

        tilemapCenter = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        //this.width = 

        visualRotation = 0.0f;
        visualRotationVelocity = 0.0f;
        visualHorizontalDirection = 1.0f;

    }

    private bool jumped = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {

            jumped = true;
            
        }
		
		playerCamera.transform.position = new Vector3(this.position.x, this.position.y, playerCamera.transform.position.z);

        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0.0f, 0.0f, this.visualRotation*EXAGERATE_VISUAL_ROTATION);
        this.transform.rotation = q;
        this.transform.localScale = new Vector3(visualHorizontalDirection, 1.0f, 1.0f);

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

        bool grounded;

        float usedHorizontalAcceleration;

        if (isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION, BOUNDS_THRESHOLD_EPSILION, 0.0f, -BOUNDS_THRESHOLD_EPSILION*2.0f)))
        {

            grounded = true;
            usedHorizontalAcceleration = HORIZONTAL_ACCEL_GROUND;

        }
        else {

            grounded = false;
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
		
		updatePosition();

        if (!grounded)
        {

            this.visualRotationVelocity -= this.visualRotation * Time.deltaTime;
            this.visualRotationVelocity -= this.velocity.x * Time.deltaTime;

            this.visualRotationVelocity *= 0.95f;

            this.visualRotation += visualRotationVelocity;

        }
        else {

            this.visualRotation *= 0.8f;
            this.visualRotationVelocity *= 0.8f;
            
        }

        if (this.velocity.x > 0.0f)
        {

            this.visualHorizontalDirection = 1.0f;

        }
        else if (this.velocity.x < 0.0f) {

            this.visualHorizontalDirection = -1.0f;

        }

    }
	
	void OnTriggerEnter2D(Collider2D other){

        //print(other);

        if (other.gameObject.CompareTag("Cloud"))
        {

            this.velocity.y = FORCED_JUMP_SPEED;
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Pigeon")) {

            this.velocity.y = FORCED_JUMP_SPEED;

        }
		
	}

}
