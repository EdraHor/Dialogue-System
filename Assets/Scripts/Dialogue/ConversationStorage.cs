using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStorage : MonoBehaviour
{

    public static ConversationStorage instance { get; private set; }
    [Header("Файлы диалогов в формате XML")]
    public TextAsset[] ConvertationFile; //Dialog XML File
    public Dictionary<string, Conversation> conversationsStorage; //Здесь хранятся все наши загруженные диалоги
    public Dictionary<string, int> answersStorage; //Здесь хранятся сделанные нами выборы

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

            Init();
    }

    public static void Init()
    {
        foreach (var conversation in instance.ConvertationFile)
        {
            var newConversation = Conversation.Load(conversation); //Загрузка диалога из файла

            foreach (var node in newConversation._conversationNodes) //определение типов нодов
            {
                if (node._choice != null) node.isChoice = true;

                if (node._name == null && !node.isChoice && node._condition == null)
                    node.conversationType = ConversationNode.ConversationType.Monologue;
                else if (node._name != null && !node.isChoice)
                    node.conversationType = ConversationNode.ConversationType.Dialogue;
                else if (node._name == null && node.isChoice)
                    node.conversationType = ConversationNode.ConversationType.Choice;
                else if (node._name != null && node.isChoice)
                    node.conversationType = ConversationNode.ConversationType.ChoiceName;
                else if (node._condition!=null)
                    node.conversationType = ConversationNode.ConversationType.Condition;
            }

            instance.conversationsStorage[newConversation.Key] = newConversation; //добавление диалога в словарь
        }
    }

    public void SaveAnswer(string key, int value)
    {
        instance.answersStorage[key] = value;
    }

    ConversationStorage() 
    {
        conversationsStorage = new Dictionary<string, Conversation>();
        answersStorage = new Dictionary<string, int>();
    }

}
