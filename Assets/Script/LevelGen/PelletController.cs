using UnityEngine;

namespace Script.LevelGen {
    public class PelletController : MonoBehaviour { 
        // set direction that each pellet can see. i.e. can only see neighbouring pellet, use for pacstudent movement. WIP, unlikely to be used.
        
        public bool canMoveUp;
        public bool canMoveDown;
        public bool canMoveLeft;
        public bool canMoveRight;

        public GameObject pelletLeft;
        public GameObject pelletRight;
        public GameObject pelletDown;
        public GameObject pelletUp;
        
        private void Start() {
            Physics2D.queriesStartInColliders = false;
            LightHitRight();
            LightHitUp();
            LightHitLeft();
            LightHitDown();
        }

        private void LightHitDown() {
            var lightHitDown = Physics2D.RaycastAll(transform.position, Vector2.down);

            for (int i = 0; i < lightHitDown.Length; i++) {
                float dis = Mathf.Abs(lightHitDown[i].point.y - transform.position.y);
                if (dis < 0.4f) {
                    canMoveDown = true;
                    pelletDown = lightHitDown[i].collider.gameObject;
                }
            }
        }
        
        private void LightHitUp() {
            var lightHitUp = Physics2D.RaycastAll(transform.position, Vector2.up);

            for (int i = 0; i < lightHitUp.Length; i++) {
                float dis = Mathf.Abs(lightHitUp[i].point.y - transform.position.y);
                if (dis < 0.4f) {
                    canMoveUp = true;
                    pelletUp = lightHitUp[i].collider.gameObject;
                }
            }
        }
        
        private void LightHitRight() {
            var lightHitRight = Physics2D.RaycastAll(transform.position, Vector2.right);

            for (int i = 0; i < lightHitRight.Length; i++) {
                float dis = Mathf.Abs(lightHitRight[i].point.y - transform.position.y);
                if (dis < 0.4f) {
                    canMoveRight = true;
                    pelletRight = lightHitRight[i].collider.gameObject;
                }
            }
        }
        private void LightHitLeft() {
            var lightHitLeft = Physics2D.RaycastAll(transform.position, Vector2.left);

            for (int i = 0; i < lightHitLeft.Length; i++) {
                float dis = Mathf.Abs(lightHitLeft[i].point.y - transform.position.y);
                if (dis < 0.4f) {
                    canMoveLeft = true;
                    pelletLeft = lightHitLeft[i].collider.gameObject;
                }
            }
        }
    }
}
