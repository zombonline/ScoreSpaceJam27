using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown monitorDropDown;

    private void Awake()
    {
        monitorDropDown.ClearOptions();
        for(int i = 0; i < Display.displays.Length; i++)
        {
            TMP_Dropdown.OptionData newData = new TMP_Dropdown.OptionData();
            newData.text = "Monitor " + i.ToString();
            monitorDropDown.options.Add(newData); 
        }
    }
    public void ChangeMonitor(int value)
    {
        Display.displays[value].Activate();
        Camera.main.targetDisplay = value;
    }    
}
