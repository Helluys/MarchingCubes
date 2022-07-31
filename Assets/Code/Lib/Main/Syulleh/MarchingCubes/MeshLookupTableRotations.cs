using System;

namespace Syulleh.MarchingCubes {
	public static partial class MeshLookupTable {
		/// <summary>
		/// Maps a vertex index when rotating to the right by 90° (positive Y rotation).
		/// </summary>
		/// <param name="index">the vertex index</param>
		/// <returns>the rotated vertex index</returns>
		/// <exception cref="ArgumentException">The given vertex index out of the [1; 8] range</exception>
		private static int RotateVertexY (int index) => index switch {
			1 => 2,
			2 => 6,
			3 => 7,
			4 => 3,
			5 => 1,
			6 => 5,
			7 => 8,
			8 => 4,
			_ => throw new ArgumentException("Illegal vertex index " + index),
		};

		/// <summary>
		/// Maps a vertex index when rotating to the top by 90° (positive X rotation).
		/// </summary>
		/// <param name="index">the vertex index</param>
		/// <returns>the rotated vertex index</returns>
		/// <exception cref="ArgumentException">The given vertex index out of the [1; 8] range</exception>
		private static int RotateVertexX (int index) => index switch {
			1 => 4,
			2 => 3,
			3 => 7,
			4 => 8,
			5 => 1,
			6 => 2,
			7 => 6,
			8 => 5,
			_ => throw new ArgumentException("Illegal vertex index " + index),
		};

		/// <summary>
		/// Maps a vertex index when rotating to the front by 90° (positive Z rotation).
		/// </summary>
		/// <param name="index">the vertex index</param>
		/// <returns>the rotated vertex index</returns>
		/// <exception cref="ArgumentException">The given vertex index out of the [1; 8] range</exception>
		private static int RotateVertexZ (int index) => index switch {
			1 => 2,
			2 => 3,
			3 => 4,
			4 => 1,
			5 => 6,
			6 => 7,
			7 => 8,
			8 => 5,
			_ => throw new ArgumentException("Illegal vertex index " + index),
		};

		/// <summary>
		/// Maps an edge index when rotating to the right by 90° (positive Y rotation).
		/// </summary>
		/// <param name="index">the edge index</param>
		/// <returns>the rotated edge index</returns>
		/// <exception cref="ArgumentException">The given edge index out of the [1; 12] range</exception>
		private static int RotateEdgeY (int index) => index switch {
			1 => 10,
			2 => 6,
			3 => 12,
			4 => 2,
			5 => 9,
			6 => 8,
			7 => 11,
			8 => 4,
			9 => 1,
			10 => 5,
			11 => 3,
			12 => 7,
			_ => throw new ArgumentException("Illegal edge index " + index),
		};

		/// <summary>
		/// Maps an edge index when rotating to the top by 90° (positive X rotation).
		/// </summary>
		/// <param name="index">the edge index</param>
		/// <returns>the rotated edge index</returns>
		/// <exception cref="ArgumentException">The given edge index out of the [1; 12] range</exception>
		private static int RotateEdgeX (int index) => index switch {
			1 => 3,
			2 => 12,
			3 => 7,
			4 => 11,
			5 => 1,
			6 => 10,
			7 => 5,
			8 => 9,
			9 => 4,
			10 => 2,
			11 => 8,
			12 => 6,
			_ => throw new ArgumentException("Illegal edge index " + index),
		};

		/// <summary>
		/// Maps an edge index when rotating to the front by 90° (positive Z rotation).
		/// </summary>
		/// <param name="index">the edge index</param>
		/// <returns>the rotated edge index</returns>
		/// <exception cref="ArgumentException">The given edge index out of the [1; 12] range</exception>
		private static int RotateEdgeZ (int index) => index switch {
			1 => 2,
			2 => 3,
			3 => 4,
			4 => 1,
			5 => 6,
			6 => 7,
			7 => 8,
			8 => 5,
			9 => 10,
			10 => 12,
			11 => 9,
			12 => 11,
			_ => throw new ArgumentException("Illegal edge index " + index),
		};
	}
}