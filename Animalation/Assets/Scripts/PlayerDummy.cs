using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDummy : NetworkBehaviour {
	public characterRace cRace;
	public characterClass cClass;
	public team cTeam;

	public CharacterSelection cs;
	public ServerInformationScript sis;
	public NetworkIdentity nd;
	public GameObject playerChar;
	public float introTimer = 1.5f;

	// Use this for initialization
	void Start () {
		Invoke("FindCharSel", 1f);
		Invoke("CreateChar", 2f);
	}

	void FindCharSel() {
		cs = GameObject.FindGameObjectWithTag("CharacterSelection").GetComponent<CharacterSelection>();
		nd = GetComponent<NetworkIdentity>();
		//sis = GameObject.FindGameObjectWithTag("ServerInf").GetComponent<ServerInformationScript>();
		//Debug.Log(sis);
	}

	void Update() {
        if (!isLocalPlayer || !hasAuthority)
        {
			if(isLocalPlayer)
			Debug.Log("Local");
			if(hasAuthority)
			Debug.Log("Authority");

			Debug.Log("Not your dummie");
            return;
        }

	}

	public void CreateChar() {
		CmdSpawnTargetForPlayer(cs.GetRandom());
		//CmdServerSpawnTargetPlayer();
	}

	public void OnDeath() {
		playerChar = null;
	}

	[Command]
	public void CmdServerSpawnTargetPlayer() {
        //sis.RpcSpawnPlayer(this.gameObject);
		/*
        GameObject newChar = Instantiate(_target, transform.position, Quaternion.identity) as GameObject;
        NetworkServer.SpawnWithClientAuthority(newChar, this.gameObject);
		playerChar = newChar;
		*/
	}

	[Command]
    public void CmdSpawnTargetForPlayer(int _targetChara)
    {
		GameObject charNew = cs.GetTargetCharacter(_targetChara);
        GameObject charInstantianted = Instantiate(charNew, transform.position, Quaternion.identity) as GameObject;
		Debug.Log(charInstantianted);
		Debug.Log(nd.connectionToClient);

		/*
		NetworkServer.Spawn(charInstantianted);
		charInstantianted.GetComponent<NetworkIdentity>().AssignClientAuthority(nd.connectionToClient);
		*/

        // NetworkServer.SpawnWithClientAuthority(charInstantianted, nd.connectionToClient);

		NetworkServer.AddPlayerForConnection(nd.connectionToClient, charInstantianted, 0);

		playerChar = charInstantianted;
		Debug.Log(playerChar);
    }
}