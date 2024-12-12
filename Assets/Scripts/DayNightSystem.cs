using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24f;
    public int currentHour;
    float currentTimeOfDay = 0.35f;

    public List<SkyBoxTimeMapping> timeMappings;

    float blenderValue = 0.0f;

    bool lockNextDayTrigger = false;


    [Header("UI Day Time")]
    public TextMeshProUGUI timeUI;

    public WeatherSystem weatherSystem;

    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        if (weatherSystem.isSpecialWeather == false)
        {
            UpdateSkybox();
        }

        if(currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if(currentHour != 0)
        {
            lockNextDayTrigger = false;
        }
    }

    private void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach(SkyBoxTimeMapping mapping in timeMappings)
        {
            if(currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterials;

                if(currentSkybox.shader != null)
                {
                    if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blenderValue += Time.deltaTime;
                        blenderValue = Mathf.Clamp01(blenderValue);

                        currentSkybox.SetFloat("_TransitionFactor",blenderValue);
                    }

                    else
                    {
                        blenderValue = 0.0f;
                    }
                }

                break;
            }
        }

        
        if(currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
    }
}

[System.Serializable]
public class SkyBoxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterials;
}
