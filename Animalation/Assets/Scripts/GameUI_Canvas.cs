using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Canvas : MonoBehaviour {

    public static GameUI_Canvas Instance;
    public CountDown countDown;
    public Text matchTimer;

    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
