using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }
    public UnityEvent OnDayPass = new UnityEvent();
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public Season currentSeason = Season.Spring;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }
    public int dayInGame = 1;
    public TextMeshProUGUI dayUI;
    private void Start()
    {
        dayUI.text = $"Day: {dayInGame}";
    }

    

    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"Day: {dayInGame}";

        OnDayPass.Invoke();
    }
}
