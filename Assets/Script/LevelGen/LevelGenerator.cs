using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.LevelGen {
    public class LevelGenerator : MonoBehaviour
    {   
        // to delete existing quadrants
        [SerializeField] private Tilemap topLeft;
        [SerializeField] private Tilemap topRight;
        [SerializeField] private Tilemap bottomLeft;
        [SerializeField] private Tilemap bottomRight;
        
        // assigning tiles for proc gen.
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
                            RotateOuterCorner(tilePosition, row, col); // outside corner
                            break;
                        case 2:
                            tilemap.SetTile(tilePosition, outsideWall);
                            RotateOuterWall(tilePosition, row, col); // outside wall
                            break;
                        case 3:
                            tilemap.SetTile(tilePosition, insideCorner);
                            RotateInnerCorner(tilePosition, row, col); // inside corner
                            break;
                        case 4:
                            tilemap.SetTile(tilePosition, insideWall);
                            RotateInnerWall(tilePosition, row, col); // inside wall
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
        
        private void RotateOuterWall(Vector3Int tilePosition, int row, int col) {
            var adjacentTiles = GetAdjacentTiles(row, col);

            int leftTile = adjacentTiles["left"];
            int rightTile = adjacentTiles["right"];
            int topTile = adjacentTiles["top"];
            int bottomTile = adjacentTiles["bottom"];
            
            // OUTER WALL
            if ((IsOuterWall(leftTile) && IsOuterWall(rightTile)) && (!IsOuterWall(topTile) && !IsOuterWall(bottomTile))) {
                // Horizontal wall (default)
                tilemap.SetTile(tilePosition, outsideWall);
                tilemap.SetTransformMatrix(tilePosition, Matrix4x4.identity); // No rotation
            }
            else if ((IsOuterWall(topTile) && IsOuterWall(bottomTile)) && (!IsOuterWall(leftTile) && !IsOuterWall(rightTile))) {
                // Vertical wall (rotate 90 degrees)
                tilemap.SetTile(tilePosition, outsideWall);
                tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
            }
            
            // OUTER CORNER - default position is (make L with left hand, thumbs down & index right.)
            if (IsOuterCorner(1)) {
                if (leftTile == 2 && topTile == 2) {
                    // Top-left corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)));
                }
                else if (rightTile == 2 && topTile == 2) {
                    // Top-right corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
                }
                else if (rightTile == 2 && bottomTile == 2) {
                    // Bottom-right corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0)));
                }
                else if (leftTile == 2 && bottomTile == 2) {
                    // Bottom-left corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270)));
                }
            }
        }

        private void RotateOuterCorner(Vector3Int tilePosition, int row, int col) {
            var adjacentTiles = GetAdjacentTiles(row, col);

            int leftTile = adjacentTiles["left"];
            int rightTile = adjacentTiles["right"];
            int topTile = adjacentTiles["top"];
            int bottomTile = adjacentTiles["bottom"];
            
            // OUTER CORNER - default position is (make L with left hand, thumbs down & index right.)
            if (IsOuterCorner(1)) {
                if (leftTile == 2 && topTile == 2) {
                    // Top-left corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)));
                }
                else if (rightTile == 2 && topTile == 2) {
                    // Top-right corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
                }
                else if (rightTile == 2 && bottomTile == 2) {
                    // Bottom-right corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0)));
                }
                else if (leftTile == 2 && bottomTile == 2) {
                    // Bottom-left corner
                    tilemap.SetTile(tilePosition, outsideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270)));
                }
            }
        }
        
        private void RotateInnerCorner(Vector3Int tilePosition, int row, int col) {
            var adjacentTiles = GetAdjacentTiles(row, col);

            int leftTile = adjacentTiles["left"];
            int rightTile = adjacentTiles["right"];
            int topTile = adjacentTiles["top"];
            int bottomTile = adjacentTiles["bottom"];
            
            if (IsInsideCornerOrWall(3)) {
                
                if (IsInsideCornerOrWall(topTile) && IsInsideCornerOrWall(rightTile)) {
                    // Override: Rotate 270 if there is a wall on top and right
                    tilemap.SetTile(tilePosition, insideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270)));
                } 
                else if (IsInsideCornerOrWall(bottomTile) && IsInsideCornerOrWall(leftTile)) {
                    // Override: Rotate 180 if there is a wall on bottom and right
                    tilemap.SetTile(tilePosition, insideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
                } 
                else if (IsInsideCornerOrWall(bottomTile) && IsInsideCornerOrWall(rightTile) && rightTile == 4) { // YEA NAH I GIVE UP. 
                    // Override: Rotate 180 if there is a wall on bottom and right
                    tilemap.SetTile(tilePosition, insideCorner);
                    tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)));
                } 
                else {
                    // Case 1: if left and top are not corners or walls, rotate 180
                    if (!IsInsideCornerOrWall(leftTile) && !IsInsideCornerOrWall(topTile)) {
                        tilemap.SetTile(tilePosition, insideCorner);
                        tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)));
                    }
                    // Case 2: if top and right are not corners or walls, rotate 90
                    else if (!IsInsideCornerOrWall(topTile) && !IsInsideCornerOrWall(rightTile)) {
                        tilemap.SetTile(tilePosition, insideCorner);
                        tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
                    }
                    // Case 3: if left and bottom are not corners or walls, rotate 270
                    else if (!IsInsideCornerOrWall(leftTile) && !IsInsideCornerOrWall(bottomTile)) {
                        tilemap.SetTile(tilePosition, insideCorner);
                        tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270)));
                    }
                    // Case 4: if right and bottom are not corners or walls, leave it as the default orientation (0 degrees)
                    else if (!IsInsideCornerOrWall(rightTile) && !IsInsideCornerOrWall(bottomTile)) {
                        tilemap.SetTile(tilePosition, insideCorner);
                        tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0)));
                    }
                }
            }
        }


        
        private void RotateInnerWall(Vector3Int tilePosition, int row, int col) {
            var adjacentTiles = GetAdjacentTiles(row, col);

            int leftTile = adjacentTiles["left"];
            int rightTile = adjacentTiles["right"];
            int topTile = adjacentTiles["top"];
            int bottomTile = adjacentTiles["bottom"];

            // Define pellet tile type (assuming 5 is a standard pellet and 6 is a power pellet)
            bool topHasPelletOrFloor = topTile == 5 || topTile == 6 || topTile == 0; 
            bool bottomHasPelletOrFloor = bottomTile == 5 || bottomTile == 6 || bottomTile == 0;
            bool leftHasPelletOrFloor = leftTile == 5 || leftTile == 6 || leftTile == 0;
            bool rightHasPelletOrFloor = rightTile == 5 || rightTile == 6 || rightTile == 0;

            // Conditions for wall orientation
            if (topHasPelletOrFloor || bottomHasPelletOrFloor) {
                // If top or bottom has pellets or floor, stay horizontal
                tilemap.SetTile(tilePosition, insideWall);
                tilemap.SetTransformMatrix(tilePosition, Matrix4x4.identity); // No rotation
            } 
            else if (leftHasPelletOrFloor || rightHasPelletOrFloor) {
                // If left or right has pellets or floor, rotate to vertical
                tilemap.SetTile(tilePosition, insideWall);
                tilemap.SetTransformMatrix(tilePosition, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
            } 
            else {
                // Default orientation (horizontal)
                tilemap.SetTile(tilePosition, insideWall);
                tilemap.SetTransformMatrix(tilePosition, Matrix4x4.identity); // No rotation
            }
        }
        
        
        private bool IsInsideWall(int tileType) {
            // Check if tile is a wall or valid connector (inner wall, inner corner, T-junction)
            return tileType == 4 || tileType == 3 || tileType == 7;
        }

        private bool IsInsideCornerOrWall(int tileType) {
            return tileType == 3 || tileType == 4;
        }
        
        private bool IsOuterWall(int tileType) {
            // Check if tile is a wall or valid connector (outside corner, outside wall, T-junction)
            return tileType == 1 || tileType == 2 || tileType == 7;
        }

        private bool IsOuterCorner(int tileType) {
            return tileType == 1;
        }
        
        private Dictionary<string, int> GetAdjacentTiles(int row, int col)
        {
            // Fetch adjacent tiles and return them in a dictionary
            return new Dictionary<string, int>
            {
                { "left", col > 0 ? _levelMap[row, col - 1] : -1 },
                { "right", col < _levelMap.GetLength(1) - 1 ? _levelMap[row, col + 1] : -1 },
                { "top", row > 0 ? _levelMap[row - 1, col] : -1 },
                { "bottom", row < _levelMap.GetLength(0) - 1 ? _levelMap[row + 1, col] : -1 }
            };
        }
        
    }
}
