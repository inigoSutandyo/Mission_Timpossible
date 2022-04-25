using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    public PlayerStatus player;
    private List<Quest> quests;
    private List<string> dialouges;
    [SerializeField] private GameObject questBox;
    [SerializeField] private GameObject dialougeBox;
    private Text questText;
    private Text dialougeText;
    private int questCount;

    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject pistol;
    [SerializeField] private WallController wall;

    private int killCount;

    void Start()
    {
        quests = new List<Quest>();
        dialouges = new List<string>();
        killCount = 0;

        InitQuests();
        InitDialouge();
        player.quest = quests[0];
        questText = questBox.GetComponentInChildren<Text>();
        dialougeText = dialougeBox.GetComponentInChildren<Text>();

        dialougeBox.SetActive(false);

        questCount = 0;
        ShowQuest();
    }

    void Update()
    {
        if (player.quest.isDone)
        {
            QuestDone();
        }

    }

    public void AddKillCount()
    {
        int totalKillCount;

        if (player.quest.id < 7)
        {
            totalKillCount = 16;
        } else
        {
            totalKillCount = 6;
        }
        killCount += 1;
        UpdateQuestBox(killCount + " / " + totalKillCount);
        if (killCount >= totalKillCount )
        {
            QuestDone();
            player.quest.isDone = true;
        }
    }

    private void ShowQuest()
    {
        questText.text = player.quest.description;
        questText.color = new Color(255f, 140f, 0f);
    }

    private void QuestDone()
    {
        UpdateQuestBox("(Done)");
        questText.color = Color.green;
    }

    public void NewQuest()
    {
        questCount += 1;
        player.quest.isActive = false;
        player.quest = quests[questCount];
        ShowQuest();
        killCount = 0;
        if (player.quest.id == 6)
        {
            wall.DropWall();
        }
    }

    public void UpdateQuestBox(string str)
    {
        questText.text = player.quest.description + "\n" + str;
    }

    

    private void InitQuests()
    {
        Quest q1 = new Quest
        {
            id = 1,
            description = "Find Asuna and talk to her!",
            isActive = true
        };

        Quest q2 = new Quest
        {
            id = 2,
            description = "Find and pickup the pistol"
        };

        Quest q3 = new Quest
        {
            id = 3,
            description = "Shoot at the target range 10 times!"
        };

        Quest q4 = new Quest
        {
            id = 4,
            description = "Find and equip the rifle."
        };

        Quest q5 = new Quest
        {
            id = 5,
            description = "Shoot the rifle 50 times!"
        };

        Quest q6 = new Quest
        {
            id = 6,
            description = "Kill all the soldiers attacking the village"
        };

        Quest q7 = new Quest
        {
            id = 7,
            description = "Go to the teleporter area and defeat all the soldiers!"
        };

        quests.Add(q1);
        quests.Add(q2);
        quests.Add(q3);
        quests.Add(q4);
        quests.Add(q5);
        quests.Add(q6);
        quests.Add(q7);
    }

    public void CloseDialouge()
    {
        dialougeBox.SetActive(false);
    }

    public void ShowDialogue(int index)
    {
        dialougeText.text = dialouges[index];
        dialougeBox.SetActive(true);
    }

    public void ShowIdleDialogue()
    {
        dialougeText.text = "Hello soldier, do not forget to finish your mission.";
        dialougeBox.SetActive(true);
    }

    private void InitDialouge()
    {
        dialouges.Add("Pickup the pistol in the weapons area.");
        dialouges.Add("Go and shoot the pistol 10 times in the shooting range");
        dialouges.Add("Good job, go and pickup the rifle in the weapons area");
        dialouges.Add("Shoot the rifle 50x to familiarize yourself with it!");
        dialouges.Add("There are soldiers attacking the village, go and stop them!");
        dialouges.Add("Well done, go to the teleport area and defeat all the soldiers!");
    }

}
