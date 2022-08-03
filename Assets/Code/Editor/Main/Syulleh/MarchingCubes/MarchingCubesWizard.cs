using System;
using System.Linq;

using Syulleh.MarchingCubes.Unity;

using UnityEditor;

using UnityEngine;

using static Syulleh.MarchingCubes.MeshLookupTable;

using UnityMesh = UnityEngine.Mesh;

namespace Syulleh.MarchingCubes {
	public class MarchingCubesWizard : ScriptableWizard {
		public float threshold;
		public Material material;
		public Field3D field;

		[MenuItem("Syulleh/Marching Cubes - for real")]
		static void DisplayMCWizard () {
			DisplayWizard<MarchingCubesWizard>("Marching  Cubes");
		}

		private void OnWizardCreate () {
			Debug.Log("Generating " + configurations.Count() + " cubes...");
			Mesh mesh = MarchingCubes.Compute(field.Field, threshold);

			Debug.Log(String.Join(", ", mesh.vertices));
			Debug.Log(String.Join(", ", mesh.triangles));

			UnityMesh uMesh = new() {
				vertices = mesh.vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray(),
				triangles = mesh.triangles
			};

			// Instantiate game object with mesh and renderer
			GameObject go = new("Marching cubes!");
			go.AddComponent<MeshFilter>().mesh = uMesh;
			go.AddComponent<MeshRenderer>().material = material;

			Debug.Log("Done");
		}
	}
}
