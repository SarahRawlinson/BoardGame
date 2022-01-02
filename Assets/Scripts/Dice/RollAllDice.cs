using System;
using UnityEngine;

namespace BoardGame.Dice
{
    public class RollAllDice : MonoBehaviour
    {
        private DieRoll[] _dice;
        public event Action DiceRolled;
        public event Action DiceRollStarted;

        private void Start()
        {
            _dice = FindObjectsOfType<DieRoll>();
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
            foreach (DieRoll die in _dice)
            {
                die.GetComponent<WorkOutDieValue>().ChangeColour(false);
                die.RollDice();
            }
            workOutDiceValue.Roll();
        }
    }
}
