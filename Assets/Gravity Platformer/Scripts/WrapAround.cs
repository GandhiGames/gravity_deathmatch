using UnityEngine;
using System.Collections;


namespace GravityPlatformer
{
	public class WrapAround : MonoBehaviour
	{
		public enum WrapAroundVector
		{
			X,
			Y
		}
		public WrapAroundVector wrapAroundVector;

		public Transform wrapAroundTarget;

		void OnTriggerEnter2D (Collider2D other)
		{
			Vector2 newPos = (wrapAroundVector == WrapAroundVector.X) ? new Vector2 (wrapAroundTarget.transform.position.x, other.transform.position.y) :
					new Vector2 (other.transform.position.x, wrapAroundTarget.position.y);

			other.transform.position = newPos;

		}
	}
}
