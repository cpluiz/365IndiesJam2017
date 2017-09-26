using UnityEngine;

public class MovingBall : MonoBehaviour {
	[Range(0.5f,10)]
	public float distance = 0.3f;
	[Range(0.1f, 4)]
	public float period;
	public Transform movablePart;
	Rigidbody2D bodyPart;

	void Start(){
		//movablePart.connectedAnchor = (Vector2)transform.position;
		bodyPart = movablePart.GetComponent<Rigidbody2D>();
	}

	void LateUpdate(){
		//movablePart.anchor = Vector2.right * (Mathf.Sin (Time.time * period) * distance) / 2;
		bodyPart.MovePosition(movablePart.position += Vector3.right * Mathf.Sin (Time.time * period) * distance * Time.deltaTime);
	}

}
