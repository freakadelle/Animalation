using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {


    public Teams team;

    private void Awake()
    {
        if (gameObject.name.Contains("Red"))
        {
            team = Teams.red;
        } else
        {
            team = Teams.blue;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
