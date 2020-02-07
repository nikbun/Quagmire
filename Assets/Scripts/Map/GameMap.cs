﻿using Map.MapObjects;
using System.Collections.Generic;

namespace Map
{
	public class GameMap
	{
		private Circle circle;
		private Origin origin;
		private Home home;
		private Jopa jopa;
		private TolchokDistributor tolchek;

		/// <summary>
		/// Игровая карта
		/// Загружает локации и позволяет пработать с ними
		/// </summary>
		public GameMap()
		{
			circle = new Circle();
			origin = new Origin(circle);
			home = new Home(circle);
			jopa = new Jopa(origin);
			tolchek = new TolchokDistributor(circle);
		}

		public ICell GetOrigin(MapSides mapSide)
		{
			return origin.GetCell(mapSide);
		}

		public ICell GetJopa(MapSides mapSide)
		{
			return jopa.GetCell(mapSide);
		}

		/// <summary>
		/// Получает следующую клетку в толчке по трекеру
		/// </summary>
		/// <param name="tracker"></param>
		/// <returns></returns>
		public ICell GetNextTolchok(Tracker tracker)
		{
			return tolchek.GetTolchek(tracker);
		}

		/// <summary>
		/// Определяет может ли пешка ходить на заданное количество шагов
		/// </summary>
		/// <param name="tracker"></param>
		/// <param name="steps"></param>
		/// <returns></returns>
		public bool CanMove(Tracker tracker, int steps)
		{
			switch (tracker.location)
			{
				case MapLocations.Origin:
					return origin.CanMove(tracker, steps);
				case MapLocations.Circle:
					return circle.CanMove(tracker, steps);
				case MapLocations.Home:
					return home.CanMove(tracker, steps);
				case MapLocations.Jopa:
					return jopa.CanMove(tracker, steps);
				case MapLocations.Tolchok:
					return tolchek.CanMove(tracker, steps);
				default:
					return false;
			}
		}

		public ICell GetNextCell(ICell cell, MapSides side, bool inCircle = false) 
		{
			switch (cell.location)
			{
				case MapLocations.Origin:
					return origin.GetNextCell(side);
				case MapLocations.Cut:
				case MapLocations.Circle:
					return circle.GetNextCell(cell, side, inCircle);
				case MapLocations.Home:
					return home.GetNextCell(cell, side);
				case MapLocations.Jopa:
					return jopa.GetNextCell(side);
				case MapLocations.Tolchok:
					return tolchek.GetNextCell(cell, side);
				default:
					return null;
			}
		}

		public ICell GetPreviousCell(ICell cell, MapSides side)
		{
			switch (cell.location)
			{
				case MapLocations.Circle:
					return circle.GetPreviousCell(cell, side);
				case MapLocations.Home:
					return home.GetPreviousCell(cell, side);
				default:
					return null;
			}
		}

		/// <summary>
		/// Получить дополнительные клетки для специальных локаций
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public List<ICell> GetExtra(ICell cell) 
		{
			switch (cell.location)
			{
				case MapLocations.Cut:
					return ((CellCut)cell).GetExtra();
				case MapLocations.Tolchok:
					return tolchek.GetExtra(cell);
				default:
					return new List<ICell>();
			}
		}
	}

	/// <summary>
	/// Расположения на карте
	/// </summary>
	public enum MapLocations
	{
		Origin,
		Circle,
		Home,
		Jopa,
		Tolchok,
		Cut
	}

	/// <summary>
	/// Стороны карты
	/// </summary>
	public enum MapSides
	{
		Bottom,
		Left,
		Top,
		Right
	}
}