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
        
	}

    public void triggerThisJealousBoom()
    {
        GameStatics.Instance.spawnExplosion(transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isCarried)
        {
            triggerThisJealousBoom();
        }
    }

    void OnDisable() {
        Destroy(this.gameObject);
    }
}