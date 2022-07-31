using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Syulleh.MarchingCubes {
	public class MeshLookupTableTest {

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
			List<int> vertices = new();
			if (v1)
				vertices.Add(1);
			if (v2)
				vertices.Add(2);
			if (v3)
				vertices.Add(3);
			if (v4)
				vertices.Add(4);
			if (v5)
				vertices.Add(5);
			if (v6)
				vertices.Add(6);
			if (v7)
				vertices.Add(7);
			if (v8)
				vertices.Add(8);

			Assert.AreEqual(1, MeshLookupTable.allCubes.Where(c => c.PopulatedVertices.SequenceEqual(vertices)).Count());
		}
	}
}
