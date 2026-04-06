using UnityEngine;

public class CriticalManager : MonoBehaviour
{
    public int totalHits = 0;       
    public int critHits = 0;        
    public float targetRate = 0.3f; 

    public bool RollCrit()
    {
        totalHits++;
        float currentRate = 0f;
        if (totalHits > 0)
        {
            currentRate = (float)critHits / totalHits;
        }

        if (currentRate < targetRate && (float)(critHits + 1) / totalHits <= targetRate)
        {
            critHits++;
            return true;
        }

        if (currentRate > targetRate && (float)critHits / totalHits >= targetRate)
        {
            return false;
        }

        if (Random.value < targetRate)
        {
            critHits++;
            return true;
        }

        return false;
    }
}