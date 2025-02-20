using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHitAndScream;
    [SerializeField] AudioClip animalHitAndDie;

    [SerializeField] AudioClip animalAttack;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    public GameObject bloodPuddle;

    public Slider healthBarSlider;

    public event Action OnDestroyed;

    enum AnimalType
    {
        Rabbit,
        Bear,
        Wolf
    }
    [SerializeField] AnimalType thisAnimalType;

    private void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }


    public void TakeDamage(int damage)
    {
   
        if (isDead == false)
        {
            currentHealth -= damage;
            healthBarSlider.value = currentHealth / maxHealth;

            bloodSplashParticles.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();

                animator.SetTrigger("DIE");
               
                StartCoroutine(PuddleDelay());

                isDead = true;
            }
            else
            {
                PlayHitSound();

                animator.SetTrigger("HURT");

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
        soundChannel.PlayOneShot(animalHitAndDie);
    }

    private void PlayHitSound()
    {
        soundChannel.PlayOneShot(animalHitAndScream);
    }

    public void PlayAttackSound()
    {
        soundChannel.PlayOneShot(animalAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            healthBarSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBarSlider.gameObject.SetActive(false);
        }
    }
}
