using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public TextMeshProUGUI Text; //Text on the main conversation panel
    public TextMeshProUGUI Name; //Name on the main conversation panel
    public Transform ChoiceContainer; //Position over ChoiceItems
    public GameObject ChoiceItems; //button Frefab

    private ChoiceController choiceController;
    private Conversation CurrentConversation;
    private Animator animator;
    public GameObject textPanel;
    private IEnumerator corutine;
    [Range(0.001f, 0.99f)]
    public float TextSpeed = 0.1f;
    [Header("Convertasation position")]
    [SerializeField]
    private int position;
    private bool isActive = false;
    [SerializeField]
    private bool isMessageEnd;

    void Start()
    {
        animator = GetComponent<Animator>();
        TextSpeed = math.pow(1 - TextSpeed, 8); //������� ���������� ��� ��������� ��������
        //var newConversation = Conversation.Load(ConvertationFile); //�������� ������� �� �����
        //foreach (var item in newConversation._conversationNodes)
        //{
        //    if (item._answers != null) item.isChoice = true;
        //}
        //DialogueStorage.Instance.dialogueStorage = new Dictionary<string, Conversation>()
        //{
        //    [newConversation.key] = newConversation
        //};

        choiceController = ChoiceContainer.GetComponent<ChoiceController>();
        choiceController.OnMakeAChoice += OnTakeChoice;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Continue();
        }
    }

    private void OnTakeChoice(int NextNode, int id) //���������� ��� ������ ������
    {
        ConversationStorage.instance.SaveAnswer(CurrentConversation._conversationNodes[position]._choice.Key, id);
        position = NextNode;
        StopCoroutine(corutine);
        DisplayConversation();
    }

    public void Continue()
    {
        if (isActive)
        {
            if (isMessageEnd && !CurrentConversation._conversationNodes[position].isChoice)
                Debug.Log("Conversation " + position);
            NextNode();
        }
    }

    public void StartConversation(string ConversationKey)
    {
        if (!isActive)
        {
            CurrentConversation = ConversationStorage.instance.conversationsStorage[ConversationKey];
            animator.SetTrigger("StartConversation");
            isActive = true;
            position = 0;
            DisplayConversation();
            Debug.Log("Conversation " + ConversationKey + " start");
        }
    }
    public void EndConversation()
    {
        animator.SetTrigger("EndConversation");
        isActive = false;
        //�������� ������ �������
        Debug.Log("Conversation end");
    }

    private void DisplayConversation()
    {
        var Node = CurrentConversation._conversationNodes[position];
        Text.text = "";
        corutine = DisplayText(Node._text);


        switch (Node.conversationType)
        {
            case ConversationNode.ConversationType.Monologue:
                StartCoroutine(corutine); //����������� ����� ���������
                textPanel.SetActive(false);
                if (ChoiceContainer.gameObject.activeSelf) //���� ������ ��� ����������� ������ ������� ������ ������
                    RemoveAnswers();
                break;

            case ConversationNode.ConversationType.Dialogue:
                StartCoroutine(corutine); //����������� ����� ���������
                textPanel.SetActive(true);
                Name.text = Node._name;
                if (ChoiceContainer.gameObject.activeSelf) //���� ������ ��� ����������� ������ ������� ������ ������
                    RemoveAnswers();
                break;

            case ConversationNode.ConversationType.ChoiceName:
                StartCoroutine(corutine); //����������� ����� ���������
                textPanel.SetActive(true);
                CreateAnswers();
                Name.text = Node._name;
                break;

            case ConversationNode.ConversationType.Choice:
                StartCoroutine(corutine); //����������� ����� ���������
                textPanel.SetActive(false);
                CreateAnswers();
                break;
            case ConversationNode.ConversationType.Condition:
                ConversationStorage.instance.answersStorage.TryGetValue(Node._condition.Key, out var value);
                foreach (var item in Node._condition._answers)
                {
                    if (item.id == value)
                    {
                        ToNode(item.NextNode);
                        Debug.Log("��� �������� ������� �� �����"+item.NextNode.ToString());
                    }
                }
                break;
        }
    }
    private void CreateAnswers()
    {
        var Node = CurrentConversation._conversationNodes[position]._choice;
        ChoiceContainer.gameObject.SetActive(true);
        for (int i = 0; i < Node._answers.Length; i++)
        {
            ChoiceItems.GetComponent<ChoiceItem>().init(Node._answers[i]);
            Instantiate(ChoiceItems, ChoiceContainer.GetComponentInChildren<Transform>());
        }
    }
    private void RemoveAnswers()
    {
        foreach (Transform child in ChoiceContainer) Destroy(child.gameObject);
        ChoiceContainer.gameObject.SetActive(false);
    }

    IEnumerator DisplayText(string text)
    {
        isMessageEnd = false;
        for (int i = 0; i < text.ToCharArray().Length; i++)
        {
            if (text[i] == '<') //��������� �����
            {
                Text.text += text[i];
                while (text[i] != '>') //���������� ����� < tag >
                {
                    Text.text += text[i + 1];
                    i++;
                }

                i++;
                while (text[i] != '<') //����� ������ �����
                {
                    Text.text += text[i];
                    i++;
                    yield return new WaitForSeconds(TextSpeed);
                }

                Text.text += text[i];
                while (text[i] != '>')//���������� ����� </ tag >
                {
                    Text.text += text[i + 1];
                    i++;
                }
                //���������� �������� ����� ��� ������
            }
            else
            {
                Text.text += text[i];
                yield return new WaitForSeconds(TextSpeed);
            }
        }
        isMessageEnd = true;
    }

    private void SimpleDisplay()
    {
        StopCoroutine(corutine);
        isMessageEnd = true;
        Text.text = CurrentConversation._conversationNodes[position]._text;
    }

    private void NextNode()
    {
        var Node = CurrentConversation._conversationNodes[position];//����� ������������ ID � �������� �������, � �� 
                                                                    //���������� ����� � �������!!!
        if (!isMessageEnd || Node.isChoice)
        {
            SimpleDisplay();
            return;
        }
        if (Node._id > 0)
        {
            position++;
            DisplayConversation();
        }
        else
        {
            EndConversation();
        }
    }

    private void ToNode(int id)
    {
        position = id;
        DisplayConversation();
    }


}
