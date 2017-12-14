using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Cherwell.Models
{
	/// <summary>
	/// Enum to specify the rows. "None" is so the pipeline doesn't put "A" for an out of bound value.
	/// </summary>
	public enum Rows
	{
		None, A, B, C, D, E, F
	}

	/// <summary>
	/// Structure to store the three vertexes for the triangle
	/// </summary>
	public struct Coordinates
	{
		public Point V1 { get; private set; }
		public Point V2 { get; private set; }
		public Point V3 { get; private set; }

		public Coordinates(Point p1, Point p2, Point p3)
		{
			V1 = p1;
			V2 = p2;
			V3 = p3;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Coordinates))
				return false;

			if (obj == null)
				return false;

			return (this.V1 == ((Coordinates)obj).V1) &&
				   (this.V2 == ((Coordinates)obj).V2) &&
				   (this.V3 == ((Coordinates)obj).V3);
		}

		public override int GetHashCode()
		{
			return V1.GetHashCode() ^ V2.GetHashCode() ^ V3.GetHashCode();
		}

		public static bool operator ==(Coordinates coord1, Coordinates coord2)
		{
			return coord1.Equals(coord2);
		}

		public static bool operator !=(Coordinates coord1, Coordinates coord2)
		{
			return !coord1.Equals(coord2);
		}
	}

	/// <summary>
	/// Class to validate and calculate the vertexes and Row/Column
	/// </summary>
	public class Triangle
	{
		internal const int MaxColumn = 12;

		public Rows Row { get; private set; }
		public int Column { get; private set; }
		public Coordinates Coordinates { get; private set; }
		public bool IsValid { get; private set; }

		public Triangle(Rows row, int column)
		{
			Row = row;
			Column = column;

			IsValid = DetermineIsValid(row, column);

			if (IsValid)
				Coordinates = DetermineCoordinates(row, column);
		}

		public Triangle(Point p1, Point p2, Point p3)
		{
			Coordinates = new Coordinates(p1, p2, p3);
			IsValid = false;

			foreach(var coordinate in GetAllCoordinates())
			{
				if (Coordinates == coordinate.Item3)
				{
					IsValid = true;
					Row = coordinate.Item1;
					Column = coordinate.Item2;

					break;
				}
			}
		}

		/// <summary>
		/// Validates that row and column fall in the valid range
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private bool DetermineIsValid(Rows row, int column)
		{
			if ((row >= Rows.A) && (row <= Rows.F) && (column >= 1) && (column <= MaxColumn))
				return true;
			else
				return false;
		}

		private Coordinates DetermineCoordinates(Rows row, int column)
		{
			Point P1 = new Point();
			Point P2 = new Point();
			Point P3 = new Point();

			int rowIndex = ((int)row) - 1;
			int colIndex;
			bool isEvenColumn = false;

			if (column % 2 == 0)
			{
				isEvenColumn = true;
				colIndex = (column - 2) / 2;
			}
			else
			{
				colIndex = (column - 1) / 2;
			}

			if (isEvenColumn)
			{
				P1.X = (0 + 10 * colIndex);
				P1.Y = (0 + 10 * rowIndex);

				P2.X = (10 + 10 * colIndex);
				P2.Y = (0 + 10 * rowIndex);

				P3.X = (10 + 10 * colIndex);
				P3.Y = (10 + 10 * rowIndex);
			}
			else
			{
				P1.X = (0 + 10 * colIndex);
				P1.Y = (10 + 10 * rowIndex);

				P2.X = (0 + 10 * colIndex);
				P2.Y = (0 + 10 * rowIndex);

				P3.X = (10 + 10 * colIndex);
				P3.Y = (10 + 10 * rowIndex);
			}

			return new Coordinates(P1, P2, P3);
		}

		internal static List<Tuple<Rows, int, Coordinates>> GetAllCoordinates()
		{
			var coordinates = new List<Tuple<Rows, int, Coordinates>>();

			foreach (var row in Enum.GetValues(typeof(Rows)).Cast<Rows>())
			{
				if (row != Rows.None)
				{
					for (var column = 1; column <= Models.Triangle.MaxColumn; column++)
					{
						coordinates.Add(new Tuple<Rows, int, Coordinates>(row, column, new Models.Triangle(row, column).Coordinates));
					}
				}
			}

			return coordinates;
		}
	}
}
