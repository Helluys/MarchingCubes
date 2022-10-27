using System.Collections.Generic;

using Vector3f = System.Numerics.Vector3;
using System.Linq;
using System;
using System.Diagnostics;

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
		/// All 256 cube configurations, indexed by vertex presence. The vertices are ordered as per
		/// <see href="https://academy.cba.mit.edu/classes/scanning_printing/MarchingCubes.pdf">MIT paper</see>,
		/// from 1 to 8.
		/// </summary>
		public static readonly IDictionary<(bool, bool, bool, bool, bool, bool, bool, bool), CubeMesh> configurations =
			GenerateCubes().ToDictionary(c => (
				c.PopulatedVertices.Contains(1),
				c.PopulatedVertices.Contains(2),
				c.PopulatedVertices.Contains(3),
				c.PopulatedVertices.Contains(4),
				c.PopulatedVertices.Contains(5),
				c.PopulatedVertices.Contains(6),
				c.PopulatedVertices.Contains(7),
				c.PopulatedVertices.Contains(8)));

		/// <summary>
		/// Generates all 256 cube configurations.
		/// </summary>
		/// <returns>all 256 cube configurations</returns>
		private static IEnumerable<CubeMesh> GenerateCubes () =>
			ApplySymmetries(BaseCubes);

		/// <summary>
		/// All basic cube patterns as per <see href="https://transvoxel.org/Lengyel-VoxelTerrain.pdf">Lengyel paper</see> table 3.2.
		/// Edge indices are those of the <see href="https://academy.cba.mit.edu/classes/scanning_printing/MarchingCubes.pdf">original MIT paper</see> figures 4.
		/// </summary>
		private static IList<CubeMesh> BaseCubes => new List<CubeMesh>() {
			// 0
			new CubeMesh(new int[] {}, new int[] {}, new (int x, int y, int z)[] {}),
			// 1
			new CubeMesh(new int[] {1},
						 new int[] {1, 4, 9},
						 new (int x, int y, int z)[] {(1, 4, 9)}),
			// 2
			new CubeMesh(new int[] {1, 3},
						 new int[] {1, 2, 3, 4, 9, 12},
						 new (int x, int y, int z)[] {(1, 4, 9),
													 (2, 12, 3)}),
			// 3
			new CubeMesh(new int[] {1, 5},
						 new int[] {1, 4, 5, 8},
						 new (int x, int y, int z)[] {(1, 4, 8),
													 (1, 8, 5)}),
			// 4
			new CubeMesh(new int[] {1, 7},
						 new int[] {1, 4, 6, 7, 9, 12},
						 new (int x, int y, int z)[] {(1, 4, 9),
													 (6, 7, 12)}),
			// 5
			new CubeMesh(new int[] {1, 2, 6},
						 new int[] {2, 4, 5, 6, 9},
						 new (int x, int y, int z)[] {(2, 4, 6),
													 (6, 4, 9),
													 (6, 9, 5)}),
			// 6
			new CubeMesh(new int[] {1, 3, 5},
						 new int[] {1, 2, 3, 4, 5, 8, 12},
						 new (int x, int y, int z)[] {(1, 4, 8),
													 (1, 8, 5),
													 (2, 12, 3)}),
			// 7
			new CubeMesh(new int[] {1, 3, 8},
						 new int[] {1, 2, 3, 4, 7, 8, 9, 11, 12},
						 new (int x, int y, int z)[] {(1, 4, 9),
													 (2, 12, 3),
													 (7, 8, 11)}),
			// 8
			new CubeMesh(new int[] {1, 2, 6, 8},
						 new int[] {2, 4, 5, 6, 7, 8, 9, 11},
						 new (int x, int y, int z)[] {(2, 4, 6),
													 (6, 4, 9),
													 (6, 9, 5),
													 (7, 8, 11)}),
			// 9
			new CubeMesh(new int[] {1, 3, 5, 7},
						 new int[] {1, 2, 3, 4, 5, 6, 7, 8},
						 new (int x, int y, int z)[] {(1, 4, 8),
													 (1, 8, 5),
													 (2, 3, 7),
													 (2, 7, 6)}),
			// 10
			new CubeMesh(new int[] {1, 3, 6, 8},
						 new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
						 new (int x, int y, int z)[] {(1, 4, 9),
													 (2, 12, 3),
													 (7, 8, 11),
													 (5, 6, 10)}),
			// 11
			new CubeMesh(new int[] {1, 2, 5, 6},
						 new int[] {2, 4, 6, 8},
						 new (int x, int y, int z)[] {(2, 4, 8),
													 (2, 8, 6)}),
			// 12
			new CubeMesh(new int[] {1, 2, 4, 6},
						 new int[] {2, 3, 5, 6, 9, 11},
						 new (int x, int y, int z)[] {(2, 3, 6),
													 (6, 3, 11),
													 (6, 11, 5),
													 (5, 11, 9)}),
			// 13
			new CubeMesh(new int[] {1, 2, 3, 5},
						 new int[] {3, 4, 5, 8, 10, 12},
						 new (int x, int y, int z)[] {(4, 8, 3),
													 (8, 12, 3),
													 (8, 5, 12),
													 (5, 10, 12)}),
			// 14
			new CubeMesh(new int[] {1, 2, 3, 6},
						 new int[] {3, 4, 5, 6, 9, 12},
						 new (int x, int y, int z)[] {(4, 9, 3),
													 (9, 12, 3),
													 (9, 5, 12),
													 (5, 6, 12)}),
			// 15
			new CubeMesh(new int[] {1, 2, 3, 4, 6, 8},
						 new int[] {5, 6, 7, 8, 9, 12},
						 new (int x, int y, int z)[] {(8, 9, 7),
													 (9, 12, 7),
													 (9, 6, 12),
													 (9, 5, 6)}),
			// 16
			new CubeMesh(new int[] {1, 2, 4, 6, 8},
						 new int[] {2, 3, 5, 6, 7, 8, 9},
						 new (int x, int y, int z)[] {(8, 9, 7),
													 (9, 3, 7),
													 (9, 2, 3),
													 (9, 6, 2),
													 (9, 5, 6)}),
			// 17
			new CubeMesh(new int[] {1, 2, 3, 6, 8},
						 new int[] {3, 4, 5, 6, 7, 8, 9, 11, 12},
						 new (int x, int y, int z)[] {(4, 9, 3),
													 (9, 12, 3),
													 (9, 5, 12),
													 (5, 6, 12),
													 (7, 8, 11)})
		};

		/// <summary>
		/// The aambiguous cube configurations. 
		/// </summary>
		private static IList<int> AmbiguousCubes => new List<int>() { 2, 6, 7 };

		/// <summary>
		/// Applies rotational and reversal symmetries to the provided cubes to create equivalent configurations.<br />
		/// As per <see href="https://transvoxel.org/Lengyel-VoxelTerrain.pdf">Lengyel paper</see> paragraph 3.2, all
		/// configurations except #2, #6 and #7 should have the inversion symmetry applied.
		/// </summary>
		/// <param name="cubeMeshes">the base cube configurations</param>
		/// <returns>the generated configurations</returns>
		private static List<CubeMesh> ApplySymmetries (IEnumerable<CubeMesh> cubeMeshes) {
			return cubeMeshes.SelectMany(ApplyInversion)
							 .SelectMany(ApplyRotations)
							 .Distinct()
							 .ToList();
		}

		/// <summary>
		/// Applies inverse symmetry to the provided cube to create equivalent configurations.<br />
		/// As per <see href="https://transvoxel.org/Lengyel-VoxelTerrain.pdf">Lengyel paper</see> paragraph 3.2, all
		/// configurations except #2, #6 and #7 should have the inversion symmetry applied.
		/// </summary>
		/// <param name="c">the base cube configuration</param>
		/// <returns>the set of equivalent cube configurations obtained from inverse symmetries</returns>
		private static IEnumerable<CubeMesh> ApplyInversion (CubeMesh c) {
			return IsAmbiguous(c) ? new CubeMesh[] { c } : new CubeMesh[] { c, c.Reverse() };
		}

		/// <summary>
		/// Determines whether the provided base cube configuration is ambiguous or not.
		/// </summary>
		/// <param name="c">the base cube configuration</param>
		/// <returns>whether the provided base cube configuration is ambiguous or not</returns>
		private static bool IsAmbiguous (CubeMesh c) {
			Console.WriteLine("ambiguous " + AmbiguousCubes);
			Console.WriteLine("c " + c);
			return AmbiguousCubes.Contains(BaseCubes.IndexOf(c));
		}

		/// <summary>
		/// Applies rotational symmetries to the provided cube to create equivalent configurations.
		/// </summary>
		/// <param name="c">the base cube configuration</param>
		/// <returns>the set of equivalent cube configurations obtained from rotational symmetries</returns>
		private static IEnumerable<CubeMesh> ApplyRotations (CubeMesh c) {
			// All rotations of a cube: https://math.stackexchange.com/a/1419644
			return new CubeMesh[] {
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
				};
		}
	}
}
