using UnityEngine;
using System.Collections;

namespace GravityPlatformer
{
	public class PlayerHealth : MonoBehaviour
	{
		public ParticleSystem particleSystemOnDeath;
		private SpriteRenderer _renderer;

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		public void OnHit ()
		{
			CameraShake.instance.Shake ();

			if (particleSystemOnDeath) {
				particleSystemOnDeath.Emit (100);
			}

			_renderer.enabled = false;

			StartCoroutine (DestroyAfterSeconds (1f));
		}

		private IEnumerator DestroyAfterSeconds (float seconds)
		{
			yield return new WaitForSeconds (seconds);
			Destroy (gameObject);
		}
	}
}
