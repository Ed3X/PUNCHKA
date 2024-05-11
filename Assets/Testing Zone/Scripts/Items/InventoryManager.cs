using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public RawImage[] offImages;
    public RawImage[] onImages;

    private int[] collectedItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            collectedItems = new int[4]; // Assuming there are 4 items in total
            // Al inicio, desactivar todas las imágenes ON
            foreach (RawImage image in onImages)
            {
                image.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(int itemID)
    {
        if (itemID >= 1 && itemID <= 4)
        {
            collectedItems[itemID - 1] = 1; // Assuming item IDs start from 1
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < collectedItems.Length; i++)
        {
            if (collectedItems[i] == 1)
            {
                offImages[i].gameObject.SetActive(false);
                onImages[i].gameObject.SetActive(true);
            }
        }
    }
}
