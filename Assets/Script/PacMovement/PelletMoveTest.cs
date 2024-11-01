using UnityEngine;

namespace Script.PacMovement {
    public class PelletMoveTest : MonoBehaviour {

        public Transform pacStudent;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        public float moveSpeed = 0.5f;
        private Tweener _tweener;
        private Vector2Int _lastInput = Vector2Int.zero;
        private Vector2Int _currentInput = Vector2Int.zero;
        
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private void Start() {
            _tweener = GetComponent<Tweener>();
        }
        
        private void Update() {
            if (!_tweener.TweenExists(pacStudent)) {
                Vector2Int direction = GetDirectionFromInput();
                if (direction != Vector2Int.zero) {
                    _lastInput = direction;
                }

                if (AttemptMoveToNextPellet(_lastInput)) {
                    _currentInput = _lastInput;
                } else if (AttemptMoveToNextPellet(_currentInput)) {
                }
            }
        }
        
        private Vector2Int GetDirectionFromInput() {
            if (Input.GetKeyDown(KeyCode.W)) return Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.A)) return Vector2Int.left;
            if (Input.GetKeyDown(KeyCode.S)) return Vector2Int.down;
            if (Input.GetKeyDown(KeyCode.D)) return Vector2Int.right;
            return Vector2Int.zero;
        }
        
        private bool AttemptMoveToNextPellet(Vector2Int direction) {
            if (direction == Vector2Int.zero) return false;

            // Find the next pellet in the target direction
            Transform nextPellet = FindPelletInDirection(direction);
            if (nextPellet != null) {
                MoveToPellet(nextPellet);
                UpdateAnimator(direction);
                return true;
            }
            return false;
        }

        private Transform FindPelletInDirection(Vector2Int direction) { // almost there, just need to make it not able to see further than one pellet ahead. 
            GameObject[] pellets = GameObject.FindGameObjectsWithTag("Pellet");
            Transform closestPellet = null;
            float closestDistance = Mathf.Infinity;

            Vector2 targetPosition = (Vector2)pacStudent.position + new Vector2(direction.x, direction.y);

            foreach (GameObject pellet in pellets) {
                Vector2 pelletPosition = pellet.transform.position;

                // Check if the pellet is directly in line with the PacStudent's movement direction
                if (pelletPosition == targetPosition) {
                    // If found, set as closest pellet
                    return pellet.transform;
                }
            }

            return closestPellet;
        }

        private void MoveToPellet(Transform pellet) {
            _tweener.AddTween(pacStudent, pacStudent.position, pellet.position, moveSpeed * Time.deltaTime);
            PlayMovementAudio();
        }

        private void UpdateAnimator(Vector2Int direction) {
            pacAnimator.SetFloat(MoveX, direction.x);
            pacAnimator.SetFloat(MoveY, direction.y);
        }

        private void PlayMovementAudio() {
            if (!moveAudio.isPlaying) {
                moveAudio.loop = true;
                moveAudio.Play();
            }
        }

        public void StopMovementAudio() {
            if (moveAudio.isPlaying) {
                moveAudio.Stop();
            }
        }
        
    }
}
