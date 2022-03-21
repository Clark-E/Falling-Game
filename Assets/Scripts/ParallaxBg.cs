using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
	
	public Transform foregroundTrees;
	public Transform backgroundTrees;
	public Transform mountains;
	
	const float REPEAT_LENGTH_FOREGROUND_TREES = 2.63f;
	const float REPEAT_LENGTH_BACKGROUND_TREES = 2.72f;
	const float REPEAT_LENGTH_MOUNTAIN = 2.72f;
	
	void setScroll(ref Transform transform, float speed, float repeatLength){
		
		transform.localPosition = new Vector3(Mathf.Repeat(this.transform.parent.position.x*speed,repeatLength) - repeatLength*0.5f, transform.localPosition.y, transform.localPosition.z);
		
	}
	
    // Update is called once per frame
    void Update()
    {
        
		setScroll(ref foregroundTrees,-0.05f,REPEAT_LENGTH_FOREGROUND_TREES);
		setScroll(ref backgroundTrees,-0.03f,REPEAT_LENGTH_BACKGROUND_TREES);
		setScroll(ref mountains,-0.005f,REPEAT_LENGTH_MOUNTAIN);
		
    }
}
