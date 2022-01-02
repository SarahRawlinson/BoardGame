using UnityEngine;

namespace BoardGame.Dice
{
    public class WorkOutDieValue : MonoBehaviour
    {
        [SerializeField] float stationaryTime = 5f;
        [SerializeField] private float isStationaryMovement = .5f;
        [SerializeField] private Material normalColour;
        [SerializeField] private Material highlightColour;
        private float _timeStationary;
        private Rigidbody _rb;
        private void Start()
        {
            FindObjectOfType<RollAllDice>().DiceRollStarted += ResetStationaryTime;
            _rb = GetComponent<Rigidbody>();
        }

        private void ResetStationaryTime()
        {
            _timeStationary = 0f;
        }

        public int GetDiceValue()
        {
            
            DieValue correctDieValue = GetComponentInChildren<DieValue>();
            foreach (DieValue diceValue in GetComponentsInChildren<DieValue>())
            {
                if (diceValue.transform.position.y > correctDieValue.transform.position.y)
                {
                    correctDieValue = diceValue;  
                }
            }
            return correctDieValue.GetDiceValue();
            
        }

        public bool IsStationaryForTime()
        {
            return _timeStationary > stationaryTime;
        }

        private void Update()
        {
            if (IsStationary())
            {
                _timeStationary += Time.deltaTime;
            }
            else
            {
                _timeStationary = 0f;
            }
        }

        public void ChangeColour(bool highlight)
        {
            Material colour = highlight ? highlightColour : normalColour;
            GetComponent<Renderer>().material = colour;
        }

        private bool IsStationary()
        {
            return Mathf.Abs(_rb.velocity.x) < isStationaryMovement;
        }
        
    }

}
