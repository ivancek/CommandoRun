using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pickup : Actor, IPointerEnterHandler, IPointerExitHandler, IInteractable
{
    [Header("Data")]
    public ItemData itemData;

    [Header("Setup")]
    public Transform pivot;
    public Light myLight;
    public float rotateSpeed;


    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        GameObject content = Instantiate(itemData.pickupPrefab, pivot, false);
        content.name = itemData.shortName;
    }

    public virtual void Interact(Controller controller)
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
