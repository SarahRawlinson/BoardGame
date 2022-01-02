using System;
using BoardGame.Board;
using BoardGame.Dice;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame.Player
{
    public class PlayerDirector : MonoBehaviour
    {
        enum GameState
        {
            PlayerMove,
            PlayerWait,
            PlayerRoll
        }

        public event Action<int> PlayerMove;
        private GameState _currentState = GameState.PlayerWait;
        [SerializeField] private Button rollButton;
        private bool _isMoving;
        [SerializeField] private GameObject player;
        private int _playerPosition;

        private void Start()
        {
            FindObjectOfType<WorkOutDiceValue>().DiceRolled += PlayerMoves;
            _playerPosition = FindObjectOfType<RandomPositions>().GetPosition(_playerPosition);
            Vector3 newPos = FindObjectOfType<RandomPositions>().NextPosition(_playerPosition);
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case GameState.PlayerMove:
                    MovePlayer();
                    break;
                case GameState.PlayerRoll:
                    RollPlayer();
                    break;
                case GameState.PlayerWait:
                    WaitForPlayer();
                    break;
                default:
                    Debug.Log("Unknown State Has Activated");
                    break;
            }
        }

        void PlayerMoves(int move)
        {
            _playerPosition += move;
            _playerPosition = FindObjectOfType<RandomPositions>().GetPosition(_playerPosition);
            Vector3 newPos = FindObjectOfType<RandomPositions>().NextPosition(_playerPosition);

            player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
            _isMoving = false;
            PlayerMove?.Invoke(_playerPosition + 1);
        }

        private void WaitForPlayer()
        {
            rollButton.enabled = true;
            FindObjectOfType<RollAllDice>().DiceRollStarted += DiceRollStarted;
            FindObjectOfType<RollAllDice>().DiceRolled += DiceRollComplete;
            _currentState = GameState.PlayerRoll;
        }

        private void DiceRollStarted()
        {
            FindObjectOfType<RollAllDice>().DiceRolled -= DiceRollStarted;
            rollButton.enabled = false;
        }

        private void DiceRollComplete()
        {
            FindObjectOfType<RollAllDice>().DiceRolled -= DiceRollComplete;
            _currentState = GameState.PlayerMove;
        }

        private void RollPlayer()
        {

        }

        void MovePlayer()
        {
            if (!_isMoving) _currentState = GameState.PlayerWait;
        }
    }
}
