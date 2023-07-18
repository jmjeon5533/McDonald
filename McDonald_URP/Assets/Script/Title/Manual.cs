using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    public List<GameObject> Panel = new List<GameObject>();
    void Start()
    {
        SetPanel(0);
    }
    public void SetPanel(int ind)
    {
        for (int i = 0; i < Panel.Count; i++)
        {
            Panel[i].SetActive(false);
        }
        Panel[ind].SetActive(true);
        SoundManager.instance.SetAudio(SceneManager.instance.ClickSound,
            SoundManager.SoundState.SFX, false);
    }
}
