using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDown : MonoBehaviour {

    public UnityEvent OnTimerStopped;
    private Text numberOne;
    public bool isStarted;
    public float time;
    //public float actualTime;

	// Use this for initialization
	void Awake () {
        if (OnTimerStopped == null)
            OnTimerStopped = new UnityEvent();

        numberOne = transform.GetComponentsInChildren<Text>()[0];
        numberOne.enabled = isStarted;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isStarted)
            return;

        if(time > 1)
        {
            numberOne.enabled = isStarted;
            time -= Time.deltaTime;
            numberOne.text = ((int) time).ToString();
        } else
        {
            stopTimer();
        }
	}

    public void startTimer(float _max)
    {
        time = _max + 1;
        isStarted = true;
        numberOne.enabled = isStarted;
    }

    public void stopTimer()
    {
        isStarted = false;
        numberOne.enabled = isStarted;
        time = 0;
        OnTimerStopped.Invoke();
    }

    public void pauseTimer()
    {
        isStarted = false;
    }


}
