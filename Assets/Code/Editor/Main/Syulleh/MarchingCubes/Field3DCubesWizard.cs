using UnityEditor;
using UnityEngine;
using Syulleh.Math;
using Syulleh.MarchingCubes.Unity;

namespace Syulleh.MarchingCubes {
	public class Field3DCubesWizard : ScriptableWizard {

		public Vector3Int size;
		public Vector3Int origin;

		[MenuItem("Syulleh/3D Field")]
		static void Display3DFieldWizard () {
			DisplayWizard<Field3DCubesWizard>("3D Field");
		}

		private void OnWizardCreate () {
			Debug.Log("Generating " + size + " 3D field...");

			new GameObject("Field 3D").AddComponent<Field3D>().Field =
				new Field3D<float>((uint) size.x, (uint) size.y, (uint) size.z,
					(x, y, z) => Perlin.Noise(x + origin.x + .5f, y + origin.y + .5f, z + origin.z + .5f));
			Debug.Log("Done");
		}
	}
}
