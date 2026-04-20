using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnBasedGame : MonoBehaviour
{
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    public TextMeshProUGUI combatResultText; 
    public TextMeshProUGUI itemResultText;  

    int turn = 0;
    bool rareItemObtained = false;
    float currentRareChance = 0.2f;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    int totalSpawnedEnemies = 0;
    int totalKilledEnemies = 0;
    int totalTryAttack = 0;
    int totalHitAttack = 0;
    int totalCritAttack = 0;
    float maxDamage = 0f;
    float minDamage = 9999f;

    int countPotion = 0;
    int countGold = 0;
    int countNormalWeapon = 0;
    int countRareWeapon = 0;
    int countNormalArmor = 0;
    int countRareArmor = 0;

    public void StartSimulation()
    {
        rareItemObtained = false;
        turn = 0;
        currentRareChance = 0.2f;

        totalSpawnedEnemies = 0;
        totalKilledEnemies = 0;
        totalTryAttack = 0;
        totalHitAttack = 0;
        totalCritAttack = 0;
        maxDamage = 0f;
        minDamage = 9999f;

        countPotion = 0;
        countGold = 0;
        countNormalWeapon = 0;
        countRareWeapon = 0;
        countNormalArmor = 0;
        countRareArmor = 0;

        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복하는 구조
        while (!rareItemObtained)
        {
            turn++;
            SimulateTurn();

            if (!rareItemObtained)
            {
                currentRareChance += 0.05f;
            }
        }

        UpdateUI();
    }

    void SimulateTurn()
    {
        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        totalSpawnedEnemies += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalTryAttack += maxHitsPerTurn;
            totalHitAttack += hits;

            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCritAttack++;
                }

                if (damage > maxDamage) maxDamage = damage;
                if (damage < minDamage) minDamage = damage;

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                totalKilledEnemies++;

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];

                if (reward == "Weapon")
                {
                    if (Random.value < currentRareChance)
                    {
                        rareItemObtained = true;
                        countRareWeapon++;
                    }
                    else countNormalWeapon++;
                }
                else if (reward == "Armor")
                {
                    if (Random.value < currentRareChance)
                    {
                        rareItemObtained = true;
                        countRareArmor++;
                    }
                    else countNormalArmor++;
                }
                else if (reward == "Potion") countPotion++;
                else if (reward == "Gold") countGold++;
            }
        }
    }

    void UpdateUI()
    {
        float hitPercent = (totalTryAttack > 0) ? ((float)totalHitAttack / totalTryAttack) * 100f : 0f;
        float critPercent = (totalHitAttack > 0) ? ((float)totalCritAttack / totalHitAttack) * 100f : 0f;
        if (minDamage == 9999f) minDamage = 0f;

        combatResultText.text = $"총 진행 턴 수 : {turn}\n" +
                                $"발생한 적 : {totalSpawnedEnemies}\n" +
                                $"처치한 적 : {totalKilledEnemies}\n" +
                                $"공격 명중 결과 : {hitPercent:F2}%\n" +
                                $"발생한 치명타율 결과 : {critPercent:F2}%\n" +
                                $"최대 데미지 : {maxDamage:F2}\n" +
                                $"최소 데미지 : {minDamage:F2}";

        itemResultText.text = $"포션 : {countPotion}개\n" +
                              $"골드 : {countGold}개\n" +
                              $"무기 - 일반 : {countNormalWeapon}개\n" +
                              $"무기 - 레어 : {countRareWeapon}개\n" +
                              $"방어구 - 일반 : {countNormalArmor}개\n" +
                              $"방어구 - 레어 : {countRareArmor}개";
    }

    // --- 분포 샘플 함수들 ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}