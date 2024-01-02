using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core;

public class GeneralNotice : MonoBehaviour
{
    private string message;
    private Color textColor;
    private TextMeshProUGUI TMP;

    void Awake()
    {
        TMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(Color teamColor, string messageToPrint)
    {
        message = messageToPrint;
        textColor = teamColor;
    }
    
private void PrintNotice()
{
        TMP.color = textColor;
        TMP.text = message;
}

    public void ReturnNotice()
    {
        StartCoroutine(EventManager.DelayDespawnObject(gameObject, null, .5f));
    }
}
