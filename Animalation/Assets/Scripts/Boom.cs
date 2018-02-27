using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {

    public float radius = 1;
    private CircleCollider2D collider;
    private Animator anim;
    private float maxRadius;
    private float timeMax;
    private float timeProcess;
    AnimatorClipInfo[] animClip;
    AnimatorStateInfo animState;
    public bool isBooming;

    public float boomUpTime;
    public Vector2 boomDamageTime;

    // Use this for initialization
    void Awake() {
        collider = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        animState = anim.GetCurrentAnimatorStateInfo(0);
        timeMax = animState.length;
        animClip = anim.GetCurrentAnimatorClipInfo(0);

        makeBoom();
    }

    private void Update()
    {
        if (!isBooming)
            return;

        boomUpTime += Time.deltaTime;
        if(boomUpTime >= boomDamageTime.x && boomUpTime < boomDamageTime.y)
        {
            collider.enabled = true;
        } else
        {
            collider.enabled = false;
        }
    }

    private void makeBoom()
    {
        isBooming = true;
        transform.localScale = Vector3.one * radius;
        collider.enabled = false;
        Destroy(gameObject, timeMax);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Destructable")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            MatchRuntime.Instance.newActivePlayer();
        }
    }
}
