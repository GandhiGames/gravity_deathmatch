using UnityEngine;
using System.Collections;

namespace GravityPlatformer
{
	public class CameraShake : MonoBehaviour
	{
		public float shakeIntensityStart = 0.3f;
		public float shakeDecay = 0.02f;

		public static CameraShake instance;
		
		private Vector3 startPos;
		private bool shaking = false;
		private float shakeIntensity;

		
		void Awake ()
		{
			instance = this;
		}

		
		// Update is called once per frame
		void Update ()
		{
			if (shaking) {
				transform.position = startPos + Random.insideUnitSphere * shakeIntensity;
				shakeIntensity -= shakeDecay;
			}
			if (shakeIntensity < 0) {
				StopShake ();
			}
		}
		
		public void Shake ()
		{
			if (!shaking)
				startPos = transform.position;
			shakeIntensity = shakeIntensityStart;
			shaking = true;
		}
		
		void StopShake ()
		{
			shaking = false;
			transform.position = startPos;
		}
	}
}
