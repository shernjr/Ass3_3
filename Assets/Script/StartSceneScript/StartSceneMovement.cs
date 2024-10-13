using Script.PacMovement;
using UnityEngine;

namespace Script.StartSceneScript {
    public class StartSceneMovement : MonoBehaviour {

        public Transform pacStudent;
        public Transform ghost;
        public float moveSpeed = 5f;
        public float ghostOffset = 1f; 
        public Animator pacAnimator;
        public Animator ghostAnimator;
        private Vector3[] _positions;
        private int _currentTargetIndex;
        private bool _ghostChaseStarted = false;

        private Tweener _tweener;
        
        void Start() {
            _tweener = GetComponent<Tweener>(); 
            
            StartPosition();
            MovePacStudent();
        }

        private void StartPosition() {
            _positions = new Vector3[] {
                new Vector3(-7f, 5f, 0), // start position
                new Vector3(-7f, -5f, 0),
                new Vector3(7f, -5f, 0), 
                new Vector3(7f, 5f, 0), 
            };
            pacStudent.position = _positions[1];
            ghost.position = pacStudent.position + new Vector3(-ghostOffset, 0, 0); // Ghost starts trailing behind
            _currentTargetIndex = 2;
        }

        private void MovePacStudent() {
            Vector3 targetPos = _positions[_currentTargetIndex];

            if (_tweener.AddTween(pacStudent, pacStudent.position, targetPos, moveSpeed)) {
                Vector3 direction = (targetPos - pacStudent.position).normalized;
                UpdatePacAnimator(direction);
            }
            
            if (!_ghostChaseStarted) {
                Invoke("MoveGhost", 0.1f); 
                _ghostChaseStarted = true;
            }
        }

        private void MoveGhost() {
            Vector3 ghostTargetPos = pacStudent.position - new Vector3(ghostOffset, 0, 0);

            if (_tweener.AddTween(ghost, ghost.position, ghostTargetPos, moveSpeed)) {
                Vector3 direction = (ghostTargetPos - ghost.position).normalized;
                UpdateGhostAnimator(direction);
            }
        }

        private void UpdatePacAnimator(Vector3 direction) {
            float moveX = direction.x;
            float moveY = direction.y;
            pacAnimator.SetFloat("moveX", moveX);
            pacAnimator.SetFloat("moveY", moveY);
        }
        
        private void UpdateGhostAnimator(Vector3 direction) {
            float moveX = direction.x;
            float moveY = direction.y;
            ghostAnimator.SetFloat("moveX", moveX);
            ghostAnimator.SetFloat("moveY", moveY);
        }

        private void Update() {
            if (!_tweener.TweenExists(pacStudent)) {
                _currentTargetIndex = (_currentTargetIndex + 1) % _positions.Length;
                MovePacStudent();
            }

            if (!_tweener.TweenExists(ghost)) {
                MoveGhost();
            }
        }
    }
}
