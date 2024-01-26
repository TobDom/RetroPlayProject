using UnityEngine;

using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
//drag and drop pearls
public class Drag2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 oldPosition;
    public int pearlID;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Beginnig Drag");
        oldPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = oldPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
