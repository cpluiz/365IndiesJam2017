using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	#region Singleton
	public static CanvasManager instance;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		if(this != instance)
			Destroy (gameObject);
	}
	#endregion
	[Header("Main canvas settings")]
	private Canvas canvas;
	public RectTransform menuPanel, inGamePanel;

	[Header("Organs Settings")]
	public Text organTextPrefab;
	public RectTransform organTextArea = null;
	public Slider staminaBar;

	[Header("End Game Settings")]
	public RectTransform endGamePanel;
	public Text endTitleText, endDescriptionText, endConclusionText;
	public string organSufix, percentageSufix;
	public string goalTitle, deathTitle;
	[TextArea]
	public string goalDescription, deathDescription;
	public string[] conclusionText;
		
	Dictionary<Organs,Text> organTexts = null;

	void PrepareCanvas(){
		if (organTexts != null)
			return;
		organTexts = new Dictionary<Organs, Text> ();
		canvas = GetComponentInChildren<Canvas> ();
	}

	//TODO use only one function to transitionate betwen panels
	public void ShowMainMenu(){
		inGamePanel.gameObject.SetActive (false);
		endGamePanel.gameObject.SetActive (false);
		menuPanel.gameObject.SetActive (true);
	}

	public void ShowLevelInterface(){
		inGamePanel.gameObject.SetActive (true);
		endGamePanel.gameObject.SetActive (false);
		menuPanel.gameObject.SetActive (false);
	}

	public void ShowEndGamePanel(bool goalReached, float viability, bool sucess){
		endGamePanel.gameObject.SetActive (true);
		inGamePanel.gameObject.SetActive (false);
		menuPanel.gameObject.SetActive (false);
		if (goalReached) {
			endTitleText.text = goalTitle;
			endDescriptionText.text = goalDescription;
		} else {
			endTitleText.text = deathTitle;
			endDescriptionText.text = deathDescription;
		}
		if (sucess)
			endConclusionText.text = conclusionText [0];
		else
			endConclusionText.text = conclusionText [1];
		endDescriptionText.text = endDescriptionText.text.Replace (organSufix, GameManager.instance.targetOrgan.ToString ()).Replace (percentageSufix, viability.ToString ());
	}

	public void SetMainCamera(Camera camera){
		PrepareCanvas ();
		canvas.worldCamera = camera;
	}

	public void CreateOrganText(Organs organ){
		Text organText = Instantiate (organTextPrefab, organTextArea) as Text;
		if (organTexts == null)
			PrepareCanvas ();
		organTexts.Add (organ, organText);
	}

	public void SetOrganViability(Organs organ, float percentage){
		Text textToUpdate;
		if (organTexts.TryGetValue (organ, out textToUpdate)) {
			textToUpdate.text = System.Enum.GetName (typeof(Organs), organ)+": " + percentage.ToString () + "%";
		}
	}

}
