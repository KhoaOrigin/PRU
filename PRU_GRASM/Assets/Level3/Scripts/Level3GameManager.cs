using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private int currentEnergy;
    [SerializeField] private int energyThreshold =4;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpaner;
    private bool bossSpawned = false;
    [SerializeField] private Image energy;
    [SerializeField] GameObject gameUi;
    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        currentEnergy = 0;
        UpdateEnergy();
        boss.SetActive(false);
    }
    public void AddEnergy()
    {
        if (bossSpawned) return;
        currentEnergy += 1;
        UpdateEnergy();
        if (currentEnergy == energyThreshold)
        {
            CallBoss();
        }
    }
    private void CallBoss()
    {
        bossSpawned = true;
        boss.SetActive(true);
        enemySpaner.SetActive(false);
        gameUi.SetActive(false);
        audioManager.PlayBossClip();
    }
    private void UpdateEnergy()
    {
        if (energy != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / energyThreshold);
            energy.fillAmount = fillAmount;
        }
    }
}
