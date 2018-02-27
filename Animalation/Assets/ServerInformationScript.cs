using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerInformationScript : NetworkBehaviour {
	// Use this for initialization
	public CharacterSelection cs;

	void Start () {
		Invoke("FindAllConnectedPlayers", 1.5f);
	}

	void FindAllConnectedPlayers() {
		int index = 0;
		foreach(NetworkConnection conn in NetworkServer.connections) {
			Debug.Log(index);
			Debug.Log(conn);
			Debug.Log(conn.hostId);
			Debug.Log(conn.connectionId);
			index++;
		}
	}

	/* 
	[ClientRpc]
	public void RpcSpawnPlayer(GameObject _origin) {
		GameObject _target = cs.GetRandomCharacter();
		Debug.Log(_target);
        GameObject newChar = Instantiate(_target, transform.position, Quaternion.identity) as GameObject;
		Debug.Log(newChar);
		NetworkServer.Spawn(newChar);
		NetwerkServer.
		NetworkServer.SpawnWithClientAuthority(newChar, _origin.gameObject);
		_origin.GetComponent<PlayerDummy>().playerChar = newChar;
	}
	*/
}
