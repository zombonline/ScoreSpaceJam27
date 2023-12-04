using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Hint
{
    public string text;
    public bool isSpecial;

    public Hint(string text, bool special)
    {
        this.text = text;
        this.isSpecial = special;
    }
}


public class Hintbar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText;
    bool coroutineRunning = false;
    List<Hint> hintQueue = new List<Hint>();
    [SerializeField] TextAsset hintTextFile;
    string[] hints;
    Vector2 originalTextPos;
    private void Awake()
    {
        hints = hintTextFile.text.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);
        originalTextPos = hintText.rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if(hintQueue.Count < 1)
        {
            AssignHintText(hints[UnityEngine.Random.Range(0, hints.Length)],false);
        }
    }

    public void AssignHintText(string text, bool isSpecial)
    {
        var newHint = new Hint(text, isSpecial);

        if (coroutineRunning)
        { 
            if (newHint.isSpecial) { hintQueue.Insert(0, newHint); }
            else { hintQueue.Add(newHint); }
        }
        else
        {
            StartCoroutine(ScrollRoutine(newHint));
        }
    }
    public IEnumerator ScrollRoutine(Hint hint)
    {
        coroutineRunning = true;
        hintText.text = hint.text;
        if (hint.isSpecial) { hintText.color = Color.green; }
        else { hintText.color = Color.white; }

        hintText.rectTransform.anchoredPosition = originalTextPos;
        while (hintText.rectTransform.anchoredPosition.x >  -1920 - hintText.rectTransform.sizeDelta.x)
        {
            hintText.rectTransform.anchoredPosition = new Vector2(hintText.rectTransform.anchoredPosition.x - 5, hintText.rectTransform.anchoredPosition.y);
            yield return new WaitForSeconds(0.02f);
        }
        hintText.rectTransform.anchoredPosition = Vector3.zero;
        coroutineRunning = false;



        if (hintQueue.Count > 0)
        {
            StartCoroutine(ScrollRoutine(hintQueue[0]));
            hintQueue.RemoveAt(0);
        }
    }
}
