using UnityEngine;
using UnityEngine.SceneManagement;

    public class LevelLoader : MonoBehaviour
    {
        public void LoadMainScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadStartScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

