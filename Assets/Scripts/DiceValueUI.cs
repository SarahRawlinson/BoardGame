using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoardGame.Dice;
using TMPro;
using UnityEngine.UI;

namespace BoardGame.UI
{
    public class DiceValueUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text diceValueTextMesh;

        private WorkOutDiceValue[] dice;

        private void Start()
        {
            dice = FindObjectsOfType<WorkOutDiceValue>();
        }

        // Update is called once per frame
        void Update()
        {
            bool rollComplete = true;
            int values = 0;
            foreach (WorkOutDiceValue die in dice)
            {
                if (!die.IsStationaryForTime()) rollComplete = false;
                 values += die.GetDiceValue();
            }

            if (rollComplete) diceValueTextMesh.text = values.ToString();
            else diceValueTextMesh.text = "-";

        }
    }
}
