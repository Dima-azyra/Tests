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
        start_base();
        string s = await get_value("Show_answers", "Show_in_main");
        if (s != null)
        {
            _show = int.Parse(s);
            if (_show == 1) set_show(false);
            else set_hide(false);
        }
        else set_hide(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!set) set_hide(true);
        else if (!check) set_show(true);
        Controller.instance.write(_show);
    }

    async void set_show(bool is_change)
    {
        _show = 1;
        set_color(blue);
        set = false;
        transform.GetChild(0).GetComponent<TMP_Text>().text = show;
        if (is_change)
        {
            set_save("Show_answers", "Show_in_main", _show + "");
            await save_to_file("Show_answers");
        }
    }
   async void set_hide(bool is_change)
    {
        _show = 0;
        set_color(yellow);
        set = true;
        transform.GetChild(0).GetComponent<TMP_Text>().text = hide;
        if (is_change) 
        {
            set_save("Show_answers", "Show_in_main", _show + "");
            await save_to_file("Show_answers");
        }
        
    }

    public void set_color(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
