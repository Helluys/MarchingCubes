using System.Collections.Generic;
using System.Linq;

using Field3Df = Syulleh.Math.Field3D<float>;
using System.Numerics;
using System;

namespace Syulleh.MarchingCubes {
	public class MarchingCubes {
		private readonly struct Cube {
			public readonly CubeMesh cubeMesh;
			public readonly Field3Df.FieldValue fieldValue;

			public Cube (CubeMesh cubeMesh, Field3Df.FieldValue fieldValue) {
				this.cubeMesh = cubeMesh;
				this.fieldValue = fieldValue;
			}
		}

		public static Mesh Compute (Field3Df field, float threshold) {
			List<Vector3> vertices = new();
			List<(int x, int y, int z)> triangles = new();

			field.Map(v => GetCube(v, v => v != null && v >= threshold))
				.ForEach(cube => {
					int offset = vertices.Count;
					vertices.AddRange(cube.Value.cubeMesh.PopulatedEdges
						.Select(i => PlaceVertex(i, cube.Value.fieldValue)));
					triangles.AddRange(cube.Value.cubeMesh.Triangles
						.Select(t => (
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.x) + offset,
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.y) + offset,
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.z) + offset
						)));
				});

			// flatten triangles list to a unique array
			return new Mesh(vertices.ToArray(), triangles.SelectMany(v => new[] { v.x, v.y, v.z }).ToArray());
		}

#nullable enable
		private static Cube GetCube (Field3Df.FieldValue? value, Func<float?, bool> presenceFunc) {
			(bool, bool, bool, bool, bool, bool, bool, bool) presence =
				(presenceFunc(value?.Value),
				presenceFunc(value?.Right?.Value),
				presenceFunc(value?.Right?.Top?.Value),
				presenceFunc(value?.Top?.Value),
				presenceFunc(value?.Front?.Value),
				presenceFunc(value?.Front?.Right?.Value),
				presenceFunc(value?.Front?.Right?.Top?.Value),
				presenceFunc(value?.Front?.Top?.Value));
			return new Cube(MeshLookupTable.configurations[presence], value);
		}
#nullable disable

		static Vector3 PlaceVertex (int edgeIndex, Field3Df.FieldValue fieldValue) =>
			edgeIndex switch {
				1 => LerpVertex(fieldValue, fieldValue.Right),
				2 => LerpVertex(fieldValue.Right, fieldValue.Right?.Top),
				3 => LerpVertex(fieldValue.Right?.Top, fieldValue.Top),
				4 => LerpVertex(fieldValue, fieldValue.Top),
				5 => LerpVertex(fieldValue.Front, fieldValue.Front?.Right),
				6 => LerpVertex(fieldValue.Front?.Right, fieldValue.Front?.Right?.Top),
				7 => LerpVertex(fieldValue.Front?.Top, fieldValue.Front?.Right?.Top),
				8 => LerpVertex(fieldValue.Front, fieldValue.Front?.Top),
				9 => LerpVertex(fieldValue, fieldValue.Front),
				10 => LerpVertex(fieldValue.Right, fieldValue.Right?.Front),
				11 => LerpVertex(fieldValue.Top, fieldValue.Top?.Front),
				12 => LerpVertex(fieldValue.Top?.Right, fieldValue.Top?.Right?.Front),
				_ => throw new ArgumentException("Invalid edge index " + edgeIndex)
			};

#nullable enable
		private static Vector3 LerpVertex (Field3Df.FieldValue? a, Field3Df.FieldValue? b) {
			// a or b must be present
			float aValue = a?.Value ?? b.Value;
			float bValue = b?.Value ?? a.Value;
			float aWeight = (aValue + bValue != 0) ? aValue / (aValue + bValue) : 0.5f;

			Vector3 aPos = (a != null) ? new Vector3(a.X, a.Y, a.Z) : new Vector3(b.X, b.Y, b.Z);
			Vector3 bPos = (b != null) ? new Vector3(b.X, b.Y, b.Z) : new Vector3(a.X, a.Y, a.Z);

			return aWeight * aPos + (1f - aWeight) * bPos;
		}
#nullable disable

		private static Func<(int, int, int), (int, int, int)> AddTriplet (int increment) =>
			x => (x.Item1 + increment, x.Item2 + increment, x.Item3 + increment);
	}
}
