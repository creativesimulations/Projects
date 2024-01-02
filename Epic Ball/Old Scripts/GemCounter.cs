using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class GemCounter : MonoBehaviour
{
    
    Animator animator;

    int gemsInLevel;
    private TextMeshProUGUI text;

    private void Start()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeCounter(int collected)
    {
        animator.SetTrigger("Collect");
        text.text = collected + " / " + gemsInLevel + " Gems";
    }

    public void SetGemCounter(int gemAmountInLevel)
    {
        gemsInLevel = gemAmountInLevel;
        text.text = "0" + " / " + gemsInLevel + " Gems";
    }
}
