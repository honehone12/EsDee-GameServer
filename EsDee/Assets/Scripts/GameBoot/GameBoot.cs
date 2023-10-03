using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace EsDee
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField]
        GameObject persistentGameDataPrefab;
        [SerializeField]
        SceneLoader sceneLoader;
        

        void Awake()
        {
            Assert.IsNotNull(persistentGameDataPrefab);
            Assert.IsNotNull(sceneLoader);
        }

        void Start()
        {
            Instantiate(persistentGameDataPrefab);
            sceneLoader.Load();
        }
    }
}
