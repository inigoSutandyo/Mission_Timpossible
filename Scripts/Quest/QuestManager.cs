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
    void Start()
    {
        quests = new List<Quest>();
        dialouges = new List<string>();


        initQuests();
        initDialouge();
        player.quest = quests[0];
        questText = questBox.GetComponentInChildren<Text>();
        dialougeText = dialougeBox.GetComponentInChildren<Text>();

        dialougeBox.SetActive(false);

        questCount = 0;
        showQuest();
    }
    void Update()
    {
        if (player.quest.isDone)
        {
            questDone();
        }

    }

    private void showQuest()
    {
        questText.text = player.quest.description;
        questText.color = new Color(255f, 140f, 0f);
    }

    private void questDone()
    {
        updateQuestBox("(Done)");
        questText.color = Color.green;
    }

    public void newQuest()
    {
        questCount += 1;
        player.quest.isActive = false;
        player.quest = quests[questCount];
        showQuest();

        if (player.quest.id == 6)
        {
            wall.DropWall();
        }
    }

    public void updateQuestBox(string str)
    {
        questText.text = player.quest.description + "\n" + str;
    }

    private void initQuests()
    {
        Quest q1 = new Quest();
        q1.id = 1;
        q1.description = "Find Asuna and talk to her!";
        q1.isActive = true;

        Quest q2 = new Quest();
        q2.id = 2;
        q2.description = "Find and pickup the pistol";

        Quest q3 = new Quest();
        q3.id = 3;
        q3.description = "Shoot at the target range 10 times!";

        Quest q4 = new Quest();
        q4.id = 4;
        q4.description = "Find and equip the rifle.";

        Quest q5 = new Quest();
        q5.id = 5;
        q5.description = "Shoot the rifle 50 times!";

        Quest q6 = new Quest();
        q6.id = 6;
        q6.description = "Kill all the soldiers attacking the village";

        quests.Add(q1);
        quests.Add(q2);
        quests.Add(q3);
        quests.Add(q4);
        quests.Add(q5);
        quests.Add(q6);
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

    private void initDialouge()
    {
        dialouges.Add("Pickup the pistol in the weapons area.");
        dialouges.Add("Go and shoot the pistol 10 times in the shooting range");
        dialouges.Add("Good job, go and pickup the rifle in the weapons area");
        dialouges.Add("Shoot the rifle 50x to familiarize yourself with it!");
        dialouges.Add("There are soldiers attacking the village, go and stop them!");
        dialouges.Add("Well done, go to the teleport area and defeat all the soldiers!");
    }

}
