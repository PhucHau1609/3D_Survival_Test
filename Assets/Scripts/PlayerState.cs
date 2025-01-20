using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance {get; set;}

    //----Player Health----//
    public float currentHealth;
    public float maxHealth;

    public bool isPlayerDead;
    public RespawnLocation registeredRespawnLocation;
    public event Action OnRespawnRegistered;    

    //----Payer Calories---//
    public float currentCalories;
    public float maxCalories;

    //---Player Hydration---//
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    //---Player Oxygen---//
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 1f;
    public float oxygenTimer = 0f;
    private float decreaseInterval = 1f;

    public float outOfAirDamagePerSecond = 5f;


    public bool isHydrationActive;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    public AudioSource playerAudioSource;
    public AudioClip playerPainSound;
    public AudioClip playerDeathSound;

    private float hurtSoundDelay = 2f;
    private float nextHurtTime = 0f;

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

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        currentOxygenPercent = maxOxygenPercent;

        StartCoroutine(decreaseHydration());
    }
    IEnumerator decreaseHydration()
    {
        while(true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    void Update()
    {
        if(playerBody.GetComponent<PlayerMovement>().isUnderwater)
        {
            oxygenTimer += Time.deltaTime;

            if(oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0;
            }
        }

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTravelled >= 1)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    private void DecreaseOxygen()
    {
        currentOxygenPercent -= oxygenDecreasedPerSecond * decreaseInterval;
        
        // out of air
        if(currentOxygenPercent < 0)
        {
            currentOxygenPercent = 0;
            SetHealth(currentHealth - outOfAirDamagePerSecond);
        }
    }

    public void SetHealth(float newHealth)
    {
       
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }

    internal void setHealth(float maxHealth)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0 && !isPlayerDead) 
        {
            PlayerDead();
            Debug.Log("Player is dead");
        }
        else
        {
            if (currentHealth > 0 && Time.time >= nextHurtTime) 
            {
                playerAudioSource.PlayOneShot(playerPainSound);
                Debug.Log("Player is hurt");

                nextHurtTime = Time.time + hurtSoundDelay;
            }
        }
    }

    public void PlayerDead()
    {
        isPlayerDead = true;
        playerAudioSource.PlayOneShot(playerDeathSound);

        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        playerBody.GetComponent<PlayerMovement>().enabled = false;
        playerBody.GetComponent<MouseMovement>().enabled = false;

        if (registeredRespawnLocation != null)
        {
            Vector3 position = registeredRespawnLocation.transform.position;

            position.y += 5f; // above the location
            position.z += 5f; // next to the location

            playerBody.transform.position = position; // actually respawn the player

            currentHealth = maxHealth;
        }

        yield return new WaitForSeconds(0.2f);

        isPlayerDead = false;

        playerBody.GetComponent<PlayerMovement>().enabled = true;
        playerBody.GetComponent<MouseMovement>().enabled = true;
    }

    internal void SetRegisteredLocation(RespawnLocation respawnLocation)
    {
        registeredRespawnLocation = respawnLocation;
        OnRespawnRegistered?.Invoke();
    }
}
