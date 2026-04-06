using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [Header("Àü¸®Ç° È¹µæ È®·ü")]
    public float probNormal = 0.50f;
    public float probAdvanced = 0.30f;
    public float probRare = 0.15f;
    public float probLegendary = 0.05f;

    [Header("È¹µæÇÑ Àü¸®Ç° ¼ö·®")]
    public int countNormal = 0;
    public int countAdvanced = 0;
    public int countRare = 0;
    public int countLegendary = 0;


    public void DropLoot()
    {
        float r = Random.value; // 0.0f ~ 1.0f

        if (r < probNormal)
        {
            countNormal++;
            FailLegendary();
        }
        else if (r < probNormal + probAdvanced)
        {
            countAdvanced++;
            FailLegendary();
        }
        else if (r < probNormal + probAdvanced + probRare)
        {
            countRare++;
            FailLegendary();
        }
        else 
        {
            countLegendary++;
            ResetLootProbabilities();
        }
    }

    private void FailLegendary()
    {
        probLegendary += 0.015f;
        probNormal -= 0.005f;
        probAdvanced -= 0.005f;
        probRare -= 0.005f;
    }

    private void ResetLootProbabilities()
    {
        probNormal = 0.50f;
        probAdvanced = 0.30f;
        probRare = 0.15f;
        probLegendary = 0.05f;
    }
}