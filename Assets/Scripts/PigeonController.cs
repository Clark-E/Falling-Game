using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonController : Entity
{

    public Vector2 direction;
    float signDirection;
	
	public GameObject poopPrefab;
	GameObject player;
	
	public float POOP_DROP_THRESHOLD;
	public float POOP_DROP_PERIOD;
	
	float poopDropTimer;

    new void Start()
    {
        base.Start();
		
        this.width = 1.0f;
        this.height = 1.0f;

        //direction.Normalize();

        signDirection = 1.0f;
		
		player = GameObject.Find("Player");
		
		poopDropTimer = 0.0f;
		
    }
	
    void FixedUpdate()
    {

        Vector2 deltaPosition = direction * signDirection * Time.deltaTime;

        this.velocity = deltaPosition;

        updatePosition();

        if (this.hitTile) {

            signDirection *= -1;
            
        }
		
		if(poopDropTimer > 0.0f){
			
			poopDropTimer -= Time.deltaTime;
			
		}else if(Mathf.Abs(player.transform.position.x - this.transform.position.x) <= POOP_DROP_THRESHOLD){
			
			GameObject newPoop = Instantiate(poopPrefab, this.transform.position, this.transform.rotation);
			
			PigeonPoopController poopController = newPoop.GetComponent<PigeonPoopController>();
			
			poopController.velocity = new Vector2(0.0f,-3.0f);
			
			poopDropTimer = POOP_DROP_PERIOD;
			
		}
		
    }
}
