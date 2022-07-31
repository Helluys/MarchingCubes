using System.Numerics;

using Syulleh.Math;

namespace Syulleh.MarchingCubes
{
	public struct Mesh
	{
		public readonly Vector3[] vertices;
		public readonly uint[] triangles;

		public Mesh(Vector3[] vertices, uint[] triangles)
		{
			this.vertices = vertices;
			this.triangles = triangles;
		}
	}
}