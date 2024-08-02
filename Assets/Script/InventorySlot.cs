using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{


    public bool Play;
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
        GameObject dropped = eventData.pointerDrag;
                DragableItem dragableItem = dropped.GetComponent<DragableItem>();
                dragableItem.parentAfterDrag = transform;
        }
    }

    public void PlayCard(){
        if(CompareTag("card") && Play == true){
            
        }
    }

    public void SlotCard(){
        if(CompareTag("slot")){
        
        }
    }
}
