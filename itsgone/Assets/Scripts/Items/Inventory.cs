using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool ActivateRender;
    public List<AssetItem> PassItems;
    public List<AssetItem> ActItems;
    [SerializeField] private InventoryCell _cellTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private Transform _bodyParent;


    public void Update()
    {
        if (ActivateRender)
        {
            Render(PassItems);
            Debug.Log("Rendered!");
        }
        foreach (AssetItem armor in ActItems)
        {
            armor._ref.transform.position = GameObject.Find(armor._wear).GetComponent<Transform>().position;
        }
    }

    public void Render(List<AssetItem> items)
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }
        
        items.ForEach(item =>
        {
            var cell = Instantiate(_cellTemplate, _container);
            cell._refField = item._ref;
            cell._wearField = item._wear;
            cell.name = item.name;
            cell.Init(_draggingParent, _bodyParent, item);
            cell.Render(item);
            cell.Ejecting += () => Destroy(cell.gameObject);
        });
        ActivateRender = false;
    }
}
