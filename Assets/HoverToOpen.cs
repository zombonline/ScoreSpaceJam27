using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoverToOpen : MonoBehaviour
{
    [SerializeField] RectTransform openRect, closeRect, rectToMove;
    [SerializeField] Vector2 openPos, closePos;
    bool mouseOver = false;
    [SerializeField] UnityEvent mouseEnter, mouseExit;

    private void Update()
    {
        Vector2 localMousePosition = openRect.InverseTransformPoint(Input.mousePosition);
        if (openRect.rect.Contains(localMousePosition) && !mouseOver)
        {
            mouseOver = true;
            LeanTween.move(rectToMove, openPos, .5f).setEase(LeanTweenType.easeInBack);
            mouseEnter.Invoke();
        }
        else if (!closeRect.rect.Contains(localMousePosition) && mouseOver)
        {
            mouseOver = false;
            LeanTween.move(rectToMove, closePos, .5f).setEase(LeanTweenType.easeInBack);
            mouseExit.Invoke();
        }
    }

}
