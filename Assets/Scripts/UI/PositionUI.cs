using TMPro;
using UnityEngine;

namespace BoardGame.UI
{
    public class PositionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text positionText;

        public void SetLabel(string text)
        {
            positionText.text = text;
        }
    }
}
