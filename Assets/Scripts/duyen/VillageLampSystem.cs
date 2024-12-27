using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageLampSystem : MonoBehaviour
{
    public int timeLampsTurnOn;
    public int timeLampsTurnOff;
    public bool lampsAreOn;

    public DayNightSystem DayNightSystem;

    private void Update()
    {
        if (DayNightSystem.currentHour == timeLampsTurnOn && lampsAreOn == false)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Light>().enabled = true;
            }
            lampsAreOn = true;
        }

        if (DayNightSystem.currentHour == timeLampsTurnOff && lampsAreOn == true)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Light>().enabled = false;
            }
            lampsAreOn = false;
        }
    }
}
