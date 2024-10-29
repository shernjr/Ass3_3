using UnityEngine;

namespace Script.PacMovement {
    public class TestMovement : MonoBehaviour {
    
        public Transform pacStudent;
        public float moveSpeed = 1f;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        public Transform mapParent;
        private Tweener _tweener;
        private Vector3 _currentGridPosition;
        private KeyCode? _lastInput = null;
        private KeyCode? _currentInput = null;
        private float _originalZ; // Store original Z position

        private void Start() {
            _tweener = GetComponent<Tweener>();
            _currentGridPosition = pacStudent.localPosition;
            _originalZ = _currentGridPosition.z; // Set original Z position
        }

        private void Update() {
            GetMovementOnKeyDown();
            if (!_tweener.TweenExists(pacStudent)) {
                TryToMove(_lastInput);
            }
        }

        private void GetMovementOnKeyDown() {
            if (Input.GetKeyDown(KeyCode.W)) {
                _lastInput = KeyCode.W;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                _lastInput = KeyCode.A;
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                _lastInput = KeyCode.S;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                _lastInput = KeyCode.D;
            }
        }

        private void TryToMove(KeyCode? input) {
            if (!input.HasValue) return;
            Vector3 targetPosition = _currentGridPosition;
            
            switch (input.Value) {
                case KeyCode.W: targetPosition += Vector3.up; break;
                case KeyCode.A: targetPosition += Vector3.left; break;
                case KeyCode.S: targetPosition += Vector3.down; break;
                case KeyCode.D: targetPosition += Vector3.right; break;
            }
            
            if (IsWalkable(targetPosition)) {
                _currentInput = input;
                MoveToPosition(targetPosition);
            } else if (_currentInput.HasValue) {
                TryToMove(_currentInput);
            }
        }

        private bool IsWalkable(Vector3 position) {
            return true;
        }

        private void MoveToPosition(Vector3 targetPosition) {
            Vector3 startPos = pacStudent.localPosition;
            Vector3 endPos = new Vector3(targetPosition.x, targetPosition.y, _originalZ); // Use original Z position

            if (_tweener.AddTween(pacStudent, startPos, endPos, moveSpeed)) {
                Vector3 direction = (endPos - startPos).normalized;
                UpdateAnimator(direction);
                //PlayMovementAudio();
            }
            _currentGridPosition = targetPosition;
        }

        private void UpdateAnimator(Vector3 direction) {
            pacAnimator.SetFloat("moveX", direction.x);
            pacAnimator.SetFloat("moveY", direction.y);
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
