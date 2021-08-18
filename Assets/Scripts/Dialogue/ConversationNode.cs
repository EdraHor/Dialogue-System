using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class ConversationNode
{
    [XmlAttribute("id")]
    public int _id;

    [XmlElement("Text")]
    public string _text; //должен хранить html теги

    [XmlElement("Name")]
    public string _name;

    [XmlElement("Choice")]
    public Choice _choice;

    [XmlElement("Condition")]
    public Condition _condition;

    public bool isChoice;

    public ConversationType conversationType;

    public enum ConversationType
    { Monologue, Dialogue, ChoiceName, Choice, Condition }

    //              Нода распределения              //
    //              Давать нодам ID?
    //              Нужно ли сразу после ноды ответа переходить на другую ветку диалога? (нода распределения
    //              в ноде ответа

    //
    // * isChoice и нет !_answers нет варинтов ответа, мы переходим на новую
    //   ветку диалога на отсновании предыдущих выборов
    //
    // * isChoice и _answers выбираем один из варинтов и переходим на определенный нод
    //
    // * _name и _text - диалог
    //
    // * !_name - монолог
    //

    //    public ConversationNode(string name, string text) //конструктор создания диалога
    //    {
    //        _name = name;
    //        _text = text;
    //        isChoice = false;
    //    }
    //    public ConversationNode(string text) //конструктор создания монолога
    //    {
    //        _text = text;
    //        isChoice = false;
    //    }
    //    public ConversationNode(string name, string text, Answer[] answers) //конструктор создания выбора при диалоге
    //    {
    //        _name = name;
    //        _text = text;
    //        _answers = answers;
    //        isChoice = true;
    //    }
    //    public ConversationNode(string text, Answer[] answers) //конструктор создания выбора при монологе
    //    {
    //        _text = text;
    //        _answers = answers;
    //        isChoice = true;
    //    }

    //    public ConversationNode(string name, string text, int NextNode) //конструктор создания диалога
    //    {
    //        _name = name;
    //        _text = text;
    //        _nextNode = NextNode;
    //        isChoice = true;
    //    }
    //    public ConversationNode(string text, int NextNode) //конструктор создания монолога
    //    {
    //        _text = text;
    //        _nextNode = NextNode;
    //        isChoice = true;
    //    }
}
