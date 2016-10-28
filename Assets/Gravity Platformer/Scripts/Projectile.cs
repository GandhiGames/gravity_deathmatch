using UnityEngine;
using System.Collections;

namespace GravityPlatformer
{
	public class Projectile : MonoBehaviour
	{
		public float timeAlive = 5f;
		public LayerMask platformMask;
		public ParticleSystem projectileHitWallParticles;
		private float _currentTimeAlive;
		private float _ignoreCollisionTime = 0.1f;
		private SpriteRenderer _renderer;

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		void OnEnable ()
		{
			_currentTimeAlive = 0f;
		}

		void Update ()
		{
			_currentTimeAlive += Time.deltaTime;

			if (_currentTimeAlive >= timeAlive) {
				Destroy (gameObject);
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (_currentTimeAlive < _ignoreCollisionTime) {
				return;
			}

			if (other.CompareTag ("Platform")) {
				if (projectileHitWallParticles) {

					var hit = GetPointOfContact ();

					Vector2 incomingVec = hit.point - (Vector2)transform.position;
					Vector3 reflectVec = Vector3.Reflect (incomingVec, hit.normal);

					projectileHitWallParticles.transform.LookAt (projectileHitWallParticles.transform.position + reflectVec);
					projectileHitWallParticles.Emit (100);
				}
				_renderer.enabled = false;

				StartCoroutine (DestroyAfterSeconds (1f));

			} else if (other.CompareTag ("Player")) {
				other.gameObject.GetComponent<PlayerHealth> ().OnHit ();
				Destroy (gameObject);
			}
		}

		private IEnumerator DestroyAfterSeconds (float seconds)
		{
			yield return new WaitForSeconds (seconds);
			Destroy (gameObject);
		}

		private RaycastHit2D GetPointOfContact ()
		{
			return Physics2D.Raycast (transform.position, transform.right, 1f, platformMask);
		}
	}
}
