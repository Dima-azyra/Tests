using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    [SerializeField] Text Book;
    [SerializeField] Text Part;
    [SerializeField] Text Number;
    [SerializeField] Text Discription;
    [SerializeField] InputField Look;

    [Space(20)]

    [SerializeField] GameObject Answer;
    [SerializeField] GameObject Check_B;
    [SerializeField] GameObject Look_B;
    [SerializeField] GameObject Choose;

    public static Controller main;
    string current_input = "";

    private void Awake()
    {
        main = this;
        Choose.GetComponent<Choose>().load();
        Choose.GetComponent<Choose>().hide();
    }
    void OnEnable()
    {
        Look.onEndEdit.AddListener(delegate { inputEndEdit(); });
    }
    void OnDisable()
    {
        Look.onEndEdit.RemoveAllListeners();
    }
    public void put_book_text(string text)
    {
        Book.text = text;
    }
    public void put_part_text(string text)
    {
        Part.text = text;
    }
    public void put_number_text(string text)
    {
        Number.text = text;
    }
    public void put_discription_text(string text)
    {
        Discription.text = text;
    }

    public void put_answer(string name_topic, int number_q, string[] question, List<string> answers)
    {
        Answer.GetComponent<Answer>().put_answers(name_topic, number_q, question, answers);
    }

    public bool push_check()
    {
        return Answer.GetComponent<Answer>().check();
    }

    public void push_next()
    {
        Choose.GetComponent<Choose>().next();
    }
    
    public void push_prev()
    {
        Choose.GetComponent<Choose>().prev();
    }

    public void open_choose(int type)
    {
        Choose.GetComponent<Take_Button_interface>().open();
        Choose.GetComponent<Choose>().start_choose(type);
    }

    public string get_book_name()
    {
         return Choose.GetComponent<Choose>().get_current_book_name();
    }

    public void set_color_book(Color color)
    {
        Book.color = color;
    }

    public void set_color_part(Color color)
    {
        Part.color = color;
    }

    public void set_color_number(Color color)
    {
        Number.color = color;
    }
    public void check_color()
    {
        Choose.GetComponent<Choose>().check_current_color();
    }

    public void look_for(bool next)
    {
        Choose.GetComponent<Choose>().look_for(Look.text, next);
    }

    public void reset_button()
    {
        Check_B.GetComponent<Check_B>().reset();
    }

    private void inputEndEdit()
    {
        if(!current_input.Equals(Look.text))
        {
            current_input = Look.text;
            Choose.GetComponent<Choose>().look_for(Look.text, true);
        }
    }
}
