
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public GameObject prefab;
    public string name;
    public int price;
    public bool isUnlocked;
    public Image icon;
}