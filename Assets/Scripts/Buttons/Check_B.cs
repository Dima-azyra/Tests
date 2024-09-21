using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Check_B : MonoBehaviour, IPointerClickHandler
{
    bool check;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!check)
        {
            if (Controller.main.push_check())
            {
                check = true;
                transform.GetChild(0).GetComponent<Text>().text = "Следующий";
            }
        }
        else if (check)
        {
            Controller.main.push_next();
            reset();
        }
    }
    public void reset()
    {
        check = false;
        transform.GetChild(0).GetComponent<Text>().text = "Ответить";
    }
}
