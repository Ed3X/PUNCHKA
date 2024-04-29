using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt_Layout : MonoBehaviour
{
    public GameObject hurtLayout;

    private bool hurtVisible = false;
    private Coroutine hurtCoroutine; // Reference to the running coroutine

    private void Start()
    {
        hurtLayout.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && hurtCoroutine == null) // Check if H is pressed and no coroutine is running
        {
            hurtCoroutine = StartCoroutine(ShowAndHideHurtUI()); // Start the coroutine
        }
    }

    IEnumerator ShowAndHideHurtUI()
    {
        hurtVisible = true;
        hurtLayout.SetActive(true); // Show the UI immediately

        yield return new WaitForSeconds(0.5f); // Wait for half a second

        hurtVisible = false;
        hurtLayout.SetActive(false); // Hide the UI

        hurtCoroutine = null; // Clear the coroutine reference
    }
}