using System.Numerics;

namespace Syulleh.MarchingCubes {
	public struct Mesh {
		public readonly Vector3[] vertices;
		public readonly int[] triangles;

		public Mesh (Vector3[] vertices, int[] triangles) {
			this.vertices = vertices;
			this.triangles = triangles;
		}
	}
}