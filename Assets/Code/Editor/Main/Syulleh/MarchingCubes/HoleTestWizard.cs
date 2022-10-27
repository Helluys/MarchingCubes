using System.Linq;

using Syulleh.Math;

using UnityEditor;

using UnityEngine;

using UnityMesh = UnityEngine.Mesh;
using UnityField3D = Syulleh.MarchingCubes.Unity.Field3D;
using System.Collections;
using System.Collections.Generic;

namespace Syulleh.MarchingCubes {
	public class HoleTestWizard : ScriptableWizard {
		public Material material;

		[MenuItem("Syulleh/Marching Cubes - hole test")]
		static void DisplayHoleWizard () {
			DisplayWizard<HoleTestWizard>("Marching Cubes");
		}

		private static readonly IDictionary<(int, int, int), float> values = new Dictionary<(int, int, int), float>() {
			{(0, 0, 0), 0f},
			{(1, 0, 0), 0f},
			{(0, 0, 1), 0f},
			{(1, 0, 1), 0f},
			{(0, 1, 0), 0f},
			{(1, 1, 0), 1f},
			{(0, 1, 1), 1f},
			{(1, 1, 1), 0f},
			{(0, 2, 0), 1f},
			{(1, 2, 0), 1f},
			{(0, 2, 1), 1f},
			{(1, 2, 1), 1f}
		};

		private void OnWizardCreate () {
			Field3D<float> field = new(2, 3, 2, (x, y, z) => values[(x, y, z)]);
			Mesh mesh = MarchingCubes.Compute(field, 0.5f);

			UnityMesh uMesh = new() {
				vertices = mesh.vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray(),
				triangles = mesh.triangles
			};
			uMesh.RecalculateNormals();
			uMesh.RecalculateBounds();

			// Instantiate game object with 3D field
			GameObject fieldGo = new("Field");
			fieldGo.AddComponent<UnityField3D>().Field = field;

			// Instantiate game object with mesh and renderer
			GameObject meshGo = new("Mesh");
			meshGo.AddComponent<MeshFilter>().mesh = uMesh;
			meshGo.AddComponent<MeshRenderer>().material = material;

			Debug.Log("Done");
		}
	}
}
