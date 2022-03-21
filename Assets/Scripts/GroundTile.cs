using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class GroundTile : Tile
{

    public Sprite[] sprites;

    const int repeatW = 6;
    const int repeatH = 2;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        
        //tileData.sprite = this.sprite;
		
		location.x %= repeatW;
		if(location.x < 0){
			location.x = repeatW+location.x;
			location.x %= repeatW;
		}
		
		location.y %= repeatH;
		if(location.y < 0){
			location.y = repeatH+location.y;
			location.y %= repeatH;
		}
		
        int index = (location.x) + repeatW*(location.y);
		
		//if(index < 0) index += repeatW*repeatH;
		
        tileData.sprite = sprites[index];

        tileData.color = this.color;
        tileData.transform = this.transform;
        tileData.flags = this.flags;

        tileData.colliderType = this.colliderType;
    }
}
