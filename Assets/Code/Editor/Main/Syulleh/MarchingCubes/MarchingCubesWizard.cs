using UnityMesh = UnityEngine.Mesh;

using UnityEditor;
using static Syulleh.MarchingCubes.MeshLookupTable;
using System.Linq;
using System;
using UnityEngine;

namespace Syulleh.MarchingCubes {
	public class MarchingCubesWizard : ScriptableWizard {
		public Material material;

		[MenuItem("Syulleh/Marching Cubes")]
		static void DisplayMCWizard () {
			DisplayWizard<MarchingCubesWizard>("Marching Cubes");
		}

		private void OnWizardCreate () {
			Debug.Log("Generating " + allCubes.Count() + " cubes...");
			int i = 0;
			foreach (CubeMesh cube in allCubes) {
				UnityMesh uMesh = new() {
					// Compute vertex coordinates
					vertices = cube.PopulatedEdges
						.Select(i => edgeToPos[i])
						.Select(v => new Vector3(v.X, v.Y, v.Z))
						.ToArray(),

					// Map triangle array to vertex index instead of value
					triangles = cube.Triangles
						.Select(t => new int[] {
							Array.FindIndex(cube.PopulatedEdges, i => i == t.x),
							Array.FindIndex(cube.PopulatedEdges, i => i == t.y),
							Array.FindIndex(cube.PopulatedEdges, i => i == t.z)})
						.SelectMany(tri => tri)
						.ToArray()
				};

				// Instantiate game object with mesh and renderer
				GameObject go = new("Base cube [" + string.Join(",", cube.PopulatedVertices) + "] : [" + string.Join(",", cube.PopulatedEdges) + "]");
				go.AddComponent<MeshFilter>().mesh = uMesh;
				go.AddComponent<MeshRenderer>().material = material;

				// Arrange in space
				go.transform.position = new Vector3((i % 14) * 2, -(i / 14) * 2, 0);
				i++;

				// Add a unit box collider to see unit coordinates in editor
				BoxCollider boxCollider = go.AddComponent<BoxCollider>();
				boxCollider.size = Vector3.one;
				boxCollider.center = 0.5f * Vector3.one;
			}

			Debug.Log("Done");
		}
	}
}
