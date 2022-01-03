using System.Collections;
using System.Collections.Generic;
using BoardGame.Player;
using TMPro;
using UnityEngine;

namespace BoardGame.UI
{
    public class ActionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text placeText;

        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<PlayerDirector>().PositionAction += SetPlayerText;
        }

        private void SetPlayerText(string obj)
        {
            placeText.text = obj;
        }
    }
}
