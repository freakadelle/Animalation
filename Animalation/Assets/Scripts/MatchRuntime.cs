using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRuntime : MonoBehaviour
{

    public static MatchRuntime Instance;
    public int roundPrepareTime;

    public Transform playersWrapper;

    public MatchRuntimeStates activeState;
    public GameObject[] players_teamRed;
    public GameObject[] players_teamBlue;
    public List<Slot> slots_teamRed;
    public List<Slot> slots_teamBlue;

    public GameObject activePlayer;

    private void Awake()
    {
        Instance = this;
        slots_teamBlue = new List<Slot>();
        slots_teamRed = new List<Slot>();

        
    }

    // Use this for initialization
    void Start()
    {
        activeState = 0;
        //Camera.main.GetComponent<CameraMovement>().target = LevelGenerator.Instance.flagPoint.transform;
        Invoke("initMatch", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            initMatch();
        }

        if (Input.GetKeyDown(KeyCode.R) && activePlayer != null)
        {
            if (BoomBall.Instance == null)
            {
                GameStatics.Instance.spawnBoomBall(activePlayer.transform.position);
            }

            activePlayer.GetComponent<BombingActionMoves>().catchBall(BoomBall.Instance);
        }
    }

    private void initMatch()
    {

        activeState = MatchRuntimeStates.initMatch;
        clearMatch();
        initSlots();
        initPlayers(true);

        nextState();

        setActivePlayer(players_teamRed[0]);

        //Camera.main.GetComponent<CameraMovement>().target = players_teamRed[0].transform;
        CanvasView_GameUI.Instance.countDown.OnTimerStopped.AddListener(delegate { nextState(); });
        CanvasView_GameUI.Instance.countDown.startTimer(10);
    }

    private void clearMatch()
    {
        foreach (GameObject _player in players_teamBlue)
        {
            Destroy(_player);
        }
        foreach (GameObject _player in players_teamRed)
        {
            Destroy(_player);
        }
    }

    private void initPlayers(bool _newOnes = false)
    {
        //Set Blue palyers
        for (int i = 0; i < players_teamBlue.Length; i++)
        {
            players_teamBlue[i] = spawnPlayer();
            players_teamBlue[i].GetComponent<PlayerCharacter>().team = Teams.blue;
            setPlayerToSlot(players_teamBlue[i].transform, slots_teamBlue[slots_teamBlue.Count - 1 - i].transform);
        }

        //Set Red palyers
        for (int i = 0; i < players_teamRed.Length; i++)
        {
            players_teamRed[i] = spawnPlayer();
            players_teamRed[i].GetComponent<PlayerCharacter>().team = Teams.red;
            setPlayerToSlot(players_teamRed[i].transform, slots_teamRed[i].transform);
        }

        
    }

    private void setActivePlayer(GameObject _player)
    {
        activePlayer = _player;
        Camera.main.GetComponent<CameraMovement>().target = activePlayer.transform;
    }

    public void newActivePlayer()
    {
        foreach(GameObject go in players_teamBlue)
        {
            if(go != null)
            {
                activePlayer = go;
            }
        }

        foreach (GameObject go in players_teamRed)
        {
            if (go != null)
            {
                activePlayer = go;
            }
        }
    }

    private void nextState()
    {
        CanvasView_GameUI.Instance.countDown.OnTimerStopped.RemoveAllListeners();
        activeState++;
    }

    private void setState(int _stateID)
    {
        activeState = (MatchRuntimeStates)_stateID;
    }

    private void setState(MatchRuntimeStates _state)
    {
        activeState = _state;
    }

    private GameObject spawnPlayer()
    {
        GameObject _player = Instantiate(GameStatics.Instance.playerObj) as GameObject;
        return _player;
    }

    private GameObject spawnPlayer(Transform _trans, Transform _parent = null)
    {
        GameObject _player = Instantiate(GameStatics.Instance.playerObj) as GameObject;
        _player.transform.position = _trans.position;
        //Add parentisation
        return _player;
    }

    private void setPlayerToSlot(Transform _player, Transform _slot)
    {
        _player.position = _slot.position;
    }

    private void initSlots()
    {
        Slot[] playerSlots = LevelGenerator.Instance.getAllTilesWithComponents<Slot>();

        foreach (Slot slot in playerSlots)
        {
            switch (slot.team)
            {
                case Teams.red:
                    slots_teamRed.Add(slot);
                    break;
                case Teams.blue:
                    slots_teamBlue.Add(slot);
                    break;
            }
        }
    }

}

public enum MatchRuntimeStates
{
    initMatch,
    teamsPrepare,
    matchup,
    matchRunning,
    matchEnded
}

public enum Teams
{
    red,
    blue
}
