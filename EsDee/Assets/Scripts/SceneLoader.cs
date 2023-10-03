using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

namespace EsDee
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        string nextSceneName;
        [SerializeField]
        float delay = 2.0f;

        void Awake()
        {
            Assert.IsFalse(string.IsNullOrEmpty(nextSceneName));
            Assert.IsTrue(delay >= 0.0f);
        }

        public void Load()
        {
            _ = StartCoroutine(LoadNextScene());
        }

        IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(delay);
            yield return SceneManager.LoadSceneAsync(nextSceneName);
        }
    }
}
