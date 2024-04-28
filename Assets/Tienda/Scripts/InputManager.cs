using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    private Renderer currentHighlightedObject;
    private Color originalColor;

    public float PlayerMoney = 10f;

    [SerializeField]
    private ObjectDatabaseSO Database_tienda;

    private List<int> generatedNumbers = new List<int>();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
            HighlightedObject(hit.collider.gameObject);
        }
        else
        {
            lastPosition = new Vector3(0.0f, -3.0f, 0.0f);
            UnhighlightObject();
        }
        return lastPosition;
    }

    private void UnhighlightObject()
    {
        if(currentHighlightedObject != null)
        {
            currentHighlightedObject.material.color = originalColor;
            currentHighlightedObject = null;
        }
    }

    private void HighlightedObject(GameObject objectToHighlight)
    {
        if(currentHighlightedObject != null)
        {
            currentHighlightedObject.material.color = originalColor;
        }
        currentHighlightedObject = objectToHighlight.GetComponent<Renderer>();

        if(currentHighlightedObject != null)
        {
            originalColor = currentHighlightedObject.material.color;
            currentHighlightedObject.material.color = Color.yellow;
            if (Input.GetMouseButtonDown(0))
            {
                BuyItem();
                
            }
        }
    }
    private void BuyItem()
    {
        if(PlayerMoney > 3)
        {
            Destroy(currentHighlightedObject);
            PlayerMoney -= 3;
        }        
    }
}
