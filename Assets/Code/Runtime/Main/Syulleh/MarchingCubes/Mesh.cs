using Syulleh.Math;

namespace Syulleh.MarchingCubes
{
	public struct Mesh
	{
		public readonly Vector3<float>[] vertices;
		public readonly uint[] triangles;

		public Mesh(Vector3<float>[] vertices, uint[] triangles)
		{
			this.vertices = vertices;
			this.triangles = triangles;
		}
	}
}