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
        // Removed the Input check as we'll call ShowAndHideHurtUI() from another script
    }

    public void ShowAndHideHurtUI() // This method is now public
    {
        if (hurtCoroutine == null) // Check if no coroutine is running
        {
            hurtCoroutine = StartCoroutine(InternalShowAndHideHurtUI()); // Start the internal coroutine
        }
    }

    private IEnumerator InternalShowAndHideHurtUI()
    {
        hurtVisible = true;
        hurtLayout.SetActive(true); // Show the UI immediately

        yield return new WaitForSeconds(0.5f); // Wait for half a second

        hurtVisible = false;
        hurtLayout.SetActive(false); // Hide the UI

        hurtCoroutine = null; // Clear the coroutine reference
    }
}