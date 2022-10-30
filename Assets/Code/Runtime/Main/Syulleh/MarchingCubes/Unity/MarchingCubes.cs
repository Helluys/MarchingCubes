using System;
using System.Linq;
using System.Security.Cryptography;

using Main.Syulleh.Math;

using Syulleh.Math;

using UnityEditor;

using UnityEngine;

using MarchingCubesLib = Syulleh.MarchingCubes.MarchingCubes;

namespace Syulleh.MarchingCubes.Unity {
	public class MarchingCubes {
		/// <summary>
		/// Creates a marching cubes mesh from a Perlin noise generated field.
		/// </summary>
		/// <param name="parameters">the field and mesh parameters</param>
		/// <param name="material">the material for the mesh renderer</param>
		/// <returns>a marching cubes mesh from a Perlin noise generated field</returns>
		public static GameObject Create (MarchingCubesParameters parameters, Material material) {
			Field3D<float> field = CreateField(parameters);
			Mesh uMesh = CreateUnityMesh(parameters.Threshold, field);
			return CreateGameObject(material, uMesh);
		}

		/// <summary>
		/// Creates a marching cube mesh from the provided field.
		/// </summary>
		/// <param name="field">the value field</param>
		/// <param name="threshold">the field threshold for surface generation</param>
		/// <param name="material">the material for the mesh renderer</param>
		/// <returns>a marching cube mesh from the provided field</returns>
		public static GameObject Create (Field3D<float> field, float threshold, Material material) {
			Mesh uMesh = CreateUnityMesh(threshold, field);
			return CreateGameObject(material, uMesh);
		}

		public static Field3D<float> CreateField (MarchingCubesParameters parameters) {
			float evaluator (int x, int y, int z) {
				Vector3 fieldCoord = new(x, y, z);
				Vector3 sampleCoord = parameters.Bounds.min
						  + Vector3.Scale(fieldCoord, parameters.Resolution.Inverse());

				return parameters.FieldNoise
					* Perlin.Noise(sampleCoord)
					+ parameters.FieldConstant
					+ Vector3.Dot(parameters.FieldLinear, fieldCoord);
			}

			Debug.Log("Generating " + parameters.Resolution + " 3D field...");
			Field3D<float> field = new(parameters.Resolution.x, parameters.Resolution.y, parameters.Resolution.z, evaluator);
			Debug.Log("Done");
			return field;
		}

		private static Mesh CreateUnityMesh (float threshold, Field3D<float> field) {
			Debug.Log("Generating mesh from 3D field...");
			MeshData meshData = MarchingCubesLib.Compute(field, threshold);

			Mesh mesh = new() {
				vertices = meshData.vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray(),
				triangles = meshData.triangles
			};
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			Debug.Log("Done");
			return mesh;
		}

		private static GameObject CreateGameObject (Material material, Mesh uMesh) {
			GameObject go = new("Marching cubes mesh");
			go.AddComponent<MeshFilter>().mesh = uMesh;
			go.AddComponent<MeshRenderer>().material = material;
			return go;
		}
	}
}