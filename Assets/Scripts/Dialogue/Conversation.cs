using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Conversation")]
public class Conversation
{
    [XmlElement("Key")]
    public string Key;

    [XmlElement("Node")]
    public ConversationNode[] _conversationNodes;

    public static Conversation Load(TextAsset FILE_XML)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Conversation));
        StringReader reader = new StringReader(FILE_XML.text);
        Conversation conversation = serializer.Deserialize(reader) as Conversation;
        return conversation;
    }

}
public class Condition
{
    [XmlAttribute("AnswerKey")]
    public string Key;

    [XmlElement("Answer")]
    public Answer[] _answers;
}

public class Choice
{
    [XmlAttribute("Key")]
    public string Key;

    [XmlElement("Answer")]
    public Answer[] _answers;
}
public class Answer
{
    [XmlAttribute("AnswerID")]
    public int id;

    [XmlAttribute("NextNode")]
    public int NextNode;

    [XmlElement("AnswerText")]
    public string Text;
}

public class Action //Благодаря этому, в xml файле мы можем управлять музыкой и прочим
{
    public bool ActionBool;
    public string ActionMusic;
    public string ActionAudio;
}
