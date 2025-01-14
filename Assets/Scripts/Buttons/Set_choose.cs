using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Set_choose : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int type;
    public void OnPointerClick(PointerEventData eventData)
    {
        System.DateTime date1 = System.DateTime.Now;
        Controller.instance.open_choose(type);
        System.DateTime date2 = System.DateTime.Now;
        print(date2 - date1);
    }
}
