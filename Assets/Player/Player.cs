using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[Header("Motion Configuration")]
	[Range(1,10)]
	public float speed = 1;
	[Range(1.2f,3)]
	public float runMultiplyer = 1.2f;

	[Header("Stamina Configuration")]
	[Range(10, 100)]
	public float maxStamina;
	private float stamina;
	[Range(0.5f, 3f)]
	public float staminaConsuptionSpeed, staminaRecoverySpeed = 0.1f;


	[Header("Jump Configurations")]
	[Range(200, 1000)]
	public float jumpSpeed = 200;
	[Range(2,5)]
	public float fallMultiplyer = 2.5f;
	[Range(2,5)]
	public float lowJumpMultiplyer = 2f;
	[Tooltip("Reference to the position detector of the ground contact")]
	public Transform groundChecker;
	public LayerMask jumpableMask;
	[Tooltip("The size of the range of detection to player's foot to ground")]
	[Range(0.01f, 0.4f)]
	public float groundCheckerRadius = 0.1f;
	private bool isGrounded;

	[Header("Organ Configurations")]
	public float organMaxHealth = 100;
	private float maxHealth = 0;
	[Range(1,100)]
	public float defaultViability = 60;
	public LayerMask lungDammagers;
	[Range(0.1f, 5f)]
	public float lungRange = 0.1f;
	[Range(0.1f, 5f)]
	public float lungDefaultDammage = 0.5f;
	public Transform lungTrigger;
	[Range(0.5f, 5)]
	public float lungDammageOnLowStamina, lungDammageOnUnderwater = 1;
	private bool underWater = false;
	private Dictionary<Organs, Organ> organs;

	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		organs = new Dictionary<Organs, Organ> ();
		foreach(Organs organ in Enum.GetValues(typeof(Organs))){
			organs.Add (organ, new Organ (organ, Enum.GetName (typeof(Organs), organ), organMaxHealth));
			CanvasManager.instance.CreateOrganText (organ);
		}
		PrepareOrgans ();
		stamina = maxStamina;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Run"))
			transform.Translate (Vector2.right * Input.GetAxis ("Horizontal") * speed * runMultiplyer * Time.deltaTime);
		else
			transform.Translate (Vector2.right * Input.GetAxis ("Horizontal") * speed * Time.deltaTime);
		underWater = Physics2D.OverlapCircle ((Vector2)lungTrigger.position, lungRange, lungDammagers);
		CalculateStamina (Input.GetButton ("Run") && Input.GetAxis("Horizontal")!= 0,underWater);
		if (Input.GetAxisRaw ("Horizontal") != 0)
			transform.localScale = new Vector3 (Input.GetAxisRaw ("Horizontal"), 1, 1);
		if (Input.GetButtonDown ("Jump") && isGrounded) {
			ApplyJump ();
		}
	}

	void FixedUpdate(){
		isGrounded = Physics2D.OverlapCircle (groundChecker.position, groundCheckerRadius, jumpableMask);
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplyer-1f) * Time.deltaTime;
		} else if(rb.velocity.y > 0 && !(Input.GetButton("Jump")|| Input.GetMouseButton(0))) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplyer-1f) * Time.deltaTime;
		}
	}

	public void CalculateStamina(bool running, bool underwater){
		if(running || underwater)
			stamina -= (staminaConsuptionSpeed * Time.deltaTime);
		else
			stamina += (staminaRecoverySpeed * Time.deltaTime);

		if (stamina <= 0 && underWater) {
			DammageOrgan (Organs.lungs, lungDammageOnUnderwater * Time.deltaTime);
		} else if (stamina <= 0) {
			DammageOrgan (Organs.lungs, lungDammageOnLowStamina * Time.deltaTime);
		}

		stamina = Mathf.Clamp (stamina, 0, maxStamina);
		CanvasManager.instance.staminaBar.value = stamina / maxStamina;
	}

	public void ApplyJump(bool yOverride = true){
		if (yOverride) {
			rb.velocity = new Vector2 (rb.velocity.x, 0);
		}
		rb.AddForce (Vector2.up * jumpSpeed);
	}

	//Organ functions
	private void PrepareOrgans(){
		foreach (Organ organ in organs.Values) {
			maxHealth += organMaxHealth;
			organ.PrepareOrgan ();
		}
	}

	private void CheckIfDied(){
		foreach (Organ organ in organs.Values) {
			if (organ.GetViabilityPercentage () <= 0) {
				KillPlayer ();
				return;
			}
		}
	}

	private void KillPlayer(bool goalReached = false){
		GameManager.instance.ShowFinalPanel (goalReached);
	}

	public void DammageOrgan(Organs organ, float dmg = 1){
		Organ organToDammage;
		if (organs.TryGetValue (organ, out organToDammage)) {
			organToDammage.DammageOrgan (dmg);
		}
		CheckIfDied ();
	}

	public float GetPercentageViability(Organs organ){
		Organ organToCheck;
		if (organs.TryGetValue (organ, out organToCheck)) {
			return organToCheck.GetViabilityPercentage();
		} else {
			return 0;
		}
	}

	public bool CheckOrganViability(Organs organ, float targetViability = 60){
		Organ organToCheck;
		if (organs.TryGetValue (organ, out organToCheck)) {
			return organToCheck.GetViability (targetViability);
		} else {
			return false;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Goal")){
			KillPlayer (true);
		}
	}
}

[Flags]
public enum Organs{
	heart 		= 1, 	//byte 00000001
	kidneys 	= 2, 	//byte 00000010
	liver 		= 4, 	//byte 00000100
	lungs 		= 8, 	//byte 00001000
	pancreas 	= 16, 	//byte 00010000
	intestine 	= 32 	//byte 00100000
}