using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoreanTyper;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance {get; private set;}
    private void Awake()
    {
        instance = this;
    }
}
