using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {

    public GameObject paleteList;
    public GameObject otherPalette1;
    public GameObject otherPalette2;

    public void ToggleListPalete()
    {
        if (paleteList.activeSelf)
        {
            paleteList.SetActive(false);
        }
        else
        {
            paleteList.SetActive(true);
            otherPalette1.SetActive(false);
            otherPalette2.SetActive(false);
        }
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public void SetButtonColor(string colorInfo)
    {
        this.GetComponent<Image>().color = hexToColor(colorInfo);
        paleteList.SetActive(false);
    }
}
