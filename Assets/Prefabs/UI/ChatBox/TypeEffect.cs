using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeEffect : MonoBehaviour
{
    public TextMeshProUGUI AnimatedTextUI;
    private string sentence;
    
    public float typeSpeed = 0.1f;
    [TextArea(3,10)]
    public string TypeText;


    // Start is called before the first frame update

    public string GetText() {
        return AnimatedTextUI.text;
    }
    private void Awake()
    {
        if (TypeText == "")
        {
            sentence = AnimatedTextUI.text;
        }
        else {
            sentence = TypeText;
        }
    }
   


    private void OnEnable()
    {
        DisplayText(sentence);
    }


    public void DisplayText(string text) {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(text));
    }

   


    IEnumerator TypeSentence(string sentence) {
        AnimatedTextUI.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            AnimatedTextUI.text += letter;
            yield return new WaitForSecondsRealtime(typeSpeed);
        
        }
    }
}
