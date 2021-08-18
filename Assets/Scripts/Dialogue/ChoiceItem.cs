using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChoiceItem : MonoBehaviour, IPointerClickHandler
{
    private ChoiceController _controller;
    private TextMeshProUGUI _textUI;
    public int _id;
    public int _nextNode;

    public void init(Answer answer)
    {
        _textUI = GetComponentInChildren<TextMeshProUGUI>();
        _textUI.text = answer.Text;
        _id = answer.id;
        _nextNode = answer.NextNode;
    }

    private void Start() //Выполняется после инситанциирования
    {
        _controller = GetComponentInParent<ChoiceController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _controller.TakeAnswer(_nextNode, _id);
    }

}
