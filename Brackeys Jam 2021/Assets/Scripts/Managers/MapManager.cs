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
    public List<Obstacle> destructableObjects = new List<Obstacle>();
    public GameObject floorContainer;
 	
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

        ResetMap();

        tilemap = mapObject.GetComponentInChildren<Tilemap>();
		tilemap.CompressBounds();
		bounds = tilemap.cellBounds;
		grid = mapObject.GetComponent<Grid>();

        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        GameObject gameObjectHolder = new GameObject("Gameobject Holder");

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {

		        ScriptableTile tileBase = (ScriptableTile)allTiles[x + y * bounds.size.x];
                Vector3 vecInt = grid.CellToWorld(new Vector3Int(x+bounds.position.x,y+bounds.position.y,0));

				if (tileBase) {
					string name = tileBase.name;

                    GameObject associatedPrefab = Instantiate(tileBase.associatedPrefab) as GameObject;
                    associatedPrefab.transform.SetParent(gameObjectHolder.transform);

                    if (associatedPrefab.name.Contains("Wall")) {
                        associatedPrefab.GetComponentInChildren<SpriteRenderer>().sprite = tileBase.sprite;
                    }

                    if (associatedPrefab.GetComponentInChildren<PlayerScript>()) {
                        GameManager.Instance.playerManager.playerScript = associatedPrefab.GetComponentInChildren<PlayerScript>();
                        GameManager.Instance.playerManager.playerScript.playerShootingBehaviour = associatedPrefab.GetComponentInChildren<ShootingBehaviour>();
                        Camera.main.gameObject.transform.position = associatedPrefab.transform.position;
                        //find player and assign cinemachine virtual camera to follow it
                        GameManager.Instance.vcam.Follow = GameManager.Instance.playerManager.playerScript.lookTarget.transform;
                    }
                    
                    if (associatedPrefab.GetComponentInChildren<Obstacle>()) {
                        destructableObjects.Add(associatedPrefab.GetComponentInChildren<Obstacle>());
                    }

					associatedPrefab.transform.position = new Vector2(vecInt.x+.5f,vecInt.y+.5f);
                    associatedPrefab.name = name + " - " + associatedPrefab.transform.position;

                    //put floor beneath every tile
                    if (!associatedPrefab.name.Contains("Floor")) {
                        GameObject floor = Instantiate(floorContainer, associatedPrefab.transform.position, Quaternion.identity);
                        floor.transform.SetParent(gameObjectHolder.transform);
                    }

                    mapObjects.Add(associatedPrefab);
				} else {
                    GameObject floor = Instantiate(floorContainer, new Vector2(vecInt.x+.5f,vecInt.y+.5f), Quaternion.identity);
                    floor.transform.SetParent(gameObjectHolder.transform);
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

        GameManager.Instance.enemyManager.currentEnemies.Clear();
        GameManager.Instance.enemyManager.currentSpawners.Clear();
        destructableObjects.Clear();

        if (mapObjects.Count > 0) {

            for (int i = mapObjects.Count - 1; i >= 0; i--) {
                Destroy(mapObjects[i]);
            }

            mapObjects.Clear();
        }

    }

	public Vector3Int WorldToCell (Vector3 n) {
		return grid.WorldToCell(n)-bounds.position;		
	}
}
