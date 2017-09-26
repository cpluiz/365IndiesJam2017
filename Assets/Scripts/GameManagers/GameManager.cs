using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	#region Singleton
	public static GameManager instance;

	void Start(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		if(this != instance)
			Destroy (gameObject);
	}
	#endregion

	public CanvasManager canvasManager;

	public AudioClip menu, youLose, youWin;
	public AudioClip[] bgmMusic;

	public Organs targetOrgan;
	Player player;
	Camera mainCamera;
	AudioSource bgmSource;

	void Awake(){
		SceneManager.sceneLoaded += OnLoadScene;
	}

	void OnLoadScene(Scene scene, LoadSceneMode sceneMode){
		targetOrgan = GetRandomEnum<Organs> ();
		player = FindObjectOfType<Player> ();
		mainCamera = Camera.main;
		canvasManager = CanvasManager.instance;
		canvasManager.enabled = true;
		canvasManager.SetMainCamera (mainCamera);
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			canvasManager.ShowMainMenu ();
		} else {
			canvasManager.ShowLevelInterface ();
		}
	}

	public void ShowFinalPanel(bool goalReached){
		canvasManager.ShowEndGamePanel (goalReached, player.GetPercentageViability(targetOrgan), VerifyOrgan());
	}

	public bool VerifyOrgan(){
		bool value = player.CheckOrganViability (targetOrgan);
		return value;
	}

	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}

}

public static class POGTranslator{
	public static string POGTranslate(Organs organ){
		switch (organ) {
		case Organs.heart:
			return "coração";
		case Organs.intestine:
			return "intestino";
		case Organs.kidneys:
			return "rins";
		case Organs.liver:
			return "fígado";
		case Organs.lungs:
			return "pulmões";
		case Organs.pancreas:
			return "pâncreas";
		default:
			return "Deu ruim e caiu e um órgão que ainda não existe na lista";
		}
	}
}