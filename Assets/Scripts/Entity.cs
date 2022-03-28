using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct collisionResult
{

    public bool hit;

    public int x;
    public int y;

}

public class Entity : MonoBehaviour
{

    public Tilemap tilemap;
	
	public TileLookup tileLookup;
	
    protected Vector2 position;
    protected Vector2 previousPosition;

    [HideInInspector]
    public Vector2 velocity;

    //width and height, measured from center to edge.
    public float width = 0.5f;
    public float height = 1.0f;

    protected float BOUNDS_THRESHOLD_EPSILION = 1e-5f;

    protected float tileRadius = 0.5f;

    protected Vector3 tilemapCenter;

    protected int MAX_COLLISION_RESULTS = 16;

    protected bool hitTile;
	
    protected void getTileBounds(out float x1, out float x2, out float y1, out float y2)
    {

        x1 = this.position.x - width;
        x2 = this.position.x + width;

        y1 = this.position.y - height;
        y2 = this.position.y + height;

        //x1 -= tilemapCenter.x;
        //x2 -= tilemapCenter.x;

        //y1 -= tilemapCenter.y;
        //y2 -= tilemapCenter.y;

    }

    protected bool isSolidTile(int x, int y)
    {

        TileBase tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));
		
		if (tile)
        {

            //print("One Way");

			if(tile == tileLookup.oneWayPlatform){

                //print(y.ToString() + (previousPosition.y - height - 1.0f).ToString());

                if (y - BOUNDS_THRESHOLD_EPSILION >= (previousPosition.y - height - 1.0f))
                {
					
					return(false);
					
				}

                return(true);
                //return (false);

			}else{
				
				return(true);
				
			}
			
        }
        else
        {

            return (false);

        }

    }

    protected collisionResult[] checkTileCollision(float widthThreshold, float heightThreshold, float xOff = 0.0f, float yOff = 0.0f)
    {

        collisionResult[] results = new collisionResult[MAX_COLLISION_RESULTS];

        int currentResult = 0;

        float fx1;
        float fx2;
        float fy1;
        float fy2;

        getTileBounds(out fx1, out fx2, out fy1, out fy2);

        int x1 = Mathf.FloorToInt(fx1 + (widthThreshold + xOff));
        int x2 = Mathf.CeilToInt(fx2 - (widthThreshold + xOff));
        int y1 = Mathf.FloorToInt(fy1 + (heightThreshold + yOff));
        int y2 = Mathf.CeilToInt(fy2 - (heightThreshold + yOff));

        for (int i = 0; i < results.Length; i++)
        {

            results[i].hit = false;

        }

        //print(x1.ToString() + "," + x2.ToString() + "," + y1.ToString() + "," + y2.ToString());

        for (int x = x1; x < x2; x++)
        {

            for (int y = y1; y < y2; y++)
            {

                //TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0))

                if (isSolidTile(x, y))
                {

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

    protected bool isColliding(collisionResult[] results)
    {

        return (results[0].hit);

    }

    protected void rectifyCollision(bool doX)
    {

        collisionResult[] results;

        if (doX)
        {

            results = checkTileCollision(0.0f, BOUNDS_THRESHOLD_EPSILION);

        }
        else
        {

            results = checkTileCollision(BOUNDS_THRESHOLD_EPSILION, 0.0f);

        }

        for (int i = 0; i < results.Length; i++)
        {

            if (!results[i].hit) break;

            this.hitTile = true;

            if (doX)
            {

                float tx = (float)(results[i].x) + tileRadius;

                if (tx < this.position.x)
                {

                    if (tx + tileRadius + width > this.position.x)
                    {

                        //push rightwards
                        this.position.x = tx + tileRadius + width;
                        this.velocity.x = 0.0f;

                    }

                }
                else
                {

                    if ((float)(results[i].x) - width < this.position.x)
                    {

                        //push leftwards
                        this.position.x = (float)(results[i].x) - width;
                        this.velocity.x = 0.0f;

                    }

                }

            }
            else
            {

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
                else
                {

                    if ((float)(results[i].y) - height < this.position.y)
                    {

                        //push downwards
                        this.position.y = (float)(results[i].y) - height;
                        this.velocity.y = 0.0f;

                    }
					
                }
				
            }
			
        }
		
    }

    public void setVisualPosition() {

        this.transform.position = new Vector3(this.position.x, this.position.y, this.transform.position.z);
		
    }
	
	public void updatePosition(){

        this.hitTile = false;
		this.previousPosition = this.position;
		
        position.y += velocity.y;

        rectifyCollision(false);

        position.x += velocity.x;

        rectifyCollision(true);

        setVisualPosition();

		
	}

    public void Start()
    {

        this.tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        this.position = new Vector2(this.transform.position.x, this.transform.position.y);
		
		tileLookup = GameObject.Find("TileLookup").GetComponent<TileLookup>();
		
    }

}
