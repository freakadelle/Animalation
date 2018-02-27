using UnityEngine.Networking;
using UnityEngine;

public class characterPlayerTest : NetworkBehaviour
{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

    void Update()
    {
		if (!isLocalPlayer)
		return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        transform.Translate(x, 0, 0);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
    }
	
	public override void OnStartLocalPlayer()
	{
		transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = Color.blue;
	}

[Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        // Add velocity to the bullet
        Vector2 _vec = new Vector2(10f, 0f);
        bullet.GetComponent<Rigidbody2D>().velocity = _vec;
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
	
}