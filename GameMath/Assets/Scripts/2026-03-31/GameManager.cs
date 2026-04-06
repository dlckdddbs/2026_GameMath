using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections; 
using UnityEditor.EditorTools;

public class GameManager : MonoBehaviour
{
    [Header("매니저 연결")]
    public CriticalManager critManager;
    public GachaManager lootManager;

    [Header("전투 설정")]
    public int playerDamage = 30;
    public int enemyMaxHP = 300;
    private int enemyCurrentHP;
    private bool isDead = false; 

    [Header("UI 연결")]
    public TextMeshProUGUI critInfoText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI probInfoText;
    public TextMeshProUGUI lootInfoText;

    [Header("적 이미지 설정")]
    public Image enemyImageDisplay; 
    public Sprite aliveSprite;      
    public Sprite defeatedSprite;  

    void Start()
    {
        enemyCurrentHP = enemyMaxHP;

        
        if (enemyImageDisplay != null && aliveSprite != null)
        {
            enemyImageDisplay.sprite = aliveSprite;
        }

        UpdateUI();
    }

   
    public void OnAttackButton()
    {
        if (isDead) return;

        bool isCrit = critManager.RollCrit();
        int finalDamage = isCrit ? playerDamage * 2 : playerDamage;
        enemyCurrentHP -= finalDamage;

        if (enemyCurrentHP < 0) enemyCurrentHP = 0;

        if (enemyCurrentHP <= 0)
        {
            StartCoroutine(EnemyDefeatedRoutine()); 
        }

        UpdateUI();
    }

    IEnumerator EnemyDefeatedRoutine()
    {
        isDead = true; 

        if (enemyImageDisplay != null && defeatedSprite != null)
        {
            enemyImageDisplay.sprite = defeatedSprite;
        }

        lootManager.DropLoot();

        UpdateUI();

        yield return new WaitForSeconds(1.0f);

        enemyCurrentHP = enemyMaxHP;

        if (enemyImageDisplay != null && aliveSprite != null)
        {
            enemyImageDisplay.sprite = aliveSprite;
        }

        UpdateUI();

        isDead = false; 
    }

    private void UpdateUI()
    {
        float actualCritRate = critManager.totalHits == 0 ? 0 : ((float)critManager.critHits / critManager.totalHits) * 100f;
        critInfoText.text = $"전체 공격 횟수 : {critManager.totalHits}\n" +
                            $"발생한 치명타 횟수 : {critManager.critHits}\n" +
                            $"설정된 치명타 확률 : {critManager.targetRate * 100f:F2}%\n" +
                            $"실제 치명타 확률 : {actualCritRate:F2}%";

        hpText.text = $"체력 : {enemyCurrentHP}/{enemyMaxHP}";

        probInfoText.text = $"현재 아이템 확률\n" +
                            $"일반 : {lootManager.probNormal * 100f:F1}%\n" +
                            $"고급 : {lootManager.probAdvanced * 100f:F1}%\n" +
                            $"희귀 : {lootManager.probRare * 100f:F1}%\n" +
                            $"전설 : {lootManager.probLegendary * 100f:F1}%";

        lootInfoText.text = $"현재 드롭된 아이템\n" +
                            $"일반 : {lootManager.countNormal}\n" +
                            $"고급 : {lootManager.countAdvanced}\n" +
                            $"희귀 : {lootManager.countRare}\n" +
                            $"전설 : {lootManager.countLegendary}";
    }
}