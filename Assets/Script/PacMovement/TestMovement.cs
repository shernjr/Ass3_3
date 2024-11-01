using UnityEngine;

namespace Script.PacMovement {
    public class TestMovement : MonoBehaviour {
    
        public Transform pacStudent;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        public Transform topLeftQuadrant;
        public Transform topRightQuadrant;
        public Transform bottomLeftQuadrant;
        public Transform bottomRightQuadrant;

        public float moveSpeed = 1f;
        private Tweener _tweener;
        private Vector2Int _currentGridPos = new Vector2Int(1, 1);
        private Vector2Int _lastInput = Vector2Int.zero;
        private Vector2Int _currentInput = Vector2Int.zero;

        private static readonly Vector3 PacStudentStartOffset = new Vector3(1f, 1f, 0);
        private static readonly Vector3 MapFirstTileOffset = new Vector3(1f,1f, 0);

        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        readonly int[,] _levelMap = { 
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
            _tweener = GetComponent<Tweener>();
            _currentGridPos = new Vector2Int(1, 1);  // Initial grid pos
            MoveToWorldPosition(_currentGridPos);
        }

        private void Update() {
            if (!_tweener.TweenExists(pacStudent)) {
                Vector2Int direction = GetDirectionFromInput();
                if (direction != Vector2Int.zero) {
                    _lastInput = direction; // Store last input
                }

                if (AttemptMove(_lastInput)) {
                    _currentInput = _lastInput;
                }
                // If lastInput move was blocked, try currentInput
                else if (AttemptMove(_currentInput)) {
                    // currentInput is maintained
                }
            }
        }

        private Vector2Int GetDirectionFromInput() {
            if (Input.GetKey(KeyCode.W)) return Vector2Int.up;
            if (Input.GetKey(KeyCode.A)) return Vector2Int.left;
            if (Input.GetKey(KeyCode.S)) return Vector2Int.down;
            if (Input.GetKey(KeyCode.D)) return Vector2Int.right;
            return Vector2Int.zero;
        }
        
        private bool AttemptMove(Vector2Int direction) {
            if (direction == Vector2Int.zero) return false;

            Vector2Int targetPos = _currentGridPos + direction;
            if (IsWalkable(targetPos)) {
                _currentGridPos = targetPos;
                MoveToWorldPosition(targetPos);
                UpdateAnimator(direction);
                return true;
            }
            return false;
        }

        private bool IsWalkable(Vector2Int gridPos) {
            int x = gridPos.x, y = gridPos.y;
            return x >= 0 && x < _levelMap.GetLength(1) && y >= 0 && y < _levelMap.GetLength(0) &&
                   (_levelMap[y, x] == 5 || _levelMap[y, x] == 6); // Only walkable on 5 or 6
        }

        private void MoveToWorldPosition(Vector2Int gridPos) {
            Vector3 worldPosition = GetWorldPosition(gridPos);
            _tweener.AddTween(pacStudent, pacStudent.position, worldPosition, moveSpeed);
            PlayMovementAudio();
        }
        private Vector3 GetWorldPosition(Vector2Int gridPos) {
            Transform quadrant = GetQuadrantTransform(gridPos);
            Vector3 localPosition = new Vector3(gridPos.x, -gridPos.y, 0);
            return quadrant.TransformPoint(localPosition) + PacStudentStartOffset - MapFirstTileOffset;
        }

        private Transform GetQuadrantTransform(Vector2Int gridPos) {
            bool isTopLeft = gridPos.y < _levelMap.GetLength(0) / 2 && gridPos.x < _levelMap.GetLength(1) / 2;
            bool isTopRight = gridPos.y < _levelMap.GetLength(0) / 2 && gridPos.x >= _levelMap.GetLength(1) / 2;
            bool isBottomLeft = gridPos.y >= _levelMap.GetLength(0) / 2 && gridPos.x < _levelMap.GetLength(1) / 2;

            if (isTopLeft) return topLeftQuadrant;
            if (isTopRight) return topRightQuadrant;
            if (isBottomLeft) return bottomLeftQuadrant;
            return bottomRightQuadrant;
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
