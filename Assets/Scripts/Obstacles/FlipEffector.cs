using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaEffector2D), typeof(BoxCollider2D))]
public class FlipEffector : MonoBehaviour {

	public LayerMask maskToFlip;
	private AreaEffector2D effector;
	[Range(1,20)]
	public float magnitude;

	void Start(){
		effector = GetComponent<AreaEffector2D> ();
		effector.forceMagnitude = magnitude;
	}

	void OnTriggerExit2D(Collider2D other){
		if (((1 << other.gameObject.layer) & maskToFlip) > 0) {
			effector.forceMagnitude *= -1;
		}
	}
}
