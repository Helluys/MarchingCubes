using System.Collections;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

namespace Syulleh.MarchingCubes.Unity {
	public class MarchingCubesTest {
		[UnityTest]
		public IEnumerator MarchingCubesTestWithEnumeratorPasses () {
			MarchingCubesParameters parameters = ScriptableObject.CreateInstance<MarchingCubesParameters>();
			parameters.Bounds = new Bounds(0.5f * Vector3.one, Vector3.one);
			parameters.Resolution = new Vector3Int(10, 10, 10);
			parameters.Threshold = .5f;
			parameters.FieldConstant = .5f;
			parameters.FieldNoise = .2f;
			parameters.FieldLinear = new Vector3(0f, 1f, 0f);
			GameObject go = MarchingCubes.Create(parameters, null);
			yield return null;

			Assert.IsNotNull(go.GetComponent<MeshRenderer>());
		}
	}
}