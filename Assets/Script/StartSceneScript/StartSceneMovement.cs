using Script.PacMovement;
using UnityEngine;

namespace Script.StartSceneScript {
    public class StartSceneMovement : MonoBehaviour {
    
        public Transform pacStudent;
        public Transform ghost;
        public float moveSpeed = 5f;
        public float ghostOffset = 1f; 
        public Animator pacAnimator; // Reference to the Animator
        private Vector3[] _positions;
        private int _currentTargetIndex;

        private Tweener _tweener;
        
        void Start()
        {
            _tweener = GetComponent<Tweener>(); 
            
            StartPosition();
            MoveToNextPos();
        
        }
    
        private void StartPosition() { // ISSUE : Ghost will move diagonally. need to add check to prevent movement before the other finishes.
            _positions = new Vector3[] {
                new Vector3(-7f, 5f, 0), // start
                new Vector3(-7f, -5f, 0),
                new Vector3(7f, -5f, 0), 
                new Vector3(7f, 5f, 0), 
            };
            pacStudent.position = _positions[1];
            ghost.position = _positions[0];
            _currentTargetIndex = 1;
        }

        private void MoveToNextPos() {
            Vector3 targetPos = _positions[_currentTargetIndex];

            if (_tweener.AddTween(pacStudent, pacStudent.position, targetPos, moveSpeed)) {
                Vector3 direction = (targetPos - pacStudent.position).normalized;
                UpdateAnimator(direction);
            }
            
            
            //Vector3 ghostTargetPos = pacStudent.position - new Vector3(ghostOffset, 0, 0);

            if (_tweener.AddTween(ghost, ghost.position, targetPos, moveSpeed)) {
                Vector3 direction = (targetPos - ghost.position).normalized;
                UpdateAnimator(direction);
            }

            _currentTargetIndex = (_currentTargetIndex + 1) % _positions.Length;
        }
        
        private void UpdateAnimator(Vector3 direction) {
            float moveX = direction.x;
            float moveY = direction.y;
            pacAnimator.SetFloat("moveX", moveX);
            pacAnimator.SetFloat("moveY", moveY);
        }
        
        private void Update() {
            if (!_tweener.TweenExists(pacStudent)) {
                MoveToNextPos();
            }
        }
    }
}


