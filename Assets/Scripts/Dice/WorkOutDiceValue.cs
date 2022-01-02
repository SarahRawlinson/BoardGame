using System;
using System.Collections.Generic;
using System.Linq;
using BoardGame.Dice;
using UnityEngine;

namespace BoardGame.Dice
{
    public class WorkOutDiceValue: MonoBehaviour
    {
        public event Action<int> DiceRolled; 
        private WorkOutDieValue[] dice;
        private bool rollComplete = true;
        private bool ShouldRoll = false;
        private void Start()
        {
            dice = FindObjectsOfType<WorkOutDieValue>();
        }

        private void Update()
        {
            if (ShouldRoll)
            {
                ShouldRoll = !HasRolled();
            }
        }

        public void Roll()
        {
            ShouldRoll = true;
        }

        public bool HasRolled()
        {
            GetDiceValue();
            return rollComplete;
        }
        public void GetDiceValue()
        {
            rollComplete = true;
            int values = 0;
            List<int> diceValues = new List<int>();
            foreach (WorkOutDieValue die in dice)
            {
                diceValues.Add(die.GetDiceValue());
                if (!die.IsStationaryForTime()) rollComplete = false;
                values += die.GetDiceValue();
                die.ChangeColour(false);
            }

            if (rollComplete)
            {
                DiceRolled?.Invoke(values);
                IEnumerable<int> duplicates = diceValues.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(x => x.Key);

                //Debug.Log("Duplicate elements are: " + String.Join(",", duplicates));
                if (duplicates.Count() > 0)
                {
                    foreach (WorkOutDieValue die in dice)
                    {
                        if (duplicates.ToList().Contains(die.GetDiceValue()))
                        {
                            die.ChangeColour(true);
                        }
                        else
                        {
                            die.ChangeColour(false);
                        }
                    }
                }
            }
        }
    }
}