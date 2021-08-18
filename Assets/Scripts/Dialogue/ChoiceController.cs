using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceController : MonoBehaviour
{
    public delegate void onMakeAChoice(int NextNode, int id);
    public event onMakeAChoice OnMakeAChoice;

    public void TakeAnswer(int NextNode, int id) 
    {
        OnMakeAChoice?.Invoke(NextNode, id);
    }
}
