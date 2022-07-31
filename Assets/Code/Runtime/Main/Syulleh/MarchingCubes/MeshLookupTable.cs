using System.Collections.Generic;

using Vector3i = Syulleh.Math.Vector3<int>;
using Vector3f = Syulleh.Math.Vector3<float>;
using System.Linq;
using System;

namespace Syulleh.MarchingCubes {
	/// <summary>
	/// Lookup table access and generation for Marching Cube unit cube configurations.
	/// </summary>
	public static partial class MeshLookupTable {

		/// <summary>
		/// Temporary implementation of 3D vertex placement from populated edge.
		/// </summary>
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
		/// All 256 cube configurations.
		/// </summary>
		public static readonly IEnumerable<CubeMesh> allCubes = GenerateCubes();

		/// <summary>
		/// Generates all 256 cube configurations.
		/// </summary>
		/// <returns>all 256 cube configurations</returns>
		private static IEnumerable<CubeMesh> GenerateCubes () =>
			ApplySymmetries(BaseCubes());

		/// <summary>
		/// All basic cube patterns as per <see href="https://academy.cba.mit.edu/classes/scanning_printing/MarchingCubes.pdf">MIT paper</see> figures 3 and 4.
		/// Edge index are those of the paper, including in the triangle structure.
		/// </summary>
		private static IEnumerable<CubeMesh> BaseCubes () => new List<CubeMesh>() {
			// 0
			new CubeMesh(new int[]{}, new int[]{}, new Vector3i[]{}),
			// 1
			new CubeMesh(new int[]{1},
						 new int[]{1, 4, 9},
						 new Vector3i[]{new Vector3i(1, 4, 9)}),
			// 2
			new CubeMesh(new int[]{1, 2},
						 new int[]{2, 4, 9, 10},
						 new Vector3i[]{new Vector3i(2, 4, 9),
										new Vector3i(2, 9, 10)}),
			// 3
			new CubeMesh(new int[]{1, 3},
						 new int[]{1, 2, 3, 4, 9, 12},
						 new Vector3i[]{new Vector3i(1, 4, 9),
										new Vector3i(2, 12, 3)}),
			// 4
			new CubeMesh(new int[]{1, 7},
						 new int[]{1, 4, 6, 7, 9, 12},
						 new Vector3i[]{new Vector3i(1, 4, 9),
										new Vector3i(6, 7, 12)}),
			// 5
			new CubeMesh(new int[]{2, 5, 6},
						 new int[]{1, 2, 6, 8, 9},
						 new Vector3i[]{new Vector3i(1, 9, 2),
										new Vector3i(2, 9, 8),
										new Vector3i(2, 8, 6)}),
			// 6
			new CubeMesh(new int[]{1, 2, 7},
						 new int[]{2, 4, 6, 7, 9, 10, 12},
						 new Vector3i[]{new Vector3i(2, 4, 9),
										new Vector3i(2, 9, 10),
										new Vector3i(6, 7, 12)}),
			// 7
			new CubeMesh(new int[]{2, 4, 7},
						 new int[] {1, 2, 3, 4, 6, 7, 10, 11, 12},
						 new Vector3i[]{new Vector3i(1, 10, 2),
										new Vector3i(4, 3, 11),
										new Vector3i(6, 7, 12)}),
			// 8
			new CubeMesh(new int[]{1, 2, 5, 6},
						 new int[] {2, 4, 6, 8},
						 new Vector3i[]{new Vector3i(2, 4, 6),
										new Vector3i(4, 8, 6)}),
			// 9
			new CubeMesh(new int[] {1, 5, 6, 8},
						 new int[] {1, 4, 6, 7, 10, 11},
						 new Vector3i[]{new Vector3i(1, 6, 10),
										new Vector3i(1, 7, 6),
										new Vector3i(1, 4, 7),
										new Vector3i(4, 11, 7)}),
			// 10
			new CubeMesh(new int[] {1, 4, 6, 7},
						 new int[] {1, 3, 5, 7, 9, 10, 11, 12},
						 new Vector3i[]{new Vector3i(1, 3, 9),
										new Vector3i(3, 11, 9),
										new Vector3i(5, 12, 10),
										new Vector3i(5, 7, 12)}),
			// 11
			new CubeMesh(new int[]{1, 5, 6, 7},
						 new int[] {1, 4, 7, 8, 10, 12},
						 new Vector3i[]{new Vector3i(1, 4, 8),
										new Vector3i(1, 8, 12),
										new Vector3i(1, 12, 10),
										new Vector3i(8, 7, 12)}),
			// 12
			new CubeMesh(new int[]{2, 4, 5, 6},
						 new int[] {1, 2, 3, 4, 6, 8, 9, 11},
						 new Vector3i[]{new Vector3i(1, 9, 2),
										new Vector3i(2, 9, 8),
										new Vector3i(2, 8, 6),
										new Vector3i(3, 11, 4)}),
			// 13
			new CubeMesh(new int[]{1, 3, 6, 8},
						 new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
						 new Vector3i[]{new Vector3i(1, 4, 9),
										new Vector3i(5, 6, 10),
										new Vector3i(3, 2, 12),
										new Vector3i(7, 8, 11)}),
			// 14
			new CubeMesh(new int[]{2, 5, 6, 8},
						 new int[]{1, 2, 6, 7, 9, 11},
						 new Vector3i[]{new Vector3i(1, 9, 11),
										new Vector3i(1, 11, 6),
										new Vector3i(1, 6, 2),
										new Vector3i(6, 11, 7)})
		};

