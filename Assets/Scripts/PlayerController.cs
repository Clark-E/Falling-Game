using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : Entity
{
	
	public GameObject playerCamera;

    public GameObject playerSprite;
    public GameObject hatSprite;
	public GameObject owlSprite;

    float visualRotation;
    float visualRotationVelocity;

    float visualHorizontalDirection;

    const float EXAGERATE_VISUAL_ROTATION = 80.0f;

    const float VISUAL_ROTATION_DRAG = 0.98f;
    const float VISUAL_ROTATION_LERP = 0.8f;

    protected float JUMP_SPEED = 0.11f; //jump speed, when on ground
    protected float FORCED_JUMP_SPEED = 0.13f;
    protected float HORIZONTAL_ACCEL_GROUND = 0.01f;
    protected float HORIZONTAL_ACCEL_AIR = 0.004f;

    protected float GRAVITY = 0.003f;
    protected float MAX_FALL_SPEED = 0.04f;
    protected float MIN_FALL_SPEED = 0.02f;
    protected float MAX_HORIZONTAL_SPEED = 0.07f;
	
	float owlTimer;
	const int OWL_TIMER_MAX = 40;
	const float owlSpeed = 0.10f;
	
    public Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    new void Start()
    {

        base.Start();

        velocity = new Vector2(0.0f, 0.0f);

        tilemapCenter = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        this.width = 0.48f;
        this.height = 0.98f;

        visualRotation = 0.0f;
        visualRotationVelocity = 0.0f;
        visualHorizontalDirection = 1.0f;

        spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();

    }

    private bool jumped = false;

    bool grounded;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {

            jumped = true;
            
        }
		
		playerCamera.transform.position = new Vector3(this.position.x, this.position.y, playerCamera.transform.position.z);

        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0.0f, 0.0f, this.visualRotation*EXAGERATE_VISUAL_ROTATION);
        playerSprite.transform.rotation = q;
        playerSprite.transform.localScale = new Vector3(visualHorizontalDirection, 1.0f, 1.0f);

        if (this.grounded) {

            this.spriteRenderer.sprite = sprites[2];
            
        }else if (this.velocity.y > 0.0f) {

            this.spriteRenderer.sprite = sprites[1];

        }
        else{

            this.spriteRenderer.sprite = sprites[0];

        }

    }

    void FixedUpdate()
    {
		
		if(owlTimer > 0){
			
			this.velocity.x = 0.0f;
			this.velocity.y = owlSpeed;
			this.owlTimer--;
			
			if(this.owlTimer == 0){
				
				this.owlSprite.SetActive(false);
				
			}
			
		}else{
			
			if (jumped && grounded)
			{

				velocity.y = Mathf.Max(velocity.y,JUMP_SPEED);

			}
			else
			{

				velocity.y -= GRAVITY;

			}
			
			jumped = false;
			
			float usedMaxFall;
			
			if(Input.GetKey(KeyCode.UpArrow)){
				
				usedMaxFall = -MIN_FALL_SPEED;
				
			}else{
				
				usedMaxFall = -MAX_FALL_SPEED;
				
			}
			
			if (velocity.y < usedMaxFall)
			{

				velocity.y = usedMaxFall;

			}

			//this.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + velocity.y, this.gameObject.transform.position.z);

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
			
		}

        //this.transform.position = new Vector3(this.gameObject.transform.position.x + velocity.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		
		updatePosition();

        if (!grounded)
        {

            this.visualRotationVelocity -= this.visualRotation * Time.deltaTime;
            this.visualRotationVelocity -= this.velocity.x * Time.deltaTime;

            this.visualRotationVelocity *= VISUAL_ROTATION_DRAG;

            this.visualRotation += visualRotationVelocity;

        }
        else {

            this.visualRotation *= VISUAL_ROTATION_LERP;
            this.visualRotationVelocity *= VISUAL_ROTATION_LERP;
            
        }

        if (this.velocity.x > 0.0f)
        {

            this.visualHorizontalDirection = -1.0f;

        }
        else if (this.velocity.x < 0.0f) {

            this.visualHorizontalDirection = 1.0f;

        }

    }
	
	void OnTriggerEnter2D(Collider2D other){

        //print(other);

        if (other.gameObject.CompareTag("Cloud") || other.gameObject.CompareTag("PigeonPoop"))
        {

            this.velocity.y = FORCED_JUMP_SPEED;
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Pigeon")) {

            this.velocity.y = FORCED_JUMP_SPEED;

        }else if(other.gameObject.CompareTag("Hat")){

            hatSprite.GetComponent<SpriteRenderer>().sprite = other.GetComponent<SpriteRenderer>().sprite;
            Destroy(other.gameObject);
			
		}else if(other.gameObject.CompareTag("Nest")){
			
			//Destroy(other.gameObject);
			//this.velocity.y = FORCED_JUMP_SPEED;
			this.owlTimer = OWL_TIMER_MAX;
			
			this.owlSprite.SetActive(true);
			
		}
		
	}

}
