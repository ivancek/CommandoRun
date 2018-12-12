using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base HUD class. Responsible for defining and spawning of widgets.
/// </summary>
public class HUD : Actor
{
    [Header("HUD")]
    public Transform viewport;
    public Widget[] widgetPrefabs;

    public T SpawnWidget<T>() where T : Widget
    {
        for(int i = 0; i < widgetPrefabs.Length; i++)
        {
            if(widgetPrefabs[i].GetType() == typeof(T))
            {
                Widget newWidget = Instantiate(widgetPrefabs[i], viewport, false);
                return (T)newWidget;
            }
        }
        
        return null;
    }
}
