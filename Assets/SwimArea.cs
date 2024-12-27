using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimArea : MonoBehaviour
{
    public GameObject oxyenBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().isSwimming = true;
        }
        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<PlayerMovement>().isUnderwater = true;
            oxyenBar.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().isSwimming = false;
        }
        if (other.CompareTag("MainCamera"))
        {
            other.GetComponentInParent<PlayerMovement>().isUnderwater = false;
            oxyenBar.SetActive(false);
            //Reset the oxygen bar
            PlayerState.Instance.currentOxygenPercent = PlayerState.Instance.maxOxygenPercent;
        }
    }
}
