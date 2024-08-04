using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public Image image;
    public string cardName;
    public int stats;
    public Tier tier;
    public Type type;
    public Sprite sprite; 
    [HideInInspector]public Transform parentAfterDrag;

    public void Start(){
        image.sprite = sprite;
        gameObject.name = cardName;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
    

}

public enum Tier
{
    Nothing,
    S,
    A,
    B,
    C
}

public enum Type{
    Nothing,
    Fighter,
    Tank,
    Support,
    Marksman,
    Mage
}