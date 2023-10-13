using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace EsDee
{
    public class SkillSelectUI : MonoBehaviour
    {
        enum RL : byte
        {
            R,
            L
        }

        static SkillSelectUI Instance;

        public static bool IsSkillSelectScene => Instance != null;

        [SerializeField]
        SceneLoader sceneLoader;

        void Awake()
        {
            Assert.IsNotNull(sceneLoader);
            
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            OnSkillSelected(SkillCode.CubeBigBullet, RL.R);
            OnSkillSelected(SkillCode.CubeBigBullet, RL.L);
        }

        void OnDestroy()
        {
            Assert.IsTrue(Instance == this);
            Instance = null;
        }

        public void OnToggleRCube(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CubeBigBullet, RL.R);
            }
        }

        public void OnToggleRSphere(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.SphereBigBullet, RL.R);
            }
        }

        public void OnToggleRCapsule(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CapsuleBigBullet, RL.R);
            }
        }

        public void OnToggleRCylinder(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CylinderBigBullet, RL.R);
            }
        }

        public void OnToggleLCube(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CubeBigBullet, RL.L);
            }
        }

        public void OnToggleLSphere(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.SphereBigBullet, RL.L);
            }
        }

        public void OnToggleLCapsule(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CapsuleBigBullet, RL.L);
            }
        }

        public void OnToggleLCylinder(bool flag)
        {
            if (flag)
            {
                OnSkillSelected(SkillCode.CylinderBigBullet, RL.L);
            }
        }

        public void OnOKButton()
        {
            sceneLoader.Load();
        }

        void OnSkillSelected(SkillCode code, RL rl)
        {
            switch (rl)
            {
                case RL.R:
                    UserProfile.Singleton.SkillMouseR = code;
                    break;
                case RL.L:
                    UserProfile.Singleton.SkillMouseL = code;
                    break;
                default:
                    return;
            }
        }
    }
}


