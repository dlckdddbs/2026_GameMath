using UnityEngine;

public class SimplePerlinTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;
    public float heightMultiplier = 8f;
    public int waterLevel = 3; 

    public GameObject grassPrefab; 
    public GameObject dirtPrefab;  
    public GameObject waterPrefab; 

    SimplePerlinNoise simpleNoise;
    float offsetX;
    float offsetZ;

    void Start()
    {
        simpleNoise = GetComponent<SimplePerlinNoise>();

        offsetX = Random.Range(-9999f, 9999f);
        offsetZ = Random.Range(-9999f, 9999f);

        Generate();
    }

    public void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = (x * scale) + offsetX;
                float zCoord = (z * scale) + offsetZ;

                float noise = simpleNoise.Noise(xCoord, zCoord);
                int height = Mathf.RoundToInt(noise * heightMultiplier);

                CreateTerrainColumn(x, z, height);
            }
        }
    }

    void CreateTerrainColumn(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);

            GameObject prefabToSpawn = (y == height) ? grassPrefab : dirtPrefab;

            Instantiate(prefabToSpawn, position, Quaternion.identity, transform);
        }

        if (height < waterLevel)
        {
            for (int y = height + 1; y <= waterLevel; y++)
            {
                Vector3 waterPos = new Vector3(x, y, z);
                Instantiate(waterPrefab, waterPos, Quaternion.identity, transform);
            }
        }
    }
}