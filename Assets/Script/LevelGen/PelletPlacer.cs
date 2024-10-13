using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletPlacer : MonoBehaviour { // super dodgy pellet placement hard coded. Confused about map placement which is offset, so pellets also need to be offset?

    [SerializeField] private GameObject pellet;
    [SerializeField] private GameObject powerPellet;
    
    [SerializeField] private Transform topLeftQuadrant;
    [SerializeField] private Transform topRightQuadrant;
    [SerializeField] private Transform bottomLeftQuadrant;
     [SerializeField] private Transform bottomRightQuadrant;
    
    int[,] levelMap =
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

    private void Start() {
        PlacePellets(topLeftQuadrant, new Vector3(-13.5f, 7.5f, 0), false, false);
        PlacePellets(topRightQuadrant, new Vector3(13.5f, 7.5f, 0), true, false);
        PlacePellets(bottomLeftQuadrant, new Vector3(-13.5f, -7.5f, 0), false, true);
        PlacePellets(bottomRightQuadrant, new Vector3(13.5f, -7.5f, 0), true, true);
    }

    void PlacePellets(Transform quadrant, Vector3 startingOffset, bool flipX, bool flipY) { // super dodgy.
        for (int y = 0; y < levelMap.GetLength(0); y++)
        {
            for (int x = 0; x < levelMap.GetLength(1); x++)
            {
                Vector3 position = new Vector3(x, 6f - y, 0); // badly hardcoded, no time to focus 100% on the assessment, cannot figure out map and pellet placement without taking too much time. Need to move on.
                position = ApplyQuadrantTransform(position, startingOffset, flipX, flipY); // 

                if (levelMap[y, x] == 5) {
                    Instantiate(pellet, position, Quaternion.identity, quadrant);
                }
                else if (levelMap[y, x] == 6) {
                    position.y -= 0.5f;
                    Instantiate(powerPellet, position, Quaternion.identity, quadrant);
                }
            }
        }
    }
    
    Vector3 ApplyQuadrantTransform(Vector3 originalPosition, Vector3 quadrantPosition, bool flipX, bool flipY) {
        Vector3 newPosition = originalPosition;

        // Apply mirroring based on flip flags
        if (flipX) {
            newPosition.x = -newPosition.x;
        }
        if (flipY) {
            newPosition.y = -newPosition.y;
        }

        // Offset by the quadrant's position
        newPosition += quadrantPosition;

        return newPosition;
    }
}
