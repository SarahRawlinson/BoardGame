using System;
using System.Collections;
using System.Collections.Generic;
using BoardGame.Dice;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    enum GameState {PlayerMove, PlayerWait, PlayerRoll}

    private GameState currentState = GameState.PlayerWait;
    [SerializeField] private Button rollButton;
    private bool isMoving = false;
    [SerializeField] private GameObject player;
    private int playerPosition = 0;

    private void Start()
    {
        FindObjectOfType<WorkOutDiceValue>().DiceRolled += PlayerMoves;
    }

    private void Update()
    {
        switch (currentState)
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
                break;
        } 
    }

    void PlayerMoves(int move)
    {
        playerPosition += move;
        playerPosition = FindObjectOfType<RandomPositions>().GetPosition(playerPosition);
        Vector3 newPos = FindObjectOfType<RandomPositions>().NextPosition(playerPosition);
        
        player.transform.position = new Vector3(newPos.x, player.transform.position.y, newPos.z);
        isMoving = false;
    }

    private void WaitForPlayer()
    {
        rollButton.enabled = true;
        FindObjectOfType<RollAllDice>().DiceRollStarted += DiceRollStarted;
        FindObjectOfType<RollAllDice>().DiceRolled += DiceRollComplete;
        currentState = GameState.PlayerRoll;
    }

    private void DiceRollStarted()
    {
        FindObjectOfType<RollAllDice>().DiceRolled -= DiceRollStarted;
        rollButton.enabled = false;
    }

    private void DiceRollComplete()
    {
        FindObjectOfType<RollAllDice>().DiceRolled -= DiceRollComplete;
        currentState = GameState.PlayerMove;
    }

    private void RollPlayer()
    {
        
    }

    void MovePlayer()
    {
        if (!isMoving) currentState = GameState.PlayerWait;
    }
}
