using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum characterRace{
	hogs,
	rats,
	pings
}

public enum characterClass{
	junkie,
	rocket,
	dozer
}


public class CharacterSelection : MonoBehaviour {
	public GameObject[] playerCharactersChooseable;

	public GameObject GetRandomRaceOfClassCharacter(characterClass _class) {
		int max = Enum.GetValues(typeof(characterRace)).Length;
		int rng = (int)UnityEngine.Random.Range(0, max);
		foreach(GameObject chara in playerCharactersChooseable) {
			if((int)chara.GetComponent<PlayerCharacter>().cRace == rng && chara.GetComponent<PlayerCharacter>().cClass == _class) {
				return chara;
			}
		}
		return null;
	}

	public GameObject GetRandomClassOfRaceCharacter(characterRace _race)
	{
		int max = Enum.GetValues(typeof(characterClass)).Length;
		int rng = (int)UnityEngine.Random.Range(0, max);
		foreach(GameObject chara in playerCharactersChooseable) {
			if(chara.GetComponent<PlayerCharacter>().cRace == _race && (int)chara.GetComponent<PlayerCharacter>().cClass == rng) {
				return chara;
			}
		}
		return null;
	}

	public GameObject GetRandomCharacter() {
		int rng = (int)UnityEngine.Random.Range(0, playerCharactersChooseable.Length);
		Debug.Log(playerCharactersChooseable[rng]);
		return playerCharactersChooseable[rng];
		// RpcCreatePlayerCharacter(_oldPlayer, rng, new Vector2(_connectionId, 0), Quaternion.identity);
	}

	public int GetRandom() {
		return (int)UnityEngine.Random.Range(0, playerCharactersChooseable.Length);
	}

	public GameObject GetTargetCharacter(int _target) {
		return playerCharactersChooseable[_target];
	}
}

/*
public class CharacterSelection : NetworkBehaviour {
	public GameObject[] playerCharactersChooseable; // Contains all spawnable characters

	public float testTimer = 10f;

	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start () {
		Invoke("FindAllConnectedPlayers", 0.5f);
	}

	void FindAllConnectedPlayers() {
		int index = 0;
		foreach(NetworkConnection conn in NetworkServer.connections) {
			Debug.Log(index);
			Debug.Log(conn);
			Debug.Log(conn.hostId);
			Debug.Log(conn.connectionId);
			//RpcAssignRandomCharacter(conn);
			index++;
		}
		// RpcClearPlayerSelectionAll();
		// RpcAllRandowmSpawn();
	}

	// Update is called once per frame
	void Update () {

	}

	[ClientRpc]
	void RpcClearPlayerSelectionAll() {
		for(int i = 0; i < players.Length; i++) {
			 RpcClearSelection(i);
		}
	}

	[ClientRpc]
	void RpcClearSelection(int playerControl) {
		if(players[playerControl].transform.childCount > 0) {
			players[playerControl].transform.GetChild(0).gameObject.SetActive(false);
			//Network.Destroy(players[playerControl].transform.GetChild(0).gameObject);
			Debug.Log("Disabled/Destroyed one child");
		}
	}

	[ClientRpc]
	void RpcAllRandowmSpawn() {
		foreach(NetworkConnection conn in NetworkServer.connections) {
			RpcAssignRandomCharacter(conn.connectionId);
		}
	}

	public GameObject GetRandomCharacter() {
		int rng = (int)Random.Range((int)0, playerCharactersChooseable.Length);
		return playerCharactersChooseable[rng];
		// RpcCreatePlayerCharacter(_oldPlayer, rng, new Vector2(_connectionId, 0), Quaternion.identity);
	}

	[ClientRpc]
	void RpcCreatePlayerCharacter(PlayerBrain _oldPlayer, int _charIndex, Vector3 _pos, Quaternion _rot) {
		NetworkConnection conn = _oldPlayer.connectionToClient;
		GameObject newChar = (GameObject)GameObject.Instantiate(playerCharactersChooseable[_charIndex], _pos, _rot);
		NetworkServer.Spawn(newChar);
		NetworkServer.ReplacePlayerForConnection(conn, newChar, 0);
		//newChar.transform.parent = players[_playerControl].transform;
	}
}
*/