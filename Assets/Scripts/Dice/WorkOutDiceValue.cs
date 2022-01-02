using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoardGame.Dice
{
    public class WorkOutDiceValue: MonoBehaviour
    {
        public event Action<int> DiceRolled; 
        private WorkOutDieValue[] _dice;
        private bool _rollComplete = true;
        private bool _shouldRoll;
        private void Start()
        {
            _dice = FindObjectsOfType<WorkOutDieValue>();
        }

        private void Update()
        {
            if (_shouldRoll)
            {
                _shouldRoll = !HasRolled();
            }
        }

        public void Roll()
        {
            _shouldRoll = true;
        }

        private bool HasRolled()
        {
            GetDiceValue();
            return _rollComplete;
        }

        private void GetDiceValue()
        {
            _rollComplete = true;
            int values = 0;
            List<int> diceValues = new List<int>();
            foreach (WorkOutDieValue die in _dice)
            {
                diceValues.Add(die.GetDiceValue());
                if (!die.IsStationaryForTime()) _rollComplete = false;
                values += die.GetDiceValue();
                die.ChangeColour(false);
            }

            if (_rollComplete)
            {
                DiceRolled?.Invoke(values);
                IEnumerable<int> duplicates = diceValues.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(x => x.Key);

                //Debug.Log("Duplicate elements are: " + String.Join(",", duplicates));
                List<int> duplicateList = duplicates.ToList();
                if (duplicateList.Any())
                {
                    foreach (WorkOutDieValue die in _dice)
                    {
                        die.ChangeColour(duplicateList.ToList().Contains(die.GetDiceValue()));
                    }
                }
            }
        }
    }
}