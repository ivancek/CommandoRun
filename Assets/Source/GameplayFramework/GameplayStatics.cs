using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameplayStatics
{
    static MethodInfo gcfgo;

    public static GameObject GetCurrentFocusedGameObject(this StandaloneInputModule sim)
    {
        if (gcfgo == null)
        {
            gcfgo = typeof(StandaloneInputModule).GetMethod("GetCurrentFocusedGameObject", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        return gcfgo.Invoke(sim, null) as GameObject;
    }

}
