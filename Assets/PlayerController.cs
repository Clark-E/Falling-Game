using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

struct collisionResult {

    public bool hit;

    public int x;
    public int y;
    
}

public class PlayerController : MonoBehaviour
{
	
	public GameObject playerCamera;
	
    public Tilemap tilemap;

    private Vector2 position;

    private Vector2 velocity;

    private float GRAVITY = 0.003f;
    private float MAX_FALL_SPEED = 0.03f;
    private float JUMP_SPEED = 0.10f;
    private float FORCED_JUMP_SPEED = 0.13f;
    private float HORIZONTAL_ACCEL_GROUND = 0.02f;
    private float HORIZONTAL_ACCEL_AIR = 0.004f;
    private float MAX_HORIZONTAL_SPEED = 0.055f;

    //width and height, measured from center to edge.
    private float width = 0.5f;
    private float height = 1.0f;

    private float BOUNDS_THRESHOLD_EPSILION = 1e-6f;

    private float tileRadius = 0.5f;

    private Vector3 tilemapCenter;

    private int MAX_COLLISION_RESULTS = 16;

    void getTileBounds(out float x1, out float x2, out float y1, out float y2) {

        x1 = this.position.x - width;
        x2 = this.position.x + width;

        y1 = this.position.y - height;
        y2 = this.position.y + height;

        //x1 -= tilemapCenter.x;
        //x2 -= tilemapCenter.x;

        //y1 -= tilemapCenter.y;
        //y2 -= tilemapCenter.y;

    }

    bool isSolidTile(int x, int y) {

        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

        if (tile) {

            return (true);

        }
        else {

            return (false);

        }

    }

    collisionResult[] checkTileCollision(float widthThreshold, float heightThreshold, float xOff = 0.0f, float yOff = 0.0f) {

        collisionResult[] results = new collisionResult[MAX_COLLISION_RESULTS];

        int currentResult = 0;

        float fx1;
        float fx2;
        float fy1;
        float fy2;

        getTileBounds(out fx1, out fx2, out fy1, out fy2);

        int x1 = Mathf.FloorToInt(fx1 + widthThreshold + xOff);
        int x2 = Mathf.CeilToInt(fx2 - widthThreshold + xOff);
        int y1 = Mathf.FloorToInt(fy1 + heightThreshold + yOff);
        int y2 = Mathf.CeilToInt(fy2 - heightThreshold + yOff);

        for (int i = 0; i < results.Length; i++) {

            results[i].hit = false;
            
        }

        //print(x1.ToString() + "," + x2.ToString() + "," + y1.ToString() + "," + y2.ToString());

        for (int x = x1; x < x2; x++) {

            for (int y = y1; y < y2; y++) {

                //TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0))

                if (isSolidTile(x, y)) {

                    results[currentResult].hit = true;
                    results[currentResult].x = x;
                    results[currentResult].y = y;

                    currentResult++;

                    if (currentResult == MAX_COLLISION_RESULTS) goto doneChecking;

                }

            }

        }

        doneChecking:

        return (results);

    }

    bool isColliding(collisionResult[] results) {

        return (results[0].hit);
        
    }

    void rectifyCollision(bool doX) {

        collisionResult[] results;

        if (doX)
        {

            results = checkTileCollision(0.0f, BOUNDS_THRESHOLD_EPSILION);

        }
        else {

            results = checkTileCollision(BOUNDS_THRESHOLD_EPSILION, 0.0f);

        }

        for (int i = 0; i < results.Length; i++) {

            if (!results[i].hit) break;

            if (doX){

                float tx = (float)(results[i].x) + tileRadius;

                if (tx < this.position.x)
                {

                    if (tx + tileRadius + width > this.position.x) {

                        //push rightwards
                        this.position.x = tx + tileRadius + width;
                        this.velocity.x = 0.0f;

                    }

                }
                else {

                    if ((float)(results[i].x) - width < this.position.x){

                        //push leftwards
                        this.position.x = (float)(results[i].x) - width;
                        this.velocity.x = 0.0f;

                    }

                }

            }
            else {

                float ty = (float)(results[i].y) + tileRadius;

                if (ty < this.position.y)
                {

                    if (ty + tileRadius + height > this.position.y)
                    {

                        //push upwards
                        this.position.y = ty + tileRadius + height;
                        this.velocity.y = 0.0f;

                    }

                    //push upwards
                    //this.transform.position = new Vector3(this.transform.position.x, Mathf.Max(this.transform.position.y, ty + tileRadius + height), this.transform.position.z);

                }
                else {

                    if ((float)(results[i].y) - height < this.position.y)
                    {

                        //push downwards
                        this.position.y = (float)(results[i].y) - height;
                        this.velocity.y = 0.0f;

                    }

                }

            }
            
        }

        /*if (results.Length > 0 && results[0].hit) {

            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - velocity.y, this.gameObject.transform.position.z);
            velocity.y = 0.0f;
            
        }*/

    }

    void Start()
    {

        velocity = new Vector2(0.0f, 0.0f);

        tilemapCenter = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        position = new Vector2(this.transform.position.x, this.transform.position.y);

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

        this.transform.position = new Vector3(this.position.x, this.position.y, this.transform.position.z);

    }
	
	void OnTriggerEnter2D(Collider2D other){
		
		//print(other);
		
		if(other.gameObject.CompareTag("Cloud")){
			
			this.velocity.y = FORCED_JUMP_SPEED;
			
		}
		
	}
	
}
