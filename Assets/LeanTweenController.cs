using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenController : MonoBehaviour
{
    float valueToUse = 0;

    public void SetValue(float value)
    {
        valueToUse = value; 
    }
    public void ScaleObjectX(GameObject item)
    {
        LeanTween.scaleX(item, valueToUse, .25f);
    }
}
