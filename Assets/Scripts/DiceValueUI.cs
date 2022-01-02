using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BoardGame.Dice;
using TMPro;
using UnityEngine.UI;

namespace BoardGame.UI
{
    public class DiceValueUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text diceValueTextMesh;
        private WorkOutDiceValue _workOutDiceValue;


        private void Start()
        {
            _workOutDiceValue = FindObjectOfType<WorkOutDiceValue>();
            _workOutDiceValue.DiceRolled += UpdateText;
        }

        void UpdateText(int value)
        {
            diceValueTextMesh.text = value.ToString();
        }
    }
}
