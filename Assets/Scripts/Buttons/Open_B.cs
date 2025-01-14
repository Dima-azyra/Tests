using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Open_B : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject obj;
    public void OnPointerClick(PointerEventData eventData)
    {
        obj.GetComponent<Take_Button_interface>().open();
    }
}
