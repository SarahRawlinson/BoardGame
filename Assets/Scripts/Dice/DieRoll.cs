
using UnityEngine;

namespace BoardGame.Dice
{
    public class DieRoll : MonoBehaviour
    {
        private Vector3 _diceStartPosition;

        private void Start()
        {
            _diceStartPosition = transform.position;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;
        }

        public void RollDice()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = true;
            transform.position = _diceStartPosition;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.rotation = Random.rotation;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-200f, 200f), 0f, Random.Range(-200f, 200f)),
                ForceMode.Impulse);

        }

    }
}
