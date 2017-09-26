using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

	public HingeJoint2D baseJoint;
	public HingeJoint2D ball;
	public GameObject chainLinkPrefab;

	[Range(4,100)]
	public int chainLinks = 10;
	[Range(0.1f, 1)]
	public float linkDistance = 0.2f;

	private Rigidbody2D[] chainLink;

	void Start () {
		ball.autoConfigureConnectedAnchor = false;
		chainLink = new Rigidbody2D[chainLinks+1];
		chainLink [0] = baseJoint.attachedRigidbody;
		CreateChain ();
		ball.connectedBody = chainLink [chainLinks];
		ball.connectedAnchor = Vector2.up * linkDistance;
		//Don'n even ask why, just worked this way
		ball.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * 800);
	}
	
	void CreateChain(){
		for (int i = 1; i <= chainLinks; i++) {
			GameObject zelda = Instantiate (chainLinkPrefab, transform); // Link is too mainstream =P
			zelda.transform.localPosition = chainLink[i-1].transform.localPosition + (Vector3.down * linkDistance);
			zelda.name = "ChainLink"+i.ToString();
			HingeJoint2D triforce = zelda.GetComponent<HingeJoint2D>(); // Let's steal the Triforce of wisdom for this to work!
			triforce.connectedBody = chainLink[i-1]; // Connect the triforce to previous hero of time
			triforce.autoConfigureConnectedAnchor = false;
			triforce.connectedAnchor = Vector2.down * linkDistance;
			chainLink [i] = triforce.GetComponent<Rigidbody2D> ();
		}
	}
}
