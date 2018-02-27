using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneSwitcher : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//Application.LoadLevel("HighScore");
		SceneManager.LoadScene(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
