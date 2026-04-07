using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0);
    }
    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }
    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }
    public void OnAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);
        bool isMiss = normalDamage < (baseDamage - 2 * sd);
        bool isWeakPoint = normalDamage > (baseDamage + 2 * sd);
        bool isCrit = Random.value < critRate;

        float finalDamage = normalDamage;
        string logText = "";

        if (isMiss)
        {
            finalDamage = 0;
            logText = "<color=grey>[명중 실패]</color> 데미지: 0";
        }
        else
        {
            if (isWeakPoint)
            {
                finalDamage *= 2.0f; 
                logText += "<color=blue>[약점 공격!]</color> ";
            }

            if (isCrit)
            {
                finalDamage *= critMult;
                logText += "<color=red>[치명타!]</color> ";
            }

            logText += string.Format("데미지: {0:F1}", finalDamage);
        }

        attackCount++;
        totalDamage += finalDamage;

        logDisplay.text = logText;
        UpdateUI();
    }

    public void OnAttack12()
    {
        int weakCount = 0;
        int missCount = 0;
        int critCount = 0;
        float maxDamage = 0f;

        float sd = baseDamage * stdDevMult;

        for (int i = 0; i < 1000; i++)
        {
            float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

            bool isMiss = normalDamage < (baseDamage - 2 * sd);
            bool isWeakPoint = normalDamage > (baseDamage + 2 * sd);
            bool isCrit = Random.value < critRate;

            float finalDamage = normalDamage;

            if (isMiss)
            {
                finalDamage = 0;
                missCount++;
            }
            else
            {
                if (isWeakPoint)
                {
                    finalDamage *= 2.0f;
                    weakCount++;
                }

                if (isCrit)
                {
                    finalDamage *= critMult;
                    critCount++;
                }
            }
            if (finalDamage > maxDamage)
            {
                maxDamage = finalDamage;
            }

            attackCount++;
            totalDamage += finalDamage;
        }

        logDisplay.text = string.Format("<b>[1000회 공격 결과]</b>\n약점 공격: {0}회 / 명중 실패: {1}회\n전체 크리티컬: {2}회\n최대 데미지 기록: {3:F1}",
            weakCount, missCount, critCount, maxDamage);
        UpdateUI();
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수: {1}\n평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}