using System;
using UnityEngine.EventSystems;

public class NavigationSurface : Actor, IPointerClickHandler
{
    public Action<PointerEventData> onClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClicked != null)
        {
            onClicked.Invoke(eventData);
        }
    }
}
