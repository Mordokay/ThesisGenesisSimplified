using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message
{
    public int id;
    public float messageTransmissionTime;
    public float messageDecayment;
    public List<Tag> tags;
    public string description;

    [System.Serializable]
    public class Tag
    {
        public string name;
        public int weight;

        public Tag(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
        }
    };

    public override string ToString() {

        string msg = "";
        msg += "ID: " + id + " msgTime: " + messageTransmissionTime + " { ";
        foreach(Tag t in tags)
        {
            msg += t.name + " " + t.weight + ",";
        }
        if(tags.Count > 0)
        {
            msg.Substring(0, msg.Length - 1);
        }
        msg += "} Description: " + description;
        return msg;
    }

    public Message(Message msg)
    {
        this.id = msg.id;
        this.messageTransmissionTime = msg.messageTransmissionTime;
        this.messageDecayment = msg.messageDecayment;
        this.tags = msg.tags;
        this.description = msg.description;
    }

    public Message(int id, float messageTransmissionTime, string description, string tagsText)
    {
        //Debug.Log("id " + id + " messageTimeOfLife " + messageTimeOfLife + " description " + description + " tagsText " + tagsText);
        this.id = id;
        this.messageTransmissionTime = messageTransmissionTime;
        this.description = description;
        this.messageDecayment = 1.0f;

        tags = new List<Tag>();
        string[] tagsList = tagsText.Split(',');
        foreach (string t in tagsList)
        {
            string[] tagData = t.Split(' ');
            tags.Add(new Tag(tagData[0], System.Int32.Parse(tagData[1])));
        }
    }

    public Message(int id, float messageTransmissionTime, string description, List<Tag> tags)
    {
        //Debug.Log("id " + id + " messageTimeOfLife " + messageTimeOfLife + " description " + description + " tagsText " + tagsText);
        this.id = id;
        this.messageTransmissionTime = messageTransmissionTime;
        this.description = description;
        this.messageDecayment = 1.0f;

        this.tags = tags;
    }
}