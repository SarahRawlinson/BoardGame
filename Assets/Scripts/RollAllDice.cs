using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Dice
{
    public class RollAllDice : MonoBehaviour
    {
        private DiceRoll[] dice;

        private void Start()
        {
            dice = FindObjectsOfType<DiceRoll>();
        }

        public void RollDice()
        {
            foreach (DiceRoll die in dice)
            {
                die.RollDice();
            }
        }
    }
}
