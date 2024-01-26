using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class Drop2D : MonoBehaviour, IDropHandler
{
    public DraggingBox dragbox;
    public int slotID;
    public void OnDrop(PointerEventData eventData)
    {
        //moze //1linijka
        //eventData.pointerDrag.transform.position = transform.position;
        GameObject newPearl = Instantiate(eventData.pointerDrag.gameObject, transform, true);
        newPearl.transform.position = transform.position;

        dragbox.SetID(slotID, eventData.pointerDrag.GetComponent<Drag2D>().pearlID);
    }
}
