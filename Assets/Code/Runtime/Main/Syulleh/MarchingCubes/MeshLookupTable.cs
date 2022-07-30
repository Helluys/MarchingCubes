using System.Collections.Generic;

using Vector3i = Syulleh.Math.Vector3<int>;
using Vector3f = Syulleh.Math.Vector3<float>;

namespace Syulleh.MarchingCubes {
	public class MeshLookupTable {
		public class CubeMesh {
			public readonly int[] PopulatedEdges;
			public readonly Vector3i[] Triangles;

			public CubeMesh (int[] populatedEdges, Vector3i[] triangles) {
				PopulatedEdges = populatedEdges;
				Triangles = triangles;
			}
		}

		public static readonly IReadOnlyDictionary<int, Vector3f> edgeToPos = new Dictionary<int, Vector3f>() {
			{1, new Vector3f(.5f, 0, 0)},
			{2, new Vector3f(1f, .5f, 0)},
			{3, new Vector3f(.5f, 1f, 0)},
			{4, new Vector3f(0, .5f, 0)},
			{5, new Vector3f(.5f, 0, 1f)},
			{6, new Vector3f(1f, .5f, 1f)},
			{7, new Vector3f(.5f, 1f, 1f)},
			{8, new Vector3f(0, .5f, 1f)},
			{9, new Vector3f(0, 0, .5f)},
			{10, new Vector3f(1, 0, .5f)},
			{11, new Vector3f(0, 1f, .5f)},
			{12, new Vector3f(1, 1f, .5f)},
		};

		/// <summary>
		/// All basic cube patterns as per <see href="https://academy.cba.mit.edu/classes/scanning_printing/MarchingCubes.pdf">MIT paper</see> figures 3 and 4.
		/// Edge index are those of the paper, including in the triangle structure.
		/// </summary>
		public static readonly List<CubeMesh> baseCubes = new() {
			new CubeMesh(new int[]{}, new Vector3i[]{}),					// 0
			new CubeMesh(new int[]{1, 4, 9},								// 1
				new Vector3i[]{new Vector3i(1, 4, 9)}),
			new CubeMesh(new int[]{2, 4, 9, 10},							// 2
				new Vector3i[]{new Vector3i(2, 4, 9), new Vector3i(2, 9, 10)}),
			new CubeMesh(new int[]{1, 2, 3, 4, 9, 12},						// 3
				new Vector3i[]{new Vector3i(1, 4, 9), new Vector3i(2, 12, 3)}),
			new CubeMesh(new int[]{1, 4, 6, 7, 9, 12},						// 4
				new Vector3i[]{new Vector3i(1, 4, 9), new Vector3i(6, 7, 12)}),
			new CubeMesh(new int[]{1, 2, 6, 8, 9},							// 5
				new Vector3i[]{new Vector3i(1, 9, 2), new Vector3i(2, 9, 8), new Vector3i(2, 8, 6)}),
			new CubeMesh(new int[]{2, 4, 6, 7, 9, 10, 12},					// 6
				new Vector3i[]{new Vector3i(2, 4, 9), new Vector3i(2, 9, 10), new Vector3i(6, 7, 12)}),
			new CubeMesh(new int[] {1, 2, 3, 4, 6, 7, 10, 11, 12},			// 7
				new Vector3i[]{new Vector3i(1, 10, 2), new Vector3i(4, 3, 11), new Vector3i(6, 7, 12)}),
			new CubeMesh(new int[] {2, 4, 6, 8},							// 8
				new Vector3i[]{new Vector3i(2, 4, 6), new Vector3i(4, 8, 6)}),
			new CubeMesh(new int[] {1, 4, 6, 7, 10, 11},					// 9
				new Vector3i[]{new Vector3i(1, 6, 10), new Vector3i(1, 7, 6), new Vector3i(1, 4, 7), new Vector3i(4, 11, 7)}),
			new CubeMesh(new int[] {1, 3, 5, 7, 9, 10, 11, 12 },			// 10
				new Vector3i[]{new Vector3i(1, 3, 9), new Vector3i(3, 11, 9), new Vector3i(5, 12, 10), new Vector3i(5, 7, 12)}),
			new CubeMesh(new int[] {1, 4, 7, 8, 10, 12},					// 11
				new Vector3i[]{new Vector3i(1, 4, 8), new Vector3i(1, 8, 12), new   Vector3i(1, 12, 10), new Vector3i(8, 7, 12)}),
			new CubeMesh(new int[] {1, 2, 3, 4, 6, 8, 9, 11},				// 12
				new Vector3i[]{new Vector3i(1, 9, 2), new Vector3i(2, 9, 8), new Vector3i(2, 8, 6), new Vector3i(3, 11, 4)}),
			new CubeMesh(new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},	// 13
				new Vector3i[]{new Vector3i(1, 4, 9), new Vector3i(5, 6, 10), new Vector3i(3, 2, 12), new Vector3i(7, 8, 11)}),
			new CubeMesh(new int[]{1, 2, 6, 7, 9, 11},						// 14
				new Vector3i[]{new Vector3i(1, 9, 11), new Vector3i(1, 11, 6), new Vector3i(1, 6, 2), new Vector3i(6, 11, 7)})
		};
	}
}