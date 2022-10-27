using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Syulleh.MarchingCubes {
	public class MeshLookupTableTest {
		/// <summary>
		/// Maps vertex index pair to edge index.
		/// </summary>
		private static readonly IDictionary<(int, int), int> vertToEdge = new Dictionary<(int, int), int>() {
			{(1,2), 1},
			{(2,3), 2},
			{(3,4), 3},
			{(1,4), 4},
			{(5,6), 5},
			{(6,7), 6},
			{(7,8), 7},
			{(5,8), 8},
			{(1,5), 9},
			{(2,6), 10},
			{(4,8), 11},
			{(3,7), 12},
		};

		/// <summary>
		/// Asserts that the vertex presence combination is present exactly once in <see cref="MeshLookupTable.allCubes"/>.
		/// </summary>
		/// <param name="v1">vertex 1 presence</param>
		/// <param name="v2">vertex 2 presence</param>
		/// <param name="v3">vertex 3 presence</param>
		/// <param name="v4">vertex 4 presence</param>
		/// <param name="v5">vertex 5 presence</param>
		/// <param name="v6">vertex 6 presence</param>
		/// <param name="v7">vertex 7 presence</param>
		/// <param name="v8">vertex 8 presence</param>
		[Test, Combinatorial]
		public void AllCubes (
			[Values(false, true)] bool v1,
			[Values(false, true)] bool v2,
			[Values(false, true)] bool v3,
			[Values(false, true)] bool v4,
			[Values(false, true)] bool v5,
			[Values(false, true)] bool v6,
			[Values(false, true)] bool v7,
			[Values(false, true)] bool v8) {
			CubeMesh cubeMesh = MeshLookupTable.configurations[(v1, v2, v3, v4, v5, v6, v7, v8)];

			Assert.IsNotNull(cubeMesh);
			AssertEdgePresence(cubeMesh);
		}

		/// <summary>
		/// Asserts edge presence is consistent with vertice presence.
		/// </summary>
		/// <param name="cubeMesh">the cube mesh under test</param>
		private static void AssertEdgePresence (CubeMesh cubeMesh) {
			for (int i1 = 1; i1 <= 7; i1++) {
				for (int i2 = i1 + 1; i2 <= 8; i2++) {
					if (vertToEdge.ContainsKey((i1, i2))) {
						bool intersection = cubeMesh.PopulatedVertices.Contains(i1) != cubeMesh.PopulatedVertices.Contains(i2);
						Assert.AreEqual(intersection, cubeMesh.PopulatedEdges.Contains(vertToEdge[(i1, i2)]));
					}
				}
			}
		}
	}
}
