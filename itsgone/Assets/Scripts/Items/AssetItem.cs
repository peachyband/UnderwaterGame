using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Item")]
public class AssetItem : ScriptableObject, Item
{
    public string Name => _name;
    public Sprite UIIcon => _uiIcon;


    public string _wear;
    public GameObject _ref;    
    [SerializeField] private string _name;
    [SerializeField] private Sprite _uiIcon;
}

