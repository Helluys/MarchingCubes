using System.Numerics;

namespace Syulleh.MarchingCubes {
	public struct MeshData {
		public readonly Vector3[] vertices;
		public readonly int[] triangles;

		public MeshData (Vector3[] vertices, int[] triangles) {
			this.vertices = vertices;
			this.triangles = triangles;
		}
	}
}