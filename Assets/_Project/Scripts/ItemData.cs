using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "MergeGame/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int itemLevel;
    public Sprite itemIcon;
    public double goldPerSecond; // float -> double yapıldı
    public ItemData nextLevelItem;
}