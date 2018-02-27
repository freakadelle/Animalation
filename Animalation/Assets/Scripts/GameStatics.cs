using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatics : MonoBehaviour {

    public static GameStatics Instance;
    public float GRAVITY = 9.81f;
    public Object explosionPrefab;
    public Object boomBallObj;
    public Object playerObj;

    public LayerMask groundLayerMask;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        explosionPrefab = Resources.Load("Prefabs/BombingExplosion");
        boomBallObj = Resources.Load("Prefabs/BombBall");
        playerObj = Resources.Load("Prefabs/Player");
    }

    public void spawnExplosion(Vector3 _pos)
    {
        GameObject explosion = Instantiate(explosionPrefab) as GameObject;
        explosion.transform.position = _pos;
    }

    public BoomBall spawnBoomBall(Vector3 _pos)
    {
        GameObject boomBall = Instantiate(boomBallObj) as GameObject;
        boomBall.transform.position = _pos;
        boomBall.transform.parent = MatchRuntime.Instance.transform;
        return boomBall.GetComponent<BoomBall>();
    }
}
