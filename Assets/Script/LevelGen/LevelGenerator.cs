using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.LevelGen {
    public class LevelGenerator : MonoBehaviour
    {   
        [SerializeField] private Tilemap topLeft;
        [SerializeField] private Tilemap topRight;
        [SerializeField] private Tilemap bottomLeft;
        [SerializeField] private Tilemap bottomRight;
        
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile outsideCorner;
        [SerializeField] private Tile outsideWall;
        [SerializeField] private Tile insideCorner;
        [SerializeField] private Tile insideWall;
        [SerializeField] private Tile tJunction;
        [SerializeField] private Tile floor;
        [SerializeField] private GameObject pellet;
        [SerializeField] private GameObject powerPellet;
    
        int[,] _levelMap =
        {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };
    
        //private float _tileSize = 1.0f;
        private Vector3 _startPos = new Vector3(-15f, 8f, 0f);

    
        void Start() {
            DeleteQuadrant();
            GenerateMap();
        }

        // Update is called once per frame
        /*void Update()
        {
        
        }*/

        private void DeleteQuadrant() {
           topLeft.ClearAllTiles();
           topRight.ClearAllTiles();
           bottomLeft.ClearAllTiles();
           bottomRight.ClearAllTiles();
        }
    
        private void GenerateMap() {
            for (int row = 0; row < _levelMap.GetLength(0); row++)
            {
                for (int col = 0; col < _levelMap.GetLength(1); col++)
                {
                    Vector3Int tilePosition = new Vector3Int((int)_startPos.x + col,(int)_startPos.y - row, 0);

                    int tileType = _levelMap[row, col];

                    switch (tileType)
                    {
                        case 0:
                            tilemap.SetTile(tilePosition, floor);
                            break;
                        case 1:
                            tilemap.SetTile(tilePosition, outsideCorner);
                            break;
                        case 2:
                            tilemap.SetTile(tilePosition, outsideWall);
                            break;
                        case 3:
                            tilemap.SetTile(tilePosition, insideCorner);
                            break;
                        case 4:
                            tilemap.SetTile(tilePosition, insideWall);
                            break;
                        case 5:
                            tilemap.SetTile(tilePosition, floor);
                            Vector3 pelletPosition = new Vector3(tilePosition.x + 1.5f, tilePosition.y + 5.5f, 0);
                            Instantiate(pellet, pelletPosition, Quaternion.identity);
                            break;
                        case 6: 
                            tilemap.SetTile(tilePosition, floor);
                            Vector3 powerPelletPosition = new Vector3(tilePosition.x + 1.5f, tilePosition.y + 5f, 0);
                            Instantiate(powerPellet, powerPelletPosition, Quaternion.identity);
                            break;
                        case 7:
                            tilemap.SetTile(tilePosition, tJunction);
                            break;
                        default:
                            tilemap.SetTile(tilePosition, null);
                            break;
                    }
                }
            }
        }
    }
    
    // TBD: aligning walls method | mirroring all of the above. 
}
