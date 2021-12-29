
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private Vector3 diceStartPosition;

    private void Start()
    {
        diceStartPosition = transform.position;
        GetComponent<Rigidbody>().isKinematic = false;
        RollDice();
    }

    public void RollDice()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = diceStartPosition;
        transform.rotation =  Random.rotation;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-200f,200f),0f,Random.Range(-200f,200f)), ForceMode.Impulse);
        
    }
    
}
