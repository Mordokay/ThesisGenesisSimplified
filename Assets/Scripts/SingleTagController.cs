using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTagController : MonoBehaviour {

    public GameObject tagButton;

    public void SetTagChoosen(string name)
    {
        tagButton.GetComponentInChildren<Text>().text = name;
        tagButton.GetComponent<TagListSelectorController>().listOfTags.SetActive(false);
    }
}