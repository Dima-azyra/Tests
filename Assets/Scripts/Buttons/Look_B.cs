using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Look_B : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool next;
    public void OnPointerClick(PointerEventData eventData)
    {
        Controller.main.look_for(next);
    }
}
