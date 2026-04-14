using UnityEngine;

[CreateAssetMenu(fileName = "NewStage", menuName = "MergeGame/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public Sprite backgroundSprite; // O bölüme özel arka plan
    public int startLevel;          // O bölümün başladığı item level (Örn: 1, 9, 17...)
    public int endLevel;            // O bölümün bittiği item level (Örn: 8, 16, 24...)
}