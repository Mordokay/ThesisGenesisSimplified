using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagListSelectorController : MonoBehaviour {

    public GameObject listOfTags;
    
    public void SetTagChoosen(string name)
    {
        this.GetComponentInChildren<Text>().text = name;
        listOfTags.SetActive(false);
    }

    public void toggleActiveList()
    {
        if (listOfTags.activeSelf)
        {
            listOfTags.SetActive(false);
        }
        else
        {
            listOfTags.SetActive(true);
        }
    }
}
