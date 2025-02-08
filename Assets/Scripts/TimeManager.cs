using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }
    public UnityEvent OnDayPass = new UnityEvent();
    public UnityEvent<int> OnHourPass = new UnityEvent<int>();//8/2/duyenduyen
    public enum Season { Spring,Summer,Fall,Winter}
    public Season currentSeason = Season.Spring;

    private int daysPerSeason = 30;
    private int daysInCurrentSeason = 1;

    public enum DayOfWeek { Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday}
    public DayOfWeek currentDayOfWeed = DayOfWeek.Monday;

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
    public int yearInGame = 0;
    public TextMeshProUGUI dayUI;
    private void Start()
    {
        UpdateUI();
    }

    

    public void TriggerNextDay()
    {
        dayInGame += 1;
        daysInCurrentSeason += 1;

        currentDayOfWeed = (DayOfWeek)(((int)currentDayOfWeed + 1) % 7);

        if(daysInCurrentSeason > daysPerSeason)
        {
            // Switch to next season
            daysInCurrentSeason = 1;
            currentSeason = GetNextSeason();
        }

        UpdateUI();

        OnDayPass.Invoke();
    }

    private Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeason;
        int nextSeasonIndex = (currentSeasonIndex + 1) % 4;

        //
        if (nextSeasonIndex == 0)
        {
            yearInGame += 1;
        }

        return (Season)nextSeasonIndex;
    }

    private void UpdateUI()
    {
        dayUI.text = $"{currentDayOfWeed} {daysInCurrentSeason}, {currentSeason}";
    }
}
