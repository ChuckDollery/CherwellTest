using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cherwell.Controllers
{
	/// <summary>
	/// Controller to process the triangle
	/// </summary>
	[Produces("application/json")]
	[Route("api/Triangle")]
	public class TriangleController : Controller
	{
		/// <summary>
		/// Gets all coordinates for the triangles
		/// </summary>
		/// <returns></returns>
		// GET: api/Triangle
		[HttpGet]
		public IActionResult Get()
		{
			var coordinates = Models.Triangle.GetAllCoordinates();
			var onlyCoordinates = coordinates.Select(child => child.Item3).ToList();

			return Ok(onlyCoordinates);
		}

		/// <summary>
		/// Gets a single coordinate based on the <paramref name="row"/> and <paramref name="row"/> supplied
		/// </summary>
		/// <param name="row">The row to fetch the coordinate for. <see cref="Models.Rows"/></param>
		/// <param name="column">The column to fetch the coordinate for, max value is <see cref="Models.Triangle.MaxColumn"/></param>
		/// <returns></returns>
		// GET: api/Triangle/A/1
		[HttpGet("{row}/{column}", Name = "Get")]
		public IActionResult Get(Models.Rows row, int column)
		{
			var triangle = new Models.Triangle(row, column);

			if (triangle.IsValid)
				return Ok(triangle.Coordinates);
			else
				return BadRequest("Validation error");
		}

		/// <summary>
		/// Gets the row <see cref="Models.Rows" /> and column /> for the triangle for the given coordinates
		/// </summary>
		/// <param name="V1X">X part for vertex 1</param>
		/// <param name="V1Y">Y part for vertex 1</param>
		/// <param name="V2X">X part for vertex 2</param>
		/// <param name="V2Y">Y part for vertex 2</param>
		/// <param name="V3X">X part for vertex 3</param>
		/// <param name="V3Y">Y part for vertex 3</param>
		/// <returns></returns>
		// GET: api/Triangle/A/1
		[HttpGet("{V1X}/{V1Y}/{V2X}/{V2Y}/{V3X}/{V3Y}", Name = "GetRowColumn")]
		public IActionResult Get(int V1X, int V1Y, int V2X, int V2Y, int V3X, int V3Y)
		{
			var triangle = new Models.Triangle(new Point(V1X, V1Y), new Point(V2X, V2Y), new Point(V3X, V3Y));

			if (triangle.IsValid)
				return Ok(new { Row = triangle.Row.ToString(), Column = triangle.Column });
			else
				return BadRequest("Validation error");
		}
	}
}
