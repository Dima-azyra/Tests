using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static SaveSystem;

public class Show_answers_in_main : MonoBehaviour, IPointerClickHandler
{
    public int _show;
    bool set;
    bool check;
    string show = "?";
    string hide = "?";

    Color blue = new Color(0.16f, 0.58f, 0.9f, 1);
    Color yellow = new Color(0.9f, 0.8f, 0.25f, 1);
    Color grey = new Color(0.5f, 0.5f, 0.5f, 1);

    private async void Awake()
    {
        string s = await get_value("Show_answers", "Show_in_main");
        if (s != null)
        {
            _show = int.Parse(s);
            if (_show == 1) set_show();
            else set_hide();
        }
        else set_hide();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!set) set_hide();
        else if (!check) set_show();
        Controller.instance.write(_show);
    }

    async void set_show()
    {
        _show = 1;
        set_color(blue);
        set = false;
        transform.GetChild(0).GetComponent<TMP_Text>().text = show;
        set_save("Show_answers", "Show_in_main", _show + "");
    }
   async void set_hide()
    {
        _show = 0;
        set_color(yellow);
        set = true;
        transform.GetChild(0).GetComponent<TMP_Text>().text = hide;
        set_save("Show_answers", "Show_in_main", _show + "");
    }

    public void set_color(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
