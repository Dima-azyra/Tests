using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Set_choose : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int type;
    public void OnPointerClick(PointerEventData eventData)
    {
        Controller.main.open_choose(type);
    }
}
