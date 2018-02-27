using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingActionMoves : MonoBehaviour {

    public Rigidbody2D boomballRig;
    public bool hasBall;
    public Vector2 throwingDir;
    public float throwingPower;
    public float throwingPowerGain;
    public float throwingPowerMax;

    private bool isThrowHolding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Cancel if has no ball
        if (!hasBall || BoomBall.Instance == null)
            return;

        boomballRig.transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        if (Input.GetButtonDown("Fire1"))
        {
            isThrowHolding = true;
            throwingPower = 0;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isThrowHolding = false;
            throwBall();
            throwingPower = 0;
        }

        if(Input.GetButtonUp("Fire2"))
        {
            boomScoreNow();
        }

        if (isThrowHolding)
        {
            throwingPower += throwingPowerGain * Time.deltaTime;
            throwingPower = Mathf.Min(throwingPower, throwingPowerMax);
        }

    }

    private void boomScoreNow()
    {
        BoomBall.Instance.triggerThisJealousBoom();
    }

    private void throwBall()
    {
        boomballRig.gravityScale = 1;

        BoomBall.Instance.isCarried = false;

        Vector2 throwingDir = new Vector2(GetComponent<CharacterMovements>().isFacingRight ? -this.throwingDir.x : this.throwingDir.x, this.throwingDir.y / 2);
        boomballRig.AddForce((throwingDir * throwingPower) + new Vector2(GetComponent<Rigidbody2D>().velocity.x * 2, 0), ForceMode2D.Impulse);

        hasBall = false;
        this.throwingPower = 0;
    }

    public void catchBall(BoomBall _ball)
    {
        _ball.isCarried = true;
        boomballRig = _ball.gameObject.GetComponent<Rigidbody2D>();
        hasBall = true;

        //boomballRig.transform.parent = gameObject.transform;
        boomballRig.velocity = Vector3.zero;
        boomballRig.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boomball")
        {
            catchBall(collision.gameObject.GetComponent<BoomBall>());
        }
    }

}