		/// <summary>
		/// Applies rotational and reversal symmetries to the provided cubes to create more configurations.
		/// </summary>
		/// <param name="cubeMeshes">the base cube configurations</param>
		/// <returns>the generated configurations</returns>
		private static List<CubeMesh> ApplySymmetries (IEnumerable<CubeMesh> cubeMeshes) =>
			cubeMeshes
				.SelectMany(c => new CubeMesh[] { c, c.Reverse() })
				// All rotations of a cube: https://math.stackexchange.com/a/1419644
				.SelectMany(c => new CubeMesh[] {
					// Identity
					c,																				
					// X
					c.Map(RotateVertexX, RotateEdgeX),												
					// Y
					c.Map(RotateVertexY, RotateEdgeY),												
					// Z
					c.Map(RotateVertexZ, RotateEdgeZ),												
					// XX
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX),				
					// XY
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY),				
					// XZ
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexZ, RotateEdgeZ),				
					// YX
					c.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexX, RotateEdgeX),				
					// YY
					c.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY),				
					// ZY
					c.Map(RotateVertexZ, RotateEdgeZ)
						.Map(RotateVertexY, RotateEdgeY),	
					// ZZ
					c.Map(RotateVertexZ, RotateEdgeZ)
						.Map(RotateVertexZ, RotateEdgeZ),
					// XXX
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX),
					// XXY
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY),
					// XXZ
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexZ, RotateEdgeZ),
					// XYX
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexX, RotateEdgeX),
					// XYY
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY),
					// XZZ
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexZ, RotateEdgeZ)
						.Map(RotateVertexZ, RotateEdgeZ),
					// YXX
					c.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX),
					// YYY
					c.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY),
					// ZZZ
					c.Map(RotateVertexZ, RotateEdgeZ)
						.Map(RotateVertexZ, RotateEdgeZ)
						.Map(RotateVertexZ, RotateEdgeZ),
					// XXXY
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY),
					// XXYX
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexX, RotateEdgeX),
					// XYXX
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexX, RotateEdgeX),
					// XYYY
					c.Map(RotateVertexX, RotateEdgeX)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY)
						.Map(RotateVertexY, RotateEdgeY)
				})
				.Distinct()
				.ToList();
	}
}
