﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapObjects
{
	/// <summary>
	/// Абстрактный метод для локаций, в которых пешки наклыдываються друг на друга
	/// </summary>
	abstract class AStack
	{
		public Dictionary<PlayerPosition, ICell> cells = new Dictionary<PlayerPosition, ICell>();

		/// <summary>
		/// Проверка на возможность двигаться
		/// Устанавливает трасировку на пешку в случае возможности движения
		/// </summary>
		/// <param name="pawn">Передвигаемая пешка</param>
		/// <param name="steps">Количество шагов</param>
		/// <returns></returns>
		public bool CanMove(MapPawn pawn, int steps)
		{
			var trace = new Trace(from:pawn.trace?.from);
			if (steps != 6)
				return false;
			bool canMove;
			trace.UpdateTrace(GetTarget(pawn, out canMove));
			pawn.SetTrace(canMove, canMove ? trace : null);
			return canMove;
		}
		/// <summary>
		/// Получает позицию в зависимости от расположения игрока(игроки снизу, слева, сверху, справа)
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		public Vector3 GetPosition(PlayerPosition playerPosition)
		{
			return GetCell(playerPosition).GetWay()[0];
		}

		/// <summary>
		/// Получает клетку в зависимости от расположения игрока
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		public ICell GetCell(PlayerPosition playerPosition)
		{
			return cells[playerPosition];
		}

		/// <summary>
		/// Получает клетку выхода из стековой локации 
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="canOccupy"></param>
		/// <returns></returns>
		public abstract ICell GetTarget(MapPawn pawn, out bool canOccupy);
	}
}