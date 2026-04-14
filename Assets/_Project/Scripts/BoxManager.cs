using UnityEngine;
using System.Collections;

public class BoxManager : MonoBehaviour
{
    public GameObject boxPrefab; // Buraya GameBox-11 prefabını koyacaksın
    public float spawnInterval = 10f;

    void Start() => StartCoroutine(SpawnBoxRoutine());

    IEnumerator SpawnBoxRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBox();
        }
    }

    public void SpawnBox()
    {
        if (StageManager.Instance == null) return;
        
        float offset = StageManager.Instance.stageOffset;
        float centerX = StageManager.Instance.currentStageIndex * offset;
        
        Vector3 spawnPos = new Vector3(centerX + Random.Range(-1.5f, 1.5f), Random.Range(-2f, 2f), 0);
        Instantiate(boxPrefab, spawnPos, Quaternion.identity);
    }
}