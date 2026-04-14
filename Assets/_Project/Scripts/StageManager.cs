using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Kamera Ayarları")]
    public Transform mainCamera;
    public float stageOffset = 6.15f;
    public float transitionDuration = 0.6f; // Geçiş süresi (sn)
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Yumuşaklık eğrisi

    [Header("Bölüm Verileri")]
    public List<StageData> stages;
    public int currentStageIndex = 0;
    public int maxUnlockedStage = 0;

    private Coroutine transitionCoroutine;

    void Awake() => Instance = this;

    // Item seviyesine göre hangi odada olduğunu bulur
    public int GetStageIndexForItem(int itemLevel)
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (itemLevel >= stages[i].startLevel && itemLevel <= stages[i].endLevel)
                return i;
        }
        return 0;
    }

    public void CheckStageTransition(int newItemLevel)
    {
        int targetIndex = GetStageIndexForItem(newItemLevel);
        if (targetIndex > maxUnlockedStage)
        {
            maxUnlockedStage = targetIndex;
            MoveToStage(targetIndex);
        }
    }

    public void MoveNext() { if (currentStageIndex < maxUnlockedStage) MoveToStage(currentStageIndex + 1); }
    public void MoveBack() { if (currentStageIndex > 0) MoveToStage(currentStageIndex - 1); }

    private void MoveToStage(int index)
    {
        currentStageIndex = index;
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(SmoothMoveCamera(index * stageOffset));
    }

    IEnumerator SmoothMoveCamera(float targetX)
    {
        float elapsedTime = 0;
        Vector3 startPos = mainCamera.position;
        Vector3 endPos = new Vector3(targetX, startPos.y, startPos.z);

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            // Easing eğrisini burada uyguluyoruz:
            mainCamera.position = Vector3.Lerp(startPos, endPos, transitionCurve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.position = endPos;
    }
}