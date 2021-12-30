using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame.Dice
{
    public class WorkOutDiceValue : MonoBehaviour
    {
        [SerializeField] float stationaryTime = 5f;
        [SerializeField] private float isStationaryMovement = .5f;
        [SerializeField] private Material normalColour;
        [SerializeField] private Material highlightColour;
        private float timeStationary = 0f;
        private Rigidbody rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public int GetDiceValue()
        {
            
            DiceValue correctDiceValue = GetComponentInChildren<DiceValue>();
            foreach (DiceValue diceValue in GetComponentsInChildren<DiceValue>())
            {
                if (diceValue.transform.position.y > correctDiceValue.transform.position.y)
                {
                    correctDiceValue = diceValue;  
                }
            }
            return correctDiceValue.GetDiceValue();
            
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
