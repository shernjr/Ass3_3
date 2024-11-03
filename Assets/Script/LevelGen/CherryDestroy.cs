using UnityEngine;

namespace Script.LevelGen {
    public class CherryDestroy : MonoBehaviour {
        private void OnBecameInvisible() {
            Destroy(gameObject);
        }
    }
}
