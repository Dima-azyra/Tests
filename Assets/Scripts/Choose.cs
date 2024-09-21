using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Choose : MonoBehaviour, Take_Button_interface
{
    [SerializeField] GameObject choose_element;

    Dictionary<string, Dictionary<int, string[]>> questions_book = new Dictionary<string, Dictionary<int, string[]>>();
    Dictionary<string, Dictionary<int, string[]>> answer_book = new Dictionary<string, Dictionary<int, string[]>>();

    Color red = new Color(0.8f, 0.1f, 0, 1);
    Color green = new Color(0.1f, 0.6f, 0, 1);
    Color black = Color.black;

    int number_book;
    int number_topic;
    int number_q;
    string name_topic;
    string current_search = "";
    int search_count = 0;

    Dictionary<int,int[]> search_mass = new Dictionary<int, int[]>();

    Dictionary<int, string> books = new Dictionary<int, string>()
    {
        {1, " нига 1"},
        {2, " нига 2"},
        {3, " нига 3"},
    };
    public void close()
    {
        foreach(Transform child in transform.GetChild(0).GetChild(0).GetChild(0))
        {
            Destroy(child.gameObject);
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void open()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void start_choose(int type)
    {
        if (type == 1)
        {
            foreach (KeyValuePair<int, string> book in books)
            {
                GameObject element = Instantiate(choose_element, transform.GetChild(0).GetChild(0).GetChild(0));
                element.GetComponent<Choose_element>().set_param(book.Key, book.Value, this, type, get_book_color(book.Value, book.Key));
            }
        }
        else if (type == 2)
        {
            int topic_count = 0;
            foreach (KeyValuePair<string, Dictionary<int, string[]> > topic in questions_book)
            {
                topic_count++;
                GameObject element = Instantiate(choose_element, transform.GetChild(0).GetChild(0).GetChild(0));
                element.GetComponent<Choose_element>().set_param(topic_count, topic.Key, this, type, get_topic_color(topic.Key, topic.Value));

            }
        }
        else if (type == 3)
        {
            foreach (KeyValuePair<int, string[]> questions in get_topic(number_topic))
            {
                GameObject element = Instantiate(choose_element, transform.GetChild(0).GetChild(0).GetChild(0));
                element.GetComponent<Choose_element>().set_param(questions.Key, questions.Value[0].Insert(questions.Value[0].IndexOf("а)") - 1, "\n"), this, type, get_number_color(questions.Key));
            }
        }
    }

    public void choose_action(int number, int type, string name)
    {
        if (type == 1)
        {
            set_book(number);
            number_q = 1;
            number_topic = 1;
            var questions = get_topic(number_topic);
            set_view(questions);
            close();
        }
        else if (type == 2)
        {
            number_q = 1;
            number_topic = number;
            var questions = get_topic(number_topic);
            set_view(questions);
            close();
        }
        else if (type == 3)
        {
            number_q = number;
            var questions = get_topic(number_topic);
            set_view(questions);
            close();
        }
        Controller.main.reset_button();
    }

    public void load()
    {
        SaveSystem.load_from_file("last_position");
        if (SaveSystem.get_dict("last_position").Count > 0)
        {
            set_book(int.Parse(SaveSystem.get_value("last_position","book")));
            var questions = get_topic(int.Parse(SaveSystem.get_value("last_position", "topic")));
            number_topic = int.Parse(SaveSystem.get_value("last_position", "topic"));
            number_q = int.Parse(SaveSystem.get_value("last_position", "question"));
            SaveSystem.load_from_file(get_save_name());
            set_view(questions);
  
        }
        else choose_action(1, 1, books[1]);
    }

    public void next()
    {
        var questions = get_topic(number_topic);
        if(questions.Count > number_q)
        {
            number_q++;
            set_view(questions);
        }
        else if(questions_book.Count > number_topic)
        {
            number_topic++;
            number_q = 1;
            questions = get_topic(number_topic);
            set_view(questions);
        }
        else if(books.Count > number_book)
        {
            number_book++;
            set_book(number_book);
            questions = get_topic(number_topic);
            set_view(questions);
        }
        else
        {
            set_book(number_book);
            questions = get_topic(number_topic);
            set_view(questions);
        }
        set_view(questions);
        Controller.main.reset_button();
    }

    public void prev()
    {
        var questions = get_topic(number_topic);
        if (number_q > 1)
        {
            number_q--;
            set_view(questions);
        }
        else if (number_topic > 1)
        {
            number_topic--;
            questions = get_topic(number_topic);
            number_q = questions.Count;
            set_view(questions);
        }
        else if (number_book > 1)
        {
            number_book--;
            set_book(number_book);
            number_topic = questions_book.Count;
            questions = get_topic(number_topic);
            number_q = questions.Count;
            set_view(questions);
        }
        else
        {
            set_book(books.Count);
            number_topic = questions_book.Count;
            questions = get_topic(number_topic);
            number_q = questions.Count;
            set_view(questions);
        }
        set_view(questions);
        Controller.main.reset_button();
    }

    public Dictionary<int, string[]> get_topic(int n)
    {
        int current_n = 0;
        foreach (KeyValuePair<string, Dictionary<int, string[]>> topic in questions_book)
        {
            current_n++;
            if (current_n == n)
            {
                name_topic = topic.Key;
                return topic.Value;
            }
        }
        return null;
    }

    void set_book(int n)
    {
        number_book = n;
        number_q = 1;
        number_topic = 1;

        questions_book = get_book_by_number(n);
        answer_book = get_answer_by_number(n);
    }

    Dictionary<string, Dictionary<int, string[]>> get_book_by_number(int number)
    {
        if (number == 1) return Book_1.questions_book_1;
        else if (number == 2) return Book_2.questions_book_2;
        else if (number == 3) return Book_3.questions_book_3;
        else return null;
    }

    Dictionary<string, Dictionary<int, string[]>> get_answer_by_number(int number)
    {
        if (number == 1) return Book_1.answer_book_1;
        else if (number == 2) return Book_2.answer_book_2;
        else if (number == 3) return Book_3.answer_book_3;
        else return null;
    }

    void set_view(Dictionary<int, string[]> questions)
    {
        check_current_color();
        Controller.main.put_book_text(books[number_book]);
        Controller.main.put_part_text(name_topic);
        Controller.main.put_number_text("¬опрос "+number_q + "");
        Controller.main.put_discription_text(questions[number_q][0].Insert(questions[number_q][0].IndexOf("а)") - 1, "\n"));
        Controller.main.put_answer(name_topic, number_q, questions[number_q], new List<string>(answer_book[name_topic][number_q]));
        save_last_view();
    }

    void save_last_view()
    {
        SaveSystem.set_save("last_position", "book", number_book+"");
        SaveSystem.set_save("last_position", "topic", number_topic + "");
        SaveSystem.set_save("last_position", "question", number_q + "");
        SaveSystem.save_to_file("last_position");
    }

    public string get_current_book_name()
    {
        return books[number_book];
    }

    string get_save_name()
    {
        return create_save_name(get_current_book_name(),name_topic);
    }

    public void check_current_color()
    {
        Controller.main.set_color_book(get_book_color(get_current_book_name(), number_book));
        Controller.main.set_color_part(get_topic_color(name_topic, get_topic(number_topic)));
        Controller.main.set_color_number(get_number_color(number_q));
    }

    string create_save_name(string book_name, string topic_name)
    {
        return book_name + "_" + topic_name;
    }
    Color get_book_color(string book_name, int book_number)
    {
        bool check = true;
        foreach (KeyValuePair <string, Dictionary<int, string[]>> topic in get_book_by_number(book_number))
        {
            var save = SaveSystem.get_dict(create_save_name(book_name, topic.Key));
            if (save.ContainsValue(0 + "")) return red;
            else if (!save.ContainsValue(1 + "") || topic.Value.Count != save.Count) check = false;
        }
        if(check) return green;
        else return black;
    }
    Color get_topic_color(string topic_name, Dictionary<int, string[]> topic)
    {
        var save = SaveSystem.get_dict(create_save_name(get_current_book_name(), topic_name));
        if (save.ContainsValue(0 + "")) return red;
        else if (topic.Count == save.Count) return green;
        else return black;
    }

    Color get_number_color(int number)
    {
        var save = SaveSystem.get_dict(get_save_name());
        if (save.ContainsKey(number+"")) 
        {
            if (save[number + ""].Equals(0 + "")) return red;
            else return green;
        }
        return black;
    }

    public void look_for(string text, bool next)
    {
        if (!text.Equals(""))
        {
            if (!current_search.Equals(text))
            {
                search_count = 0;
                current_search = text;
                search_mass.Clear();
                int iterations = 0;
                foreach (KeyValuePair<int, string> book in books)
                {
                    int topic_number = 0;
                    foreach (KeyValuePair<string, Dictionary<int, string[]>> topic in get_book_by_number(book.Key))
                    {
                        topic_number++;
                        foreach (KeyValuePair<int, string[]> question in topic.Value)
                        {
                            if (question.Value[0].Split("а)")[0].ToLower().Contains(text.ToLower()))
                            {
                                search_mass.Add(iterations, new int[] { book.Key, topic_number, question.Key });
                                iterations++;
                            }
                        }
                    }
                }
            }
            else if (search_count < search_mass.Count - 1 && next) search_count++;
            else if (search_count > 0 && !next) search_count--;
            else if(next) search_count = 0;
            else if (!next) search_count = search_mass.Count - 1;

            if (search_mass.Count > 0)
            {
                Controller.main.reset_button();
                set_book(search_mass[search_count][0]);
                number_topic = search_mass[search_count][1];
                number_q = search_mass[search_count][2];
                set_view(get_topic(number_topic));
            }
        }
    }
}
