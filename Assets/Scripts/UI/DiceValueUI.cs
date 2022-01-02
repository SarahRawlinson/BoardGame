using UnityEngine;
using BoardGame.Dice;
using TMPro;

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
