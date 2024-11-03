using Script.PacMovement;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.LevelGen {
    public class CherryController : MonoBehaviour {

        public GameObject cherryPrefab;
        private Tweener _tweener;

        // Timer of 10 seconds to spawn.
        public float spawnInterval = 10f;
        private float _spawnTimer;
        private float _camWidth;
        private float _camHeight;
        private Vector3 _levelCenter = new Vector3(0, 0, 0);

        private void Start() {
            _tweener = GetComponent<Tweener>();
            Camera mainCamera = Camera.main;
            if (mainCamera != null) {
                _camHeight = 2f * mainCamera.orthographicSize;
                _camWidth = _camHeight * mainCamera.aspect;
            } else {
                Debug.LogError("Main camera not found! Please ensure it is tagged 'MainCamera'.");
            }
        }

        private void Update() {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= spawnInterval) {
                SpawnCherry();
                _spawnTimer = 0f;
            }
            /*GameObject[] cherries = GameObject.FindGameObjectsWithTag("Cherry");
            if (cherries.Length > 0) {
                foreach (GameObject cherry in cherries) {
                    float distanceFromCenter = Vector3.Distance(cherry.transform.position, _levelCenter);
                    if (cherry != null && distanceFromCenter > 25) {
                        Destroy(cherry);
                    }
                }
            }*/
        }

        // Spawn anywhere outside camera.
        private void SpawnCherry() {
            Vector3 spawnPos = RandomSpawnPos();
            GameObject cherrySpawn = Instantiate(cherryPrefab, spawnPos, quaternion.identity);
            MoveCherry(cherrySpawn);
        }

        private Vector3 RandomSpawnPos() {
            float offset = 1f; // Offset to spawn outside view

            // Randomly select side and position
            int side = Random.Range(0, 4); // 0: top, 1: bottom, 2: left, 3: right
            switch (side) {
                case 0: return new Vector3(Random.Range(-_camWidth / 2, _camWidth / 2), _camHeight / 2 + offset, 0); // Top
                case 1: return new Vector3(Random.Range(-_camWidth / 2, _camWidth / 2), -_camHeight / 2 - offset, 0); // Bottom
                case 2: return new Vector3(-_camWidth / 2 - offset, Random.Range(-_camHeight / 2, _camHeight / 2), 0); // Left
                default: return new Vector3(_camWidth / 2 + offset, Random.Range(-_camHeight / 2, _camHeight / 2), 0); // Right
            }
        }
        
        // Move towards middle point and keep going.
        // Use linear lerping addTween.
        private void MoveCherry(GameObject cherry) {
            Vector3 direction = (_levelCenter - cherry.transform.position).normalized;
            Vector3 endPos = _levelCenter + direction * (Mathf.Max(_camWidth, _camHeight) * 1.2f); 
            float moveDuration = 5f; 
            _tweener.AddTween(cherry.transform, cherry.transform.position, endPos, moveDuration);
        }
    }
}
