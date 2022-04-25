using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeleportController : MonoBehaviour
{
    private bool isStartTimer;
    private bool isAlreadyTriggered;
    private float time;
    [SerializeField] private GameObject teleporterPanel;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private PlayerStatus player;

    // Start is called before the first frame update
    void Start()
    {
        time = 60f;
        isStartTimer = false;
        isAlreadyTriggered = false;
        teleporterPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            isAlreadyTriggered = true;
            StartCoroutine(StartMessage());
            isStartTimer = true;
            StartCoroutine(StartTimer());
        }
    }

    private IEnumerator StartMessage()
    {
        teleporterPanel.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        teleporterPanel.SetActive(false);
    }

    private IEnumerator StartTimer()
    {

        yield return new WaitForSeconds(1.0f);
        while (isStartTimer)
        {
            yield return new WaitForSeconds(1.0f);
            time -= 1.0f;
            questManager.UpdateQuestBox(time + " / " + 60);
            if (time <= 0)
            {
                player.PlayerDie();
                isStartTimer = false;
                break;
            }
        }
    }
}
