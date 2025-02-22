using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherSystem : MonoBehaviour
{
    [Range(0f, 1f)] public float chanceToRainSpring = 0.3f;
    [Range(0f, 1f)] public float chanceToRainSummer = 0.00f;
    [Range(0f, 1f)] public float chanceToRainFall = 0.4f;
    [Range(0f, 1f)] public float chanceToRainWinter = 0.7f;

    public GameObject rainEffect;
    public Material rainSkyBox;
    public Light existingLight;

    public bool isSpecialWeather;
    private Coroutine lightningCoroutine;

    public enum WeatherCondition { Sunny, Rainy }
    private WeatherCondition currentWeather = WeatherCondition.Sunny;

    private void Start() => TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);

    public void GenerateRandomWeather()
    {
        float chanceToRain = TimeManager.Instance.currentSeason switch
        {
            TimeManager.Season.Spring => chanceToRainSpring,
            TimeManager.Season.Summer => chanceToRainSummer,
            TimeManager.Season.Fall => chanceToRainFall,
            TimeManager.Season.Winter => chanceToRainWinter,
            _ => 0f
        };

        if (Random.value <= chanceToRain)
        {
            currentWeather = WeatherCondition.Rainy;
            isSpecialWeather = true;
            Invoke("StartRain", 1f);
        }
        else
        {
            currentWeather = WeatherCondition.Sunny;
            isSpecialWeather = false;
            StopRain();
        }
    }

    private void StartRain()
    {
        if (!SoundManager.Instance.rainChannel.isPlaying)
        {
            SoundManager.Instance.rainChannel.clip = SoundManager.Instance.rainSound;
            SoundManager.Instance.rainChannel.loop = true;
            SoundManager.Instance.rainChannel.Play();
        }
        RenderSettings.skybox = rainSkyBox;
        rainEffect.SetActive(true);
        lightningCoroutine = StartCoroutine(LightningEffect());
    }

    private void StopRain()
    {
        if (SoundManager.Instance.rainChannel.isPlaying)
        {
            SoundManager.Instance.rainChannel.Stop();
        }
        rainEffect.SetActive(false);
        if (lightningCoroutine != null)
        {
            StopCoroutine(lightningCoroutine);
            lightningCoroutine = null;
        }
        if (existingLight != null)
        {
            existingLight.enabled = true;
        }
    }

    private IEnumerator LightningEffect()
    {
        while (currentWeather == WeatherCondition.Rainy)
        {
            existingLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            existingLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }
}
