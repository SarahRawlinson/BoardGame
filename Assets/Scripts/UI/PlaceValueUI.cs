using BoardGame.Player;
using TMPro;
using UnityEngine;

namespace BoardGame.UI
{
    public class PlaceValueUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text placeText;

        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<PlayerDirector>().PlayerMove += SetPlayerText;
        }

        private void SetPlayerText(int obj)
        {
            placeText.text = obj.ToString();
        }
    }
}
