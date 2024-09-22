//using System.Collections;

using UnityEngine;

namespace Script.PacMovement {
    public class PacStudentMovement : MonoBehaviour {
    
        public Transform pacStudent;
        public float moveSpeed = 5f;
        public AudioSource moveAudio;
        public Animator pacAnimator; // Reference to the Animator
        private Vector3[] _positions;
        private int _currentTargetIndex;

        private Tweener _tweener;
        
    
        // Start is called before the first frame update
        void Start()
        {
            _tweener = GetComponent<Tweener>();
            StartPosition();
            MoveToNextPos();
        
        }
    
        private void StartPosition() {
            _positions = new Vector3[] {
                new Vector3(-12.5f, 11.8f, 0), // top left
                new Vector3(-12.5f, 8f, 0),    // bottom left
                new Vector3(-7.5f, 8f, 0),     // bottom right
                new Vector3(-7.5f, 11.7f, 0),  // top right
            };
        }

        private void MoveToNextPos() {
            Vector3 targetPos = _positions[_currentTargetIndex];

            if (_tweener.AddTween(pacStudent, pacStudent.position, targetPos, moveSpeed)) {
                Vector3 direction = (targetPos - pacStudent.position).normalized;
                UpdateAnimator(direction);
                PlayMovementAudio();
            }

            _currentTargetIndex = (_currentTargetIndex + 1) % _positions.Length;
        }
    
        private void UpdateAnimator(Vector3 direction) {
            float moveX = direction.x;
            float moveY = direction.y;
            pacAnimator.SetFloat("moveX", moveX);
            pacAnimator.SetFloat("moveY", moveY);
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
    
        private void Update() {
            if (!_tweener.TweenExists(pacStudent)) {
                MoveToNextPos();
            }
        }
    }
}
