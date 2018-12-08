using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public Transform pivot;
    public float rotateSpeed;

    public void MouseOver()
    {
        Debug.LogFormat("{0} hovered", name);
    }

    private void Update()
    {
        pivot.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

}
