using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBrain : NetworkBehaviour
{
    public PlayerCharacter playerCharacter;
    public bool isOfflinePlayer;

	void OnStartAuthority() {
		Debug.Log("Si");
	}

    // Update is called once per frame
    void Update()
    {
        if ((!isLocalPlayer || !hasAuthority) && !isOfflinePlayer)
        {
			if(isLocalPlayer)
				Debug.Log("Local");
			if(hasAuthority)
				Debug.Log("Authority");

			Debug.Log("Not your dummie");
            return;
        }

		if(Input.GetKeyDown(KeyCode.T)) {
			transform.localScale = new Vector2(0.5f, 0.5f);
		} else if(Input.GetKeyDown(KeyCode.Z)) {
			CmdTest();
		} else if(Input.GetKeyDown(KeyCode.U)) {
			RpcTest();
		}

		playerCharacter = GetComponent<PlayerCharacter>();
		// pc = this.transform.GetChild(0).GetComponent<PlayerCharacter>();
		//Debug.Log("Not local, could process input");

        if(isOfflinePlayer)
        {
            playerCharacter.CalculateInput(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"), Input.GetButtonDown("Fire1"), Input.GetButtonUp("Fire1"), Input.GetButtonUp("Fire2"), Input.GetButton("Fire3"));
        } else
        {
            playerCharacter.CmdCalculateInput(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"), Input.GetButtonDown("Fire1"), Input.GetButtonUp("Fire1"), Input.GetButtonUp("Fire2"), Input.GetButton("Fire3"));
        }

		
	}

	[Command]
	void CmdTest() {
		transform.localScale = new Vector2(0.8f, 2.5f);
	}

	[ClientRpc]
	void RpcTest() {
		transform.localScale = new Vector2(2.8f, 0.5f);
	}
}