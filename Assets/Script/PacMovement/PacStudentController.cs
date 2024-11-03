using UnityEngine;

namespace Script.PacMovement {
    public class PacStudentController : MonoBehaviour {

        public Transform pacStudent;
        public float moveSpeed = 1f;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        public ParticleSystem bloodFX;
        
        private Tweener _tweener;
        private Vector3 _currentGridPosition;
        private KeyCode? _lastInput = null;
        private KeyCode? _currentInput = null;
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start() {
            _tweener = GetComponent<Tweener>();
            _currentGridPosition = pacStudent.position;
        }
        
        private void Update() {
            GetMovementOnKeyDown();
            
            if (!_tweener.TweenExists(pacStudent)) {
                pacAnimator.SetFloat(MoveX, 0);
                pacAnimator.SetFloat(MoveY, 0);
                StopMovementAudio();
                StopDustEffect();

                if (TryToMove(_lastInput)) {
                    _currentInput = _lastInput;
                    PlayMovementAudio();  
                    PlayDustEffect();
                } 
                else if (_currentInput.HasValue) {
                    if (TryToMove(_currentInput)) {
                        PlayMovementAudio();
                        PlayDustEffect();
                    }
                }
            } 
            else {
                if (!moveAudio.isPlaying) {
                    PlayMovementAudio();
                }
            }
        }

        private void GetMovementOnKeyDown() {
            if (Input.GetKeyDown(KeyCode.W)) _lastInput = KeyCode.W;
            if (Input.GetKeyDown(KeyCode.A)) _lastInput = KeyCode.A;
            if (Input.GetKeyDown(KeyCode.S)) _lastInput = KeyCode.S;
            if (Input.GetKeyDown(KeyCode.D)) _lastInput = KeyCode.D;
        }

        private bool TryToMove(KeyCode? input) {
            if (!input.HasValue) return false;
            Vector3 targetGridPosition = _currentGridPosition;
            Vector3 direction = Vector3.zero;

            switch (input.Value) {
                case KeyCode.W: direction += Vector3.up; break;
                case KeyCode.A: direction += Vector3.left; break;
                case KeyCode.S: direction += Vector3.down; break;
                case KeyCode.D: direction += Vector3.right; break;
            }

            targetGridPosition += direction;

            if (IsWalkable(direction)) {
                MoveToGridPosition(targetGridPosition);
                return true;
            }

            return false;
        }

        private bool IsWalkable(Vector3 direction) {
            RaycastHit2D hit = Physics2D.Raycast(_currentGridPosition, direction, 1f, LayerMask.GetMask("Wall"));
            return !hit.collider;
        }
        
        private void MoveToGridPosition(Vector3 targetGridPosition) {
            Vector3 startPos = pacStudent.position;
            Vector3 endPos = targetGridPosition;

            if (_tweener.AddTween(pacStudent, startPos, endPos, moveSpeed)) {
                Vector3 direction = (startPos - endPos).normalized;
                UpdateAnimator(direction);
                PlayMovementAudio();
            }
            _currentGridPosition = targetGridPosition;
        }
        
        private void UpdateAnimator(Vector3 direction) {

            float moveX = direction.x;
            float moveY = direction.y;
            
            pacAnimator.SetFloat(MoveX, moveX);
            pacAnimator.SetFloat(MoveY, moveY);
        }
        
        private void PlayMovementAudio() {
            if (!moveAudio.isPlaying) {
                moveAudio.Play();
            }
        }
        
        public void StopMovementAudio() {
            if (moveAudio.isPlaying) {
                moveAudio.Stop();
            }
        }
        
        private void PlayDustEffect() {
            if (!bloodFX.isPlaying) {
                bloodFX.Play(); // Play the dust particle effect
            }
        }

        private void StopDustEffect() {
            if (bloodFX.isPlaying) {
                bloodFX.Stop(); // Stop the dust particle effect
            }
        }

    }
}
