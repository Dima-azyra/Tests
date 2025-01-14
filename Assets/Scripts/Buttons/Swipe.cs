using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;
public class Swipe : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text text;

    Vector2 Down_pos_X;
    Vector2 Down_pos_Y;
    Vector2 Up_pos_X;
    Vector2 Up_pos_Y;
    DateTime time_down;
    DateTime time_up;

    public void OnPointerEnter(PointerEventData eventData)
    {
        time_down = DateTime.Now;
        Down_pos_X = eventData.position;
        Down_pos_Y = eventData.position;
        Down_pos_X.y = 0;
        Down_pos_Y.x = 0;

/*        text.text = Screen.width + "\n" +
            "time: " + time_down + "\n" +
            "Down_pos_X: " + Down_pos_X + "\n" +
            "Down_pos_Y: " + Down_pos_Y;*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        time_up = DateTime.Now;
        double time = (time_up - time_down).TotalMilliseconds;
        Up_pos_X = eventData.position;
        Up_pos_Y = eventData.position;
        Up_pos_X.y = 0;
        Up_pos_Y.x = 0;

        float Distance_X = Vector2.Distance(Down_pos_X, Up_pos_X);
        float Distance_Y = Vector2.Distance(Down_pos_Y, Up_pos_Y);

        if (Distance_X > Screen.width/7 && Distance_Y < Distance_X/3 && time <= 300)
        {
            if (Up_pos_X.x > Down_pos_X.x) Controller.instance.push_prev(); 
            else Controller.instance.push_next();
        }
/*
        text.text = "time: " + time_up + "\n" +
            "time delta: " + time + "\n" +
            "time Screen.width/10: " + Screen.width / 10 + "\n" +
            "Distance_X: " + Distance_X + "\n" +
            "Distance_X / 6: " + Distance_X / 6 + "\n" +
            "Distance_Y: " + Distance_Y + "\n" +
        "Up_pos_X: " + Up_pos_X + "\n" +
        "Up_pos_Y: " + Up_pos_Y;*/
    }
}
