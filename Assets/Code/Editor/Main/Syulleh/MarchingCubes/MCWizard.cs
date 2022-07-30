using UnityMesh = UnityEngine.Mesh;

using UnityEditor;
using static Syulleh.MarchingCubes.MeshLookupTable;
using System.Linq;
using System;
using UnityEngine;

namespace Syulleh.MarchingCubes {
	public class MCWizard : ScriptableWizard {
		public Material material;

		[MenuItem("Syulleh/Marching Cubes")]
		static void DisplayMCWizard () {
			DisplayWizard<MCWizard>("Marching Cubes");
		}

		private void OnWizardCreate () {
			for (int i = 0; i < 15; i++) {
				CubeMesh baseCube = baseCubes[i];

				UnityMesh uMesh = new();
				uMesh.vertices = baseCube.PopulatedEdges
					.Select(i => edgeToPos[i])
					.Select(v => new Vector3(v.x, v.y, v.z))
					.ToArray();
				uMesh.triangles = baseCube.Triangles.Select(t => new int[] {
					Array.FindIndex(baseCube.PopulatedEdges, i => i == t.x),
					Array.FindIndex(baseCube.PopulatedEdges, i => i == t.y),
					Array.FindIndex(baseCube.PopulatedEdges, i => i == t.z)})
					.SelectMany(tri => tri)
					.ToArray();

				GameObject go = new("Base cube " + i);
				go.AddComponent<MeshFilter>().mesh = uMesh;
				go.AddComponent<MeshRenderer>().material = material;

				// Arrange as in MIT paper
				go.transform.position = new Vector3((i % 3) * 2, -(i / 3) * 2, 0);
				
				// Add a unit box collider to see unit coordinates in editor
				BoxCollider boxCollider = go.AddComponent<BoxCollider>();
				boxCollider.size = Vector3.one;
				boxCollider.center = 0.5f * Vector3.one;
			}
		}
	}
}
