using UnityEngine;

namespace Script.PacMovement {
    public class PacStudentController : MonoBehaviour {

        public Transform pacStudent;
        public float moveSpeed = 1f;
        public Animator pacAnimator;
        public AudioSource moveAudio;
        public Transform mapParent;
        private Tweener _tweener;
        private Vector3 _currentGridPosition;
        private KeyCode? _lastInput = null;
        private KeyCode? _currentInput = null;

        readonly int[,] _levelMap = 
        { 
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
            _currentGridPosition = pacStudent.localPosition;
            Debug.Log($"Starting position: {_currentGridPosition}");
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
            Vector3 targetGridPosition = _currentGridPosition;

            switch (input.Value) {
                case KeyCode.W: targetGridPosition += Vector3.up;
                    break;
                case KeyCode.A: targetGridPosition += Vector3.left;
                    break;
                case KeyCode.S: targetGridPosition += Vector3.down;
                    break;
                case KeyCode.D: targetGridPosition += Vector3.right;
                    break;
            }
            
            if (IsWalkable(targetGridPosition)) {
                _currentInput = input;
                MoveToGridPosition(targetGridPosition);
            } else if (_currentInput.HasValue) {
                TryToMove(_currentInput);
            }
        }

        private bool IsWalkable(Vector3 position) {
            Vector3 worldPos = mapParent.TransformPoint(position);
            int x = Mathf.RoundToInt(worldPos.x);
            int y = Mathf.RoundToInt(worldPos.y);

            return x >= 0 && x < _levelMap.GetLength(0) && y >= 0 && y < _levelMap.GetLength(1) &&
                   (_levelMap[x, y] == 5 || _levelMap[x, y] == 6);
        }
        
        private void MoveToGridPosition(Vector3 targetGridPosition) {
            Vector3 startPos = pacStudent.localPosition;
            Vector3 endPos = mapParent.TransformPoint(new Vector3(targetGridPosition.x, targetGridPosition.y, pacStudent.localPosition.z));

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
