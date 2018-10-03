using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCFeedbackUpdater : MonoBehaviour {

    public GameObject feedbackCanvas;
    UIManager uiManager;

    public GameObject npcObject;
    public GameObject npcFeedbackLine;

    public Text feedbackMessageNumberText;
    public GameObject feedbackMessageCanvas;
    public GameObject feedbackThinkingCanvas;

    public Slider assertivenessSlider;
    public Slider cooperativenessSlider;

    public Slider assertivenessSliderMeter;
    public Slider cooperativenessSliderMeter;

    public Transform listAttributes;

    Message messageBeingTracked;

    void Start () {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (uiManager.isFeedbackEnabled)
        {
            feedbackCanvas.SetActive(true);
            feedbackCanvas.transform.localPosition = npcObject.transform.localPosition;
            refreshFeedbackCanvas();
        }
        else
        {
            feedbackCanvas.SetActive(false);
        }

        if (feedbackMessageCanvas.activeSelf)
        {
            if (messageBeingTracked.messageDecayment <= 0.0f)
            {
                feedbackMessageCanvas.SetActive(false);
            }
            else
            {
                feedbackMessageCanvas.transform.localPosition = npcObject.transform.localPosition;
                feedbackMessageNumberText.text = messageBeingTracked.id + System.Environment.NewLine + messageBeingTracked.messageDecayment;
            }
        }
        if (feedbackThinkingCanvas.activeSelf)
        {
            feedbackThinkingCanvas.transform.localPosition = npcObject.transform.localPosition;
        }
    }

    public void checkMessageFeedback()
    {
        if (uiManager.isMessageLayerEnabled)
        {
            if (uiManager.messageTrackingID.text != "")
            {
                messageBeingTracked = GetComponent<NPCData>().messages.Find(x => x.id == System.Int32.Parse(uiManager.messageTrackingID.text));
                //Check if NPC has the message being tracked
                if (messageBeingTracked != null)
                {
                    feedbackMessageNumberText.text = messageBeingTracked.id + System.Environment.NewLine + messageBeingTracked.messageDecayment;
                    feedbackMessageCanvas.SetActive(true);
                }
                else
                {
                    feedbackMessageCanvas.SetActive(false);
                }
            }
        }
    }

    public void refreshFeedbackCanvas()
    {
        //assertivenessSliderMeter.gameObject.GetComponent<RectTransform>().sizeDelta = 
        //    new Vector2((1 - this.GetComponent<NPCData>().assertiveness) * 100, 20);
        //cooperativenessSliderMeter.gameObject.GetComponent<RectTransform>().sizeDelta = 
        //    new Vector2((1 - this.GetComponent<NPCData>().cooperativeness) * 100, 20);

        assertivenessSlider.value = this.GetComponent<NPCData>().currentAssertivenessLevel;
        cooperativenessSlider.value = this.GetComponent<NPCData>().currentCooperativenessLevel;
    }
}
