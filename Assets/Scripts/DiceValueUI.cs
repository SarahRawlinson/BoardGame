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
            List<int> diceValues = new List<int>();
            foreach (WorkOutDiceValue die in dice)
            {
                diceValues.Add(die.GetDiceValue());
                if (!die.IsStationaryForTime()) rollComplete = false;
                values += die.GetDiceValue();
                die.ChangeColour(false);
            }
            if (rollComplete)
            {
                diceValueTextMesh.text = values.ToString();
                IEnumerable<int> duplicates = diceValues.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(x => x.Key);
 
                //Debug.Log("Duplicate elements are: " + String.Join(",", duplicates));
                if (duplicates.Count() > 0)
                {
                    foreach (WorkOutDiceValue die in dice)
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
            else diceValueTextMesh.text = "-";

        }
    }
}
