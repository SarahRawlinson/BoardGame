using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Dice
{
    public class WorkOutDieValue : MonoBehaviour
    {
        [SerializeField] float stationaryTime = 5f;
        [SerializeField] private float isStationaryMovement = .5f;
        [SerializeField] private Material normalColour;
        [SerializeField] private Material highlightColour;
        private float timeStationary = 0f;
        private Rigidbody rb;
        private void Start()
        {
            FindObjectOfType<RollAllDice>().DiceRollStarted += ResetStationaryTime;
            rb = GetComponent<Rigidbody>();
        }

        private void ResetStationaryTime()
        {
            timeStationary = 0f;
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
            return timeStationary > stationaryTime;
        }

        private void Update()
        {
            if (IsStationary())
            {
                timeStationary += Time.deltaTime;
            }
            else
            {
                timeStationary = 0f;
            }
        }

        public void ChangeColour(bool highlight)
        {
            Material colour;
            if (highlight)
            {
                colour = highlightColour;
            }
            else
            {
                colour = normalColour;
            }

            GetComponent<Renderer>().material = colour;
        }

        private bool IsStationary()
        {
            return Mathf.Abs(rb.velocity.x) < isStationaryMovement;
        }
        
    }

}
