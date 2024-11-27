using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbitHitAndScrem;
    [SerializeField] AudioClip rabbitHitAndDie;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    public GameObject bloodPuddle;

    enum AnimalType
    {
        Rabbit,
        Lion,
        Snake
    }
    [SerializeField] AnimalType thisAnimalType;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;


                bloodSplashParticles.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponent<AI_Movement>().enabled = false;

                StartCoroutine(PuddleDelay());

                isDead = true;
            }
            else
            {
                PlayHitSound();
            }
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);


    }
    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel?.PlayOneShot(rabbitHitAndDie);
                break;
            case AnimalType.Lion:
                // TODO: Add lion dying sound
                break;
            case AnimalType.Snake:
                // TODO: Add snake dying sound
                break;
            default:
                Debug.LogWarning("No dying sound defined for this animal type.");
                break;
        }
    }

    private void PlayHitSound()
    {
        if (soundChannel != null)
        {
            soundChannel.PlayOneShot(rabbitHitAndScrem);
        }
        else
        {
            Debug.LogWarning("SoundChannel is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
