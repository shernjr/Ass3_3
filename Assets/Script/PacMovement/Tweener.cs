using System.Collections.Generic;
using UnityEngine;

namespace Script.PacMovement {
    public class Tweener : MonoBehaviour {
    
        private List<Tween> _activeTweens = new List<Tween>();
    
        void Update() {
            TweenPosition();
        }
    
        public bool TweenExists(Transform target) {
            if (target == null) {
                return false;
            }
            foreach (Tween tw in _activeTweens) {
                if (tw.Target == target) {
                    return true;
                }
            }
            return false;
        }
    
        public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration) {
            if (TweenExists(targetObject)) {
                return false;
            }
            var newTweenObject = new Tween(targetObject, startPos, endPos, Time.time, duration);
            _activeTweens.Add(newTweenObject);
            return true;
        }

        private void TweenPosition() {
            for (var i = _activeTweens.Count - 1; i >= 0; i--) {
                var tween = _activeTweens[i];
            
                var dis = Vector3.Distance(tween.Target.position, tween.EndPos);
                if (dis > 0.1f) {
                    var fraction = (Time.time - tween.StartTime) / tween.Duration;
                    var f = fraction; //Mathf.Pow(fraction, 3)

                    tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, f * Time.fixedDeltaTime);
                }
                else {
                    tween.Target.position = tween.EndPos;
                    _activeTweens.RemoveAt(i);

                    var pacStuMove = tween.Target.GetComponent<PacStudentController>();
                    if (pacStuMove != null) {
                        pacStuMove.StopMovementAudio();
                    }
                }
            }
        }
    }
}
