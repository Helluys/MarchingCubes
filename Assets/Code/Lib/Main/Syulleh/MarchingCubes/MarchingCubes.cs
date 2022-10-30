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

		public static MeshData Compute (Field3Df field, float threshold) {
			List<Vector3> vertices = new();
			List<(int x, int y, int z)> triangles = new();

			field.Map(v => GetCube(v, threshold))
				.ForEach(cube => {
					int offset = vertices.Count;
					vertices.AddRange(cube.Value.cubeMesh.PopulatedEdges
						.Select(i => PlaceVertex(i, cube.Value.fieldValue, threshold)));
					triangles.AddRange(cube.Value.cubeMesh.Triangles
						.Select(t => (
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.x) + offset,
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.y) + offset,
							Array.FindIndex(cube.Value.cubeMesh.PopulatedEdges, i => i == t.z) + offset
						)));
				});

			// flatten triangles list to a unique array
			return new MeshData(vertices.ToArray(), triangles.SelectMany(v => new[] { v.x, v.y, v.z }).ToArray());
		}

		private static Cube GetCube (Field3Df.FieldValue value, float threshold) {
			bool presenceFunc (float f) => f >= threshold;

			(bool, bool, bool, bool, bool, bool, bool, bool) presence =
				(presenceFunc(value.SafeValue),
				presenceFunc(value.Right.SafeValue),
				presenceFunc(value.Right.Top.SafeValue),
				presenceFunc(value.Top.SafeValue),
				presenceFunc(value.Front.SafeValue),
				presenceFunc(value.Front.Right.SafeValue),
				presenceFunc(value.Front.Right.Top.SafeValue),
				presenceFunc(value.Front.Top.SafeValue));

			return new Cube(MeshLookupTable.configurations[presence], value);
		}

		static Vector3 PlaceVertex (int edgeIndex, Field3Df.FieldValue fieldValue, float threshold) =>
			edgeIndex switch {
				1 => LerpVertex(fieldValue, fieldValue.Right, threshold),
				2 => LerpVertex(fieldValue.Right, fieldValue.Right.Top, threshold),
				3 => LerpVertex(fieldValue.Right.Top, fieldValue.Top, threshold),
				4 => LerpVertex(fieldValue, fieldValue.Top, threshold),
				5 => LerpVertex(fieldValue.Front, fieldValue.Front.Right, threshold),
				6 => LerpVertex(fieldValue.Front.Right, fieldValue.Front.Right.Top, threshold),
				7 => LerpVertex(fieldValue.Front.Top, fieldValue.Front.Right.Top, threshold),
				8 => LerpVertex(fieldValue.Front, fieldValue.Front.Top, threshold),
				9 => LerpVertex(fieldValue, fieldValue.Front, threshold),
				10 => LerpVertex(fieldValue.Right, fieldValue.Right.Front, threshold),
				11 => LerpVertex(fieldValue.Top, fieldValue.Top.Front, threshold),
				12 => LerpVertex(fieldValue.Top.Right, fieldValue.Top.Right.Front, threshold),
				_ => throw new ArgumentException("Invalid edge index " + edgeIndex)
			};

		private static Vector3 LerpVertex (Field3Df.FieldValue a, Field3Df.FieldValue b, float threshold) {
			float x = a.SafeValue + b.SafeValue != 0 ? (threshold - a.SafeValue) / (b.SafeValue - a.SafeValue) : 0.5f;

			Vector3 aPos = new(a.X, a.Y, a.Z);
			Vector3 bPos = new(b.X, b.Y, b.Z);
			return aPos + x * (bPos - aPos);
		}
	}
}
