using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;
using Barterlands.Logging;

public class RaidManager : MonoBehaviour
{
    [Inject] private StalkerUnitManager stalkerUnitManager;
    [SerializeField] private RaidMenuHandler raidMenuHandler;
    [SerializeField] private TextMeshProUGUI raidTimer;
    [SerializeField] private float raidDuration = 60f;

    [SerializeField] private RaidUIElements raidUIElements;

    private ILoggerService _logger;


    void Awake()
    {
        _logger = new UnityLogger();
    }
    public void StartRaid()
    {
        if (stalkerUnitManager.stalkersReadyForRaid.Count != 0)
        {
            raidUIElements.preRaidPanel.SetActive(false);
            raidUIElements.inRaidPanel.SetActive(true);

            StartCoroutine(ExecuteRaidCoroutine());
        }
        else
        {
            _logger.Warning("Нет юнитов, готовых отправиться в рейд.");
            return;
        }
    }
    private IEnumerator ExecuteRaidCoroutine()
    {
        raidMenuHandler.raidStatus.text = "Подготовка к рейду...";
        raidTimer.text = "Ожидание...";
        yield return new WaitForSeconds(1f);

        raidMenuHandler.raidStatus.text = "Рейд начался!";

        float timer = raidDuration;
        while (timer > 0)
        {
            raidTimer.text = $"Осталось времени: {timer:F0} сек.";
            timer -= Time.deltaTime;
            yield return null;
        }

        raidMenuHandler.raidStatus.text = "Рейд успешно завершён!";
        yield return new WaitForSeconds(5f);

        ResetUIAfterRaid();
    }

    private void ResetUIAfterRaid()
    {
        raidUIElements.preRaidPanel.SetActive(true);
        raidUIElements.inRaidPanel.SetActive(false);
    }

    [System.Serializable]
    public class RaidUIElements
    {
        public GameObject preRaidPanel;
        public GameObject inRaidPanel;
    }
}
