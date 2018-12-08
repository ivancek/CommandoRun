using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pickup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IInteractable
{
    public Transform pivot;
    public Light myLight;
    public float rotateSpeed;

    public void Interact()
    {
        Debug.LogFormat("Interacting with {0}", name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myLight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myLight.enabled = false;
    }

    private void Update()
    {
        pivot.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

}
