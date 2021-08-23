using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
	// public GameObject mapObject;
	Tilemap tilemap;
	Grid grid;	
 	BoundsInt bounds;
    List<GameObject> mapObjects = new List<GameObject>();
 	
	public static Vector2Int[] directions = new Vector2Int[]{
		Vector2Int.left,Vector2Int.up,
		Vector2Int.right,Vector2Int.down 
	};	
	Vector2Int[] diagonals = new Vector2Int[]{
		Vector2Int.left+Vector2Int.right, Vector2Int.right+Vector2Int.up, 
		Vector2Int.up+Vector2Int.down, Vector2Int.down+Vector2Int.left
	};

	void Start () {
        CreateMap(PersistentManager.Instance.maps[0]);
	}
	public void CreateMap(GameObject mapObject) {

        if (mapObjects.Count > 0) {

            for (int i = mapObjects.Count - 1; i >= 0; i--) {
                Destroy(mapObjects[i]);
            }

            mapObjects.Clear();
        }

        tilemap = mapObject.GetComponentInChildren<Tilemap>();
		tilemap.CompressBounds();
		bounds = tilemap.cellBounds;
		grid = mapObject.GetComponent<Grid>();

        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {

		        ScriptableTile tileBase = (ScriptableTile)allTiles[x + y * bounds.size.x];
                Vector3 vecInt = grid.CellToWorld(new Vector3Int(x+bounds.position.x,y+bounds.position.y,0));

				if (tileBase) {
					string name = tileBase.name;

                    GameObject associatedPrefab = Instantiate(tileBase.associatedPrefab) as GameObject;

                    if (associatedPrefab.GetComponentInChildren<PlayerScript>()) {
                        //find player and assign cinemachine virtual camera to follow it
                        GameManager.Instance.vcam.Follow = associatedPrefab.transform.GetChild(0);
                        // GameManager.Instance.vcam.LookAt = associatedPrefab.transform.GetChild(0);
                    }

					associatedPrefab.transform.position = new Vector2(vecInt.x+.5f,vecInt.y+.5f);

                    mapObjects.Add(associatedPrefab);
				}
    	
            }
        }

        // Destroy(mapObject);
    }

    public void ResetMap() {

    }

	public Vector3Int WorldToCell (Vector3 n) {
		return grid.WorldToCell(n)-bounds.position;		
	}
}
