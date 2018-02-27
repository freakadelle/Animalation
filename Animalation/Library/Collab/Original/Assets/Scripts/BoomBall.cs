using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBall : MonoBehaviour {

    public static BoomBall Instance;

    private Collider2D collider;
    private Animator animator;
    public bool isCarried;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        isCarried = true;

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 moveVel = GetComponent<Rigidbody2D>().velocity;
        float rot_z = Mathf.Atan2(moveVel.y, moveVel.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    public void triggerThisJealousBoom()
    {
        GameStatics.Instance.spawnExplosion(transform.position);

        Camera.main.GetComponent<CameraMovement>().target = MatchRuntime.Instance.activePlayer.transform;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isCarried)
        {
            triggerThisJealousBoom();
        }
    }
}
