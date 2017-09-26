using UnityEngine;

public class CameraFollow : MonoBehaviour {

	Transform targetToFollow;
	Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		targetToFollow = GameObject.FindObjectOfType<Player> ().transform;
	}

	void Update(){
		mainCamera.transform.position = targetToFollow.position + (Vector3.back * 10);
	}
}
