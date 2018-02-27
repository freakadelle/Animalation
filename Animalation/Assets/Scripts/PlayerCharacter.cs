using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum team {
	Team_None,
	TeamBlue_Slot,
	TeamRed_Slot
}

public class PlayerCharacter : NetworkBehaviour {
	public characterRace cRace;
	public characterClass cClass;
	public team teamTag;
    public Teams team;

	public CharacterMovements cm; // Can be different for character
	public BombingActionMoves thr;
	//public CharacterGraphics cg;

	void SelectTeam(team _newTeam) {
		teamTag = _newTeam;
		transform.gameObject.tag = teamTag.ToString();
	}

	// Use this for initialization
	void Start () {
		/* 
			cm = this.transform.GetChild(0).GetComponent<CharacterMovements>();
			cg = this.transform.GetChild(0).GetComponent<CharacterGraphics>();
			thr = this.transform.GetChild(0).GetComponent<Throwing>();
		*/
		cm = transform.GetComponent<CharacterMovements>();
		thr = transform.GetComponent<BombingActionMoves>();
		//cg = transform.GetComponent<CharacterGraphics>();
	}

	// Can also be sent via server?	
	[Command]
	public void CmdCalculateInput(float _x, bool _jumpDown, bool _jumpUp, bool _skill1Down, bool _skill1Up, bool _skill2Up, bool _skill3) {
		cm.RpcRecieveInputs(_x, _jumpDown, _jumpUp);
		thr.RpcRecieveInputs(_skill1Down, _skill1Up, _skill2Up);
	}

	// If disabled, destroy.
	void OnDisable() {
		Destroy(this.gameObject);
	}
}
