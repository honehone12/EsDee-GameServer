using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

namespace EsDee
{
    public class MatchingUI : MonoBehaviour
    {
        [SerializeField]
        TMP_Text waitingText;
        [SerializeField]
        TMP_Text incrText;
        [SerializeField]
        TMP_Text doneText;

        void Awake()
        {
            Assert.IsNotNull(waitingText);
            Assert.IsNotNull(incrText);
            Assert.IsNotNull(doneText);
        }

        public void IncrementText(uint numTry)
        {
            incrText.text = numTry.ToString();
        }

        public void SwapTextAsDone()
        {
            waitingText.gameObject.SetActive(false);
            doneText.gameObject.SetActive(true);
        }
    }
}
