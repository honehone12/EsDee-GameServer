using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    [System.Serializable]
    public class CharaPrefab
    {
        public CharaCode charaCode = CharaCode.NotSelected;
        public GameObject prefab;
    }


    [CreateAssetMenu(fileName = "CharaPrefabList", menuName = "CharaPrefabList")]
    public class CharaPrefabList : ScriptableObject
    {
        [SerializeField]
        List<CharaPrefab> prefabList = new();

        public CharaPrefab Find(CharaCode charaCode, out bool ok)
        {
            Assert.IsFalse(charaCode != CharaCode.NotSelected);

            var found = prefabList.Find((cp) => cp.charaCode == charaCode);
            if (found == null)
            {
                ok = false;
                return null;
            }

            ok = true;
            return found;
        }
    }
}


