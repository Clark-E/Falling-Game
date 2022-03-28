using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonPoopController : CloudController
{
	
	const float POOP_WIDTH = 0.7f;
	const float POOP_HEIGHT = 0.7f;
	
	const float WALL_CHECK_THRESHOLD = 0.05f;
	const int NUM_WALL_CHECKS = 16;
	
	new void FixedUpdate(){
		
		base.FixedUpdate();
		
		/*bool tileAtLeft = isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION,BOUNDS_THRESHOLD_EPSILION,-WALL_CHECK_THRESHOLD,0.0f));
		bool tileAtRight = isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION,BOUNDS_THRESHOLD_EPSILION,WALL_CHECK_THRESHOLD,0.0f));*/
		
		int wallDir = 0;
		
		if(isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION,BOUNDS_THRESHOLD_EPSILION,0.0f,0.0f))){
			
			for(int i = 0; i < NUM_WALL_CHECKS; i++){
				
				if(isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION,BOUNDS_THRESHOLD_EPSILION,-WALL_CHECK_THRESHOLD*(i+1),0.0f))){
					
					wallDir--;
					
				}
				
			}
			
			for(int i = 0; i < NUM_WALL_CHECKS; i++){
				
				if(isColliding(checkTileCollision(BOUNDS_THRESHOLD_EPSILION,BOUNDS_THRESHOLD_EPSILION,WALL_CHECK_THRESHOLD*(i+1),0.0f))){
					
					wallDir++;
					
				}
				
			}
			
		}
		
		//this.velocity.x = 1.0f;
		
		if(wallDir < 0){
			
			this.velocity.x = 1.0f;
			
		}else if(wallDir > 0){
			
			this.velocity.x = -1.0f;
			
		}else{
			
			this.velocity.x = 0.0f;
			
		}
		
	}
	
	new void Start(){
		
		base.Start();
		
		this.width = POOP_WIDTH;
		this.height = POOP_HEIGHT;
		
	}
	
}
