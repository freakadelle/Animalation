using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRuntime : MonoBehaviour
{

    public static MatchRuntime Instance;
    public int actualRound;
    public float playTime;

    public Transform playersWrapper;

    public MatchRuntimeStates activeState;
    public GameObject[] players_teamRed;
    public GameObject[] players_teamBlue;
    public List<Slot> slots_teamRed;
    public List<Slot> slots_teamBlue;
    public Transform slotsWrapper;
    public BoomBall boomBall;
    private Transform downBoomGoal;
    public GameObject flag;
    public float minDownBoomDistance;
    private bool isValidDownBoom;

    public Teams teamsTurn;

    private bool isMapGernerated;

    public bool automaticStateSwitch = false;

    public float prepareTime;

    public GameObject activePlayer;

    private void Awake()
    {
        Instance = this;
        slots_teamBlue = new List<Slot>();
        slots_teamRed = new List<Slot>();
        downBoomGoal = transform.Find("DownBoomGoal");
        slotsWrapper = transform.Find("teamSlots");
    }

    // Use this for initialization
    void Start()
    {
        activeState = 0;
        //Camera.main.GetComponent<CameraMovement>().target = LevelGenerator.Instance.flagPoint.transform;
        setState(MatchRuntimeStates.none);

        teamsTurn = Teams.blue;

        if(automaticStateSwitch)
        {
            setState(MatchRuntimeStates.initMatch, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //setState(MatchRuntimeStates.initPlayers);
            StopAllCoroutines();
            GameUI_Canvas.Instance.countDown.OnTimerStopped.RemoveAllListeners();
            nextState();
        }

        if (Input.GetKeyDown(KeyCode.R) && activePlayer != null)
        {
            if (boomBall == null)
            {
                boomBall = GameStatics.Instance.spawnBoomBall(activePlayer.transform.position);
            }

            activePlayer.GetComponent<BombingActionMoves>().catchBall(boomBall);
        }

        if(activeState == MatchRuntimeStates.matchRunning)
        {
            playTime += Time.deltaTime;
            GameUI_Canvas.Instance.matchTimer.text = playTime.ToString("0.00");
        }
    }

    private void initMatch()
    {
        if(!isMapGernerated)
        {
            LevelGenerator.Instance.GenerateLevel();
            flag = LevelGenerator.Instance.flagPoint;
            isMapGernerated = true;
        }

        clearMatch();
        initSlots();

        Camera.main.GetComponent<CameraMovement>().cameraZoom = 25;
        Camera.main.GetComponent<CameraMovement>().target = LevelGenerator.Instance.flagPoint.transform;

        if(automaticStateSwitch)
        {
            nextState(2);
        }
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
        clearMatch();

        setTeamTurn(teamsTurn);

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

        Camera.main.GetComponent<CameraMovement>().cameraZoom = 75;

        if(automaticStateSwitch)
        {
            nextState(2);
        }
    }

    private void setActivePlayer(GameObject _player)
    {
        activePlayer = _player;
        Camera.main.GetComponent<CameraMovement>().cameraZoom = 35;
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

    private void nextState(float _delay = 0)
    {
        
        if ((int)activeState >= Enum.GetNames(typeof(MatchRuntimeStates)).Length -1)
        {
            StartCoroutine(StateDelay(MatchRuntimeStates.initMatch, _delay));
        } else
        {
            StartCoroutine(StateDelay(activeState + 1, _delay));
        }        
    }

    private void setState(MatchRuntimeStates _state, float _delay = 0)
    {
        StartCoroutine(StateDelay(_state, _delay));
    }

    IEnumerator StateDelay(MatchRuntimeStates _state, float delay)
    {
        yield return new WaitForSeconds(delay);
        ActiveState = _state;
    }

    private GameObject spawnPlayer()
    {
        GameObject _player = Instantiate(GameStatics.Instance.playerObj) as GameObject;
        _player.transform.parent = transform;
        return _player;
    }

    private GameObject spawnPlayer(Transform _trans, Transform _parent = null)
    {
        GameObject _player = Instantiate(GameStatics.Instance.playerObj) as GameObject;
        _player.transform.position = _trans.position;
        _player.transform.parent = transform;
        return _player;
    }

    private void setPlayerToSlot(Transform _player, Transform _slot)
    {
        _player.position = _slot.position;
    }

    private void initSlots()
    {
        slotsWrapper.position = flag.transform.position;
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
            slot.transform.parent = slotsWrapper;
        }
    }

    private void OnDownBoom(GameObject _player)
    {
        downBoomGoal.position = _player.transform.position;
        isValidDownBoom = checkDownBoomValidity(_player.GetComponent<PlayerCharacter>(), downBoomGoal.position);
        setState(MatchRuntimeStates.downBoomed);
    }

    private void prepareTeams()
    {

        if(teamsTurn == Teams.red)
        {
            setActivePlayer(players_teamRed[0]);
        } else
        {
            setActivePlayer(players_teamBlue[0]);
        }

        if (boomBall == null)
        {
            boomBall = GameStatics.Instance.spawnBoomBall(activePlayer.transform.position);
        }
        activePlayer.GetComponent<BombingActionMoves>().catchBall(boomBall);

        //Camera.main.GetComponent<CameraMovement>().target = players_teamRed[0].transform;
        GameUI_Canvas.Instance.countDown.OnTimerStopped.AddListener(delegate { nextState(); });
        GameUI_Canvas.Instance.countDown.startTimer(prepareTime);
    }

    private void doDownBoomStuff()
    {
        LevelGenerator.Instance.flagPoint.transform.position = downBoomGoal.position;

        Camera.main.GetComponent<CameraMovement>().target = downBoomGoal;
        Camera.main.GetComponent<CameraMovement>().cameraZoom = 15;

        if (!isValidDownBoom)
        {
            teamsTurn = teamTurnChanged();
        }
    }

    private Teams teamTurnChanged()
    {
        if(teamsTurn == Teams.red)
        {
            teamsTurn = Teams.blue;
        } else
        {
            teamsTurn = Teams.red;
        }
        setTeamFlag();
        return teamsTurn;
    }

    private void setTeamFlag()
    {
        if (teamsTurn == Teams.red)
        {
            flag.GetComponentInChildren<Animator>().SetBool("isRedTeamsTurn", true);
        }
        else
        {
            flag.GetComponentInChildren<Animator>().SetBool("isRedTeamsTurn", false);
        }
    }

    private Teams setTeamTurn(Teams _team)
    {
        teamsTurn = _team;
        setTeamFlag();
        return teamsTurn;
    }

    private bool checkDownBoomValidity(PlayerCharacter _player, Vector3 _downBoomPos)
    {
        float distanceFlagGoal = (downBoomGoal.position.x - flag.transform.position.x);
        Debug.Log(teamsTurn + " " + _player.team + " " + distanceFlagGoal);

        if (_player.team == teamsTurn && teamsTurn == Teams.red && distanceFlagGoal <= -minDownBoomDistance)
        {
            return true;
        } else if (_player.team == teamsTurn && teamsTurn == Teams.blue && distanceFlagGoal >= minDownBoomDistance)
        {
            return true;
        }
        return false;
    }

    public MatchRuntimeStates ActiveState
    {
        get
        {
            return activeState;
        }

        set
        {
            activeState = value;

            switch (activeState)
            {
                case MatchRuntimeStates.initMatch:
                    initMatch();
                    break;
                case MatchRuntimeStates.initPlayers:
                    initPlayers();
                    break;
                case MatchRuntimeStates.teamsPrepare:
                    prepareTeams();
                    break;
                case MatchRuntimeStates.matchRunning:
                    break;
                case MatchRuntimeStates.downBoomed:
                    doDownBoomStuff();
                    break;
            }
        }
    }

}

public enum MatchRuntimeStates
{
    none,
    initMatch,
    initPlayers,
    teamsPrepare,
    //matchup,
    matchRunning,
    downBoomed,
    //matchEnded
}

public enum Teams
{
    red,
    blue
}
