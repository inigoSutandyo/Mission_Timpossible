using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerStatus player;

    public GameObject VillageEnemies;
    public GameObject TeleporterEnemies;
    public GameObject Barrier;
    public GameObject BossUI;
    public BossBehaviour BossBehaviour;

    private HealthScript BossSlider;

    private bool isVillageActive = false;
    private bool isTeleporterActive = false;
    private bool isBossUiActive = false;
    void Start()
    {
        BossSlider = BossUI.GetComponentInChildren<HealthScript>();

        VillageEnemies.SetActive(false);
        TeleporterEnemies.SetActive(false);
        BossUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.quest.id == 6 && !isVillageActive)
        {
            VillageEnemies.SetActive(true);
            isVillageActive = true;

        } else if (player.quest.id == 7 && !isTeleporterActive)
        {
            TeleporterEnemies.SetActive(true);
            Barrier.SetActive(false);
            isTeleporterActive = true;

        } else if (player.quest.id == 8 && !isBossUiActive)
        {
            BossUI.SetActive(true);
            isBossUiActive = true;
        }

        if (isBossUiActive || BossUI.activeSelf)
        {
            BossSlider.SetHealth(BossBehaviour.health);
        }
        
    }
}
