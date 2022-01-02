using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Dice
{
    public class RollAllDice : MonoBehaviour
    {
        private DieRoll[] dice;
        public event Action DiceRolled;
        public event Action DiceRollStarted;

        private void Start()
        {
            dice = FindObjectsOfType<DieRoll>();
            FindObjectOfType<WorkOutDiceValue>().DiceRolled += DiceRollFinished;
        }

        private void DiceRollFinished(int obj)
        {
            DiceRolled?.Invoke();
        }

        public void RollDice()
        {
            DiceRollStarted?.Invoke();
            WorkOutDiceValue workOutDiceValue = FindObjectOfType<WorkOutDiceValue>();
            foreach (DieRoll die in dice)
            {
                die.GetComponent<WorkOutDieValue>().ChangeColour(false);
                die.RollDice();
            }
            workOutDiceValue.Roll();
        }
    }
}
