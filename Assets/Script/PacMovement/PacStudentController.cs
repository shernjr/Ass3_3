using UnityEngine;

namespace Script.PacMovement {
    public class PacStudentController : MonoBehaviour {

        public Transform pacStudent;
        public float moveSpeed = 0.5f;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        private Tweener _tweener;
        private Vector3 _currentGridPosition;
        private KeyCode? _lastInput = null;
        
        private void Start() {
            _tweener = GetComponent<Tweener>();
            _currentGridPosition = new Vector3(-12.5f, 13f, pacStudent.position.z);
        }
        
        private void Update() {
            GetMovementOnKeyDown();
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

            if (_lastInput.HasValue) {
                Vector3 targetGridPosition = _currentGridPosition;

                switch (_lastInput.Value) {
                    case KeyCode.W: targetGridPosition += Vector3.up;
                        break;
                    case KeyCode.A: targetGridPosition += Vector3.left;
                        break;
                    case KeyCode.S: targetGridPosition += Vector3.down;
                        break;
                    case KeyCode.D: targetGridPosition += Vector3.right;
                        break;
                }
                
                if (targetGridPosition != _currentGridPosition) {
                    MoveToGridPosition(targetGridPosition);
                }
            }
        }
        
        private void MoveToGridPosition(Vector3 targetGridPosition) {
            Vector3 startPos = pacStudent.position;
            Vector3 endPos = new Vector3(targetGridPosition.x, targetGridPosition.y, pacStudent.position.z);

            if (_tweener.AddTween(pacStudent, startPos, endPos, moveSpeed)) {
                Vector3 direction = (startPos - endPos).normalized;
                UpdateAnimator(direction);
                PlayMovementAudio();
            }

            _currentGridPosition = targetGridPosition;
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
