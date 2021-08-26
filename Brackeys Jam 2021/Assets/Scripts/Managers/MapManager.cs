using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SAP2D;

public class MapManager : MonoBehaviour
{
	// public GameObject mapObject;
	Tilemap tilemap;
	Grid grid;	
 	BoundsInt bounds;
    List<GameObject> mapObjects = new List<GameObject>();
    SAP2DPathfinder pathfinder;
 	
	public static Vector2Int[] directions = new Vector2Int[]{
		Vector2Int.left,Vector2Int.up,
		Vector2Int.right,Vector2Int.down 
	};	
	Vector2Int[] diagonals = new Vector2Int[]{
		Vector2Int.left+Vector2Int.right, Vector2Int.right+Vector2Int.up, 
		Vector2Int.up+Vector2Int.down, Vector2Int.down+Vector2Int.left
	};

	void Start () {
        pathfinder = GetComponent<SAP2DPathfinder>();
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
                        GameManager.Instance.playerManager.playerScript = associatedPrefab.GetComponentInChildren<PlayerScript>();
                        GameManager.Instance.playerManager.playerScript.playerShootingBehaviour = associatedPrefab.GetComponentInChildren<ShootingBehaviour>();
                        Camera.main.gameObject.transform.position = associatedPrefab.transform.position;
                        //find player and assign cinemachine virtual camera to follow it
                        GameManager.Instance.vcam.Follow = GameManager.Instance.playerManager.playerScript.lookTarget.transform;
                    }

					associatedPrefab.transform.position = new Vector2(vecInt.x+.5f,vecInt.y+.5f);

                    mapObjects.Add(associatedPrefab);
				}
    	
            }
        }

        //set starting weapon
        GameManager.Instance.uiManager.weaponSelector.value = GameManager.Instance.uiManager.pistolIndex;

        // pathfinder.AddGrid(bounds.size.x * 2, bounds.size.y * 2);
        SAP_GridSource sapgrid = pathfinder.GetGrid(0);
        sapgrid.GridPivot = GridPivot.Center;
        sapgrid.Position = Vector3.zero;
        sapgrid.CreateGrid(bounds.size.x * 2, bounds.size.y * 2);

        pathfinder.CalculateColliders();
    }

    public void ResetMap() {

    }

	public Vector3Int WorldToCell (Vector3 n) {
		return grid.WorldToCell(n)-bounds.position;		
	}
}
