using UnityEngine;

namespace BoardGame.Dice
{
    public class DieValue : MonoBehaviour
    {
        [SerializeField] private int diceValue;

        // Start is called before the first frame update
        public int GetDiceValue()
        {
            return diceValue;
        }
    }
}
