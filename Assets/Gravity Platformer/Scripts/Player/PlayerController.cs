using UnityEngine;
using System.Collections;

namespace GravityPlatformer
{
	[RequireComponent (typeof(Animator), typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour
	{
		public float moveForce = 365f;
		public float maxSpeed = 5f;
		public float jumpForce = 1000f;
		public float rotationSpeed = 10f;
		public Transform groundCheck;
		public LayerMask platformMask;

		public float projectileForce = 300f;
		public float projectileKnockBackForce = 50f;
		public Transform[] bulletSpawners;
		public GameObject bulletPrefab;
		
		private bool _isGrounded = false;
		private Animator _animator;
		private Rigidbody2D _rigidbody;
		private bool _facingRight = true;
		private bool _shouldJump = false;
		private int flipped = 1;
		private float flipTime = 0.3f;

		private bool _isFlipping = false;
		
		
		// Use this for initialization
		void Awake ()
		{
			_animator = GetComponent<Animator> ();
			_rigidbody = GetComponent<Rigidbody2D> ();
		}
		
		// Update is called once per frame
		void Update ()
		{
			_isGrounded = Physics2D.Linecast (transform.position, groundCheck.position, platformMask);

			if (_isGrounded) {
				_animator.SetBool ("Jumping", false);
			}
			
			if (Input.GetButtonDown ("Jump") && _isGrounded) {
				_shouldJump = true;
			}

			if (Input.GetKeyDown (KeyCode.E) && !_isFlipping) {
				_animator.SetTrigger ("Shooting");
			} 

			if (Input.GetKeyUp (KeyCode.R) && !_isFlipping) {
				FlipUpDown ();
			}
		}
		
		void FixedUpdate ()
		{
			float h = Input.GetAxis ("Horizontal");
			
			_animator.SetFloat ("Speed", Mathf.Abs (h));

			Debug.DrawRay (transform.position, transform.right);
			
			if (h * _rigidbody.velocity.x < maxSpeed)
				_rigidbody.AddForce (Vector2.right * h * moveForce);
			
			if (Mathf.Abs (_rigidbody.velocity.x) > maxSpeed)
				_rigidbody.velocity = new Vector2 (Mathf.Sign (_rigidbody.velocity.x) * maxSpeed, _rigidbody.velocity.y);
			
			if (h > 0 && !_facingRight)
				Flip ();
			else if (h < 0 && _facingRight)
				Flip ();
			
			if (_shouldJump) {
				_animator.SetBool ("Jumping", true);
				var jump = (flipped > 0) ? jumpForce : -jumpForce;
				_rigidbody.AddForce (new Vector2 (0f, jump));
				_shouldJump = false;
			}
		}

		public void Shoot ()
		{

			foreach (var spawner in bulletSpawners) {
				var bullet = (GameObject)Instantiate (bulletPrefab, spawner.position, spawner.rotation);
				var shootDir = ShootDirection (bullet);
				bullet.GetComponent<Rigidbody2D> ().AddForce (shootDir * projectileForce);
			}

			_rigidbody.AddForce (-ShootDirection (gameObject) * projectileKnockBackForce);
		}

		private Vector2 ShootDirection (GameObject obj)
		{
			if (flipped > 0) {
				return (_facingRight) ? obj.transform.right : -obj.transform.right;
			} else {
				return (_facingRight) ? -obj.transform.right : obj.transform.right;
			}
		}
		
		private void Flip ()
		{
			_facingRight = !_facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		private void FlipUpDown ()
		{
			_rigidbody.gravityScale *= -1;
			_animator.SetBool ("Jumping", true);
			StartCoroutine (FlipUpsideDown ());
		}
		
		private  IEnumerator FlipUpsideDown ()
		{
			_isFlipping = true;

			float endRotation;
			
			if (flipped > 0) {
				endRotation = 180.0f;
			} else {
				endRotation = 0.0f;
			}
			
			flipped *= -1;
			
			float activationTime = Time.time;
			
			while (Time.time < activationTime + flipTime) {

				transform.Rotate (new Vector3 (0, 0, rotationSpeed * Time.deltaTime));

				yield return null;
			}

			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, endRotation));

			_facingRight = !_facingRight;

			_isFlipping = false;
		}
	}
}
