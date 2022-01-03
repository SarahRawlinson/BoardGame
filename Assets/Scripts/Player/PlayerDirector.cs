using System;
using System.Collections;
using System.Collections.Generic;
using BoardGame.Board;
using BoardGame.Dice;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public event Action<string> PositionAction;
        private GameState _currentState = GameState.PlayerWait;
        [SerializeField] private Button rollButton;
        private bool _isMoving;
        [SerializeField] private GameObject player;
        private int _playerPosition;

        private void Start()
        {
            FindObjectOfType<WorkOutDiceValue>().DiceRolled += StartPlayerMove;
            _playerPosition = FindObjectOfType<RandomPositions>().GetPosition(_playerPosition);
            Vector3 newPos = FindObjectOfType<RandomPositions>().NextPosition(_playerPosition);
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
        }

        private void StartPlayerMove(int move)
        {
            StartCoroutine(PlayerMoves(move));
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

        IEnumerator PlayerMoves(int move)
        {
            yield return new WaitForSeconds(5);
            _playerPosition += move;
            _playerPosition = FindObjectOfType<RandomPositions>().GetPosition(_playerPosition);
            Vector3 newPos = FindObjectOfType<RandomPositions>().NextPosition(_playerPosition);
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
            
            PlayerMove?.Invoke(_playerPosition + 1);
            foreach (PositionSpace position in FindObjectsOfType<PositionSpace>())
            {
                if (Vector3.Distance(position.transform.position, newPos) < 0.01)
                {

                    int i;
                    PositionSpace.PositionActions action;
                    (i, action) = position.GetAction();

                    SetQty(action, i);
                    
                }
            }
            
        }

        private void SetQty(PositionSpace.PositionActions action, int i)
        {
            switch (action)
            {
                case PositionSpace.PositionActions.GoForward:
                    PositionAction?.Invoke($"Move Forward {i.ToString()} spaces");
                    StartCoroutine(PlayerMoves(i));
                    break;
                case PositionSpace.PositionActions.GoBackwards:
                    PositionAction?.Invoke($"Move Backwards {i.ToString()} spaces");
                    StartCoroutine(PlayerMoves(-i));
                    break;
                case PositionSpace.PositionActions.MissTurn:
                    PositionAction?.Invoke($"Miss {i.ToString()} Turns");
                    _isMoving = false;
                    break;
                case PositionSpace.PositionActions.None:
                    PositionAction?.Invoke($"No Action This Turn");
                    _isMoving = false;
                    break;
                case PositionSpace.PositionActions.Start:
                    PositionAction?.Invoke($"Player Reached Start");
                    _isMoving = false;
                    break;
                case PositionSpace.PositionActions.End:
                    PositionAction?.Invoke($"Player Reached End");
                    _isMoving = false;
                    break;
                default:
                    break;
            }
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
