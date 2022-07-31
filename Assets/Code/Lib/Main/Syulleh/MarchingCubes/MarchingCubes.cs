using static System.Math;
using System.Collections.Generic;
using System.Linq;

using Syulleh.Math;
using Field3Df = Syulleh.Math.Field3D<float>;
using System.Numerics;

namespace Syulleh.MarchingCubes
{
	public class MarchingCubes
	{
		private readonly struct EdgeVertex
		{
			public bool Present { get; }
			public Vector3? Position { get; }

			public EdgeVertex(bool present, Vector3? position)
			{
				Present = present;
				Position = position;
			}
		}

#nullable enable
		private static EdgeVertex GetEdgeVertex(Field3Df.FieldValue? a, Field3Df.FieldValue? b)
		{
			return (a != null && b != null && Sign(a.Value) != Sign(b.Value))
				? new EdgeVertex(true, new Vector3((a.X + b.X) / 2f,
													(a.Y + b.Y) / 2f,
													(a.Z + b.Z) / 2f))
				: new EdgeVertex(false, null);
		}
#nullable disable

		public static Mesh Compute(Field3Df field)
		{
			List<Vector3> vertices = new();
			List<(uint x, uint y, uint z)> triangles = new();

			Field3D<EdgeVertex[]> edgeVertices = field.Map((x, y, z, here) => new EdgeVertex[12] {
				GetEdgeVertex(here, here?.Right),
				GetEdgeVertex(here?.Right, here?.Right?.Top),
				GetEdgeVertex(here?.Right?.Top, here?.Top),
				GetEdgeVertex(here, here?.Top),
				GetEdgeVertex(here?.Front, here?.Front?.Right),
				GetEdgeVertex(here?.Front?.Right, here?.Front?.Right?.Top),
				GetEdgeVertex(here?.Front?.Right?.Top, here?.Front?.Top),
				GetEdgeVertex(here?.Front, here?.Front?.Top),
				GetEdgeVertex(here, here?.Front),
				GetEdgeVertex(here?.Right, here?.Front?.Right),
				GetEdgeVertex(here?.Top, here?.Front?.Top),
				GetEdgeVertex(here?.Right?.Top, here?.Right?.Front?.Top)
			});

			// flatten triangles list to a unique array
			return new Mesh(vertices.ToArray(), triangles.SelectMany(v => new[] { v.x, v.y, v.z }).ToArray());
		}
	}
}
