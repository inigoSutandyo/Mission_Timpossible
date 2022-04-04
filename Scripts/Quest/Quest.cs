using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive { get; set; }
    public bool isDone { get; set; }

    public int id { get; set; }
    public string description { get; set; }

    public Quest()
    {
        isActive = false;
        isDone = false;
    }

 
}
