using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Syulleh.MarchingCubes {
	/// <summary>
	/// Represents a unit cube with its mesh data in the Marching Cube algorithm.
	/// See <see href="https://academy.cba.mit.edu/classes/scanning_printing/MarchingCubes.pdf"/> figures 3 and 4 for how indices are defined.
	/// </summary>
	public class CubeMesh : IEquatable<CubeMesh> {

		/// <summary>
		/// The populated vertice indices.
		/// </summary>
		public readonly int[] PopulatedVertices;

		/// <summary>
		/// The populated edge indices.
		/// </summary>
		public readonly int[] PopulatedEdges;

		/// <summary>
		/// The triangles, referring to <see cref="PopulatedEdges"/> values (not array index).
		/// </summary>
		public readonly (int x, int y, int z)[] Triangles;

		/// <summary>
		/// Constructs an instance.
		/// Takes ownership of provided arrays.
		/// </summary>
		/// <param name="populatedVertices">the populated vertices</param>
		/// <param name="populatedEdges">the populated edges</param>
		/// <param name="triangles">the triangles</param>
		public CubeMesh (int[] populatedVertices, int[] populatedEdges, (int x, int y, int z)[] triangles) {
			PopulatedVertices = populatedVertices;
			PopulatedEdges = populatedEdges;
			Triangles = triangles;
		}

		#region mapping
		/// <summary>
		/// Returns a <see cref="CubeMesh"/> with mapped vertices and edges.
		/// </summary>
		/// <param name="mapVertex"the verex mapping function</param>
		/// <param name="mapEdge">the edge mapping function</param>
		/// <returns>a <see cref="CubeMesh"/> with each <see cref="PopulatedEdges"/> mapped using <paramref name="mapEdge"/></returns>
		public CubeMesh Map (Func<int, int> mapVertex, Func<int, int> mapEdge) {
			(int x, int y, int z) mapTri ((int x, int y, int z) v) => new(mapEdge(v.x), mapEdge(v.y), mapEdge(v.z));
			return new CubeMesh(PopulatedVertices.Select(mapVertex).OrderBy(i => i).ToArray(),
								PopulatedEdges.Select(mapEdge).OrderBy(i => i).ToArray(),
								Triangles.Select(mapTri).ToArray());
		}

		/// <summary>
		/// Returns a <see cref="CubeMesh"/> with reversed triangles of this cube mesh. The <see cref="PopulatedEdges"/> are identical.
		/// </summary>
		/// <returns>a <see cref="CubeMesh"/> with reversed triangles of this cube mesh</returns>
		public CubeMesh Reverse () {
			static (int x, int y, int z) reverseTri ((int x, int y, int z) v) => new(v.x, v.z, v.y);
			return new CubeMesh(Enumerable.Range(1, 8).Except(PopulatedVertices).ToArray(),
								PopulatedEdges,
								Triangles.Select(reverseTri).ToArray());
		}
		#endregion mapping

		#region equatable
		public override bool Equals (object obj) => Equals(obj as CubeMesh);
		public bool Equals (CubeMesh other) => other is not null && PopulatedVertices.SequenceEqual(other.PopulatedVertices);
		public override int GetHashCode () => ((IStructuralEquatable) PopulatedVertices).GetHashCode(EqualityComparer<int>.Default);

		public static bool operator == (CubeMesh left, CubeMesh right) => EqualityComparer<CubeMesh>.Default.Equals(left, right);
		public static bool operator != (CubeMesh left, CubeMesh right) => !(left == right);
		#endregion equatable
	}
}