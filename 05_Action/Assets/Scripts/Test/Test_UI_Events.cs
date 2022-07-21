using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test_UI_Events : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag - {eventData.position}");
    }

    public void OnDrag(PointerEventData eventData)
    {        
        Debug.Log($"OnDrag - {eventData.position}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag - {eventData.position}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick - {eventData.position}");
    }
}
