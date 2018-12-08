using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public Transform pivot;
    public Light myLight;
    public float rotateSpeed;

    public void MouseOut()
    {
        myLight.enabled = false;
    }

    public void MouseOver()
    {
        myLight.enabled = true;
    }

    private void Update()
    {
        pivot.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

}
