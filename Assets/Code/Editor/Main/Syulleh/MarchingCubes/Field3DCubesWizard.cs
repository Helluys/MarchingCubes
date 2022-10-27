using UnityEditor;
using UnityEngine;
using Syulleh.Math;
using Syulleh.MarchingCubes.Unity;

namespace Syulleh.MarchingCubes {
	public class Field3DCubesWizard : ScriptableWizard {

		public Vector3Int size;
		public Vector3Int origin;

		public float constant, noiseFactor;
		public Vector3 linear;

		[MenuItem("Syulleh/3D Field")]
		static void Display3DFieldWizard () {
			DisplayWizard<Field3DCubesWizard>("3D Field");
		}

		private void OnWizardCreate () {
			Debug.Log("Generating " + size + " 3D field...");

			float evaluator (int x, int y, int z) =>
				noiseFactor * Perlin.Noise(x + origin.x + .5f, y + origin.y + .5f, z + origin.z + .5f)
				+ constant + linear.x * x + linear.y * y + linear.z * z;

			new GameObject("Field 3D").AddComponent<Field3D>().Field =
				new Field3D<float>(size.x, size.y, size.z, evaluator);
			Debug.Log("Done");
		}
	}
}
