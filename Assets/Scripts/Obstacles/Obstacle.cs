using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[DisallowMultipleComponent]
public class Obstacle : MonoBehaviour {
	public float dammage = 1;
	[EnumFlag("Organ to Dammage")]
	public Organs organToDammage;

	void OnCollisionStay2D(Collision2D other){
		Player player = other.gameObject.GetComponent<Player> ();
		if (player != null) {
			foreach (Organs organ in System.Enum.GetValues(typeof(Organs))) {
				if((organToDammage & organ) > 0)
					player.DammageOrgan(organ, dammage * Time.deltaTime);
			}
		}
	}


}