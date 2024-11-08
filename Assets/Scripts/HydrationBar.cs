using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public Text hydrationCounter;

    public GameObject playerState;

    private float currentHydrationPercent, maxHydrationPercent;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentHydrationPercent = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydrationPercent = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currentHydrationPercent / maxHydrationPercent; //0 - 1
        slider.value = fillValue;

        hydrationCounter.text = currentHydrationPercent + "%";
    }
}
