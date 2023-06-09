using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}
    public Text ScoreText;
    public int Score = 0;
    private void Awake() {
        instance = this;
    }
}
