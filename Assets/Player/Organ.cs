using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Organ {

	public string name;
	public Organs organ;
	private float maxHealth, health;

	public bool GetViability(float viabilityTarget = 60){
		return GetViabilityPercentage () >= viabilityTarget;
	}

	public float GetViabilityPercentage(){
		return Mathf.Round((health / maxHealth) * 100);
		//return (health / maxHealth) * 100;
	}

	public void DammageOrgan(float dmg){
		health = Mathf.Clamp ((health - dmg), 0, maxHealth);
		UpdateViabilityText ();
	}

	public Organ(Organs _organ, string _name, float _maxHealth){
		organ = _organ;
		name = _name;
		maxHealth = _maxHealth;
	}

	public void PrepareOrgan(){
		health = maxHealth;
		UpdateViabilityText ();
	}

	public void UpdateViabilityText(){
		CanvasManager.instance.SetOrganViability (organ, GetViabilityPercentage ());
	}

}
