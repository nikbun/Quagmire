﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Map;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public float gameSpeed = 10; // Скорость игры
	public GameStates gameState = GameStates.RollDice; // Состояние игры
	[Range(1, 4)]
	public int countPlayers = 4; // Количество игроков
	public int currentPlayer = 0;
	public static GameController instance = null; // Статичный экземпляр контроллера игры
	public List<GameObject> instancePlayers; // Экземпляры игроков
	public Dictionary<PlayerPosition, Player> players = new Dictionary<PlayerPosition, Player>(); // Скрипты игроков для управления игроками
	public GameMap gameMap = new GameMap(); // Игровая карта

#if UNITY_EDITOR
	public int DebugDiceNumber = 0;
#endif

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	void Start() //Выставение фишек на начальные позиции 
	{
		InitPlayers();
	}

	/// <summary>
	/// Загружаем игроков на стартовые позиции
	/// </summary>
	void InitPlayers()
	{
		for (int i = 0; i < countPlayers; i++)
		{
			var playerPos = (PlayerPosition)i;
			var player = Instantiate(instancePlayers[i], gameMap.GetOriginPosition(playerPos), Quaternion.identity);
			var sPlayer = player.GetComponent<Player>();
			sPlayer.playerPosition = playerPos;
			players.Add(playerPos, sPlayer);
		}
	}

	/// <summary>
	/// Выбрать следующего игрока
	/// </summary>
	public int NextTurn()
	{
		currentPlayer = (++currentPlayer)%countPlayers;
		gameState = GameStates.RollDice;
		return currentPlayer;
	}

	public void CanMovePawns(int steps)
	{
		bool canMove = players[(PlayerPosition)currentPlayer].CanMovePawns(steps);
		if (canMove)
			gameState = GameStates.WalkPawn;
		else
			NextTurn();
	}

	public void EndTurn()
	{
		foreach (var key in players.Keys)
		{
			if (!players[key].IsPawnsEndMove())
				return;
		}
		if (gameState == GameStates.WalkPawn)
			NextTurn();
	}
}

public enum GameStates
{
	RollDice,
	WalkPawn
}