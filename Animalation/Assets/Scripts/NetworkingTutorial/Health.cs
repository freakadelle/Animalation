using UnityEngine;
using UnityEngine.Networking;
public class Health : NetworkBehaviour 
{
    public const int maxHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public bool destroyOnDeath;

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			} 
			else
			{
				currentHealth = maxHealth;

				// called on the Server, but invoked on the Clients
				RpcRespawn();
			}
		}
    }

	void OnChangeHealth (int currentHealth )
    {
		Debug.Log("hp: " + currentHealth);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }
}
