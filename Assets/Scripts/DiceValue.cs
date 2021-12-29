using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Dice
{
    public class DiceValue : MonoBehaviour
    {
        [SerializeField] private int diceValue;

        // Start is called before the first frame update
        public int GetDiceValue()
        {
            return diceValue;
        }
    }
}
