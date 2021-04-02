using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class InventoryCell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler/*, IPointerClickHandler*/
{
    public event Action Ejecting;
    
    [SerializeField] private Text _nameField;
    [SerializeField] private Image _iconField;
    public GameObject _refField;
    public AssetItem _cellField;
    public string _wearField;

    private Inventory _invsys;
    private InventorySys _pockets;
    private WearScript _wearsys;
    private Rigidbody rb;
    private Transform _draggingParent;
    private Transform _originalParent;
    private Transform _bodyParent;
    private bool _draggedFromPassive = false;
    private bool _draggedFromActive = false;
    
    private void Start()
    {
        rb = _refField.GetComponent<Rigidbody>();
        
        _invsys = GameObject.FindGameObjectWithTag("InvSys").GetComponent<Inventory>();
        _wearsys = _refField.GetComponent<WearScript>();
        _pockets = GameObject.Find("Pockets").GetComponent<InventorySys>();
        //_typePool[5] = "legs";
        //_typePool[6] = "boots";
    }
    private void ChangedMind()
    {
        Transform space = GameObject.Find(_wearField).GetComponent<Transform>();
        _refField.transform.rotation = space.rotation;
        _refField.transform.position = space.position;
        _refField.transform.SetParent(space);
        _refField.SetActive(true);
        transform.SetParent(_bodyParent);
    }
    private void ThrowOut()
    {
        Debug.Log("Throwed out " + name + "!");
        _wearsys.isEquiped = false;
        rb.freezeRotation = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.AddForce(Vector3.forward, ForceMode.Impulse);
        _refField.SetActive(true);
        _refField.transform.SetParent(null);
        if (_invsys.ActItems.Contains(_cellField)) _invsys.ActItems.Remove(_cellField);
        if (_invsys.PassItems.Contains(_cellField)) _invsys.PassItems.Remove(_cellField);
        Eject();
    }
    public void Equip()
    {
        if (_refField.transform.tag == "Consumble") InsertInGrid();
        if (_refField.transform.tag == "Equipment" && !_wearsys.isEquiped)
        {
            if (CheckAvalibility(_wearField))
            {
                Transform space = GameObject.Find(_wearField).GetComponent<Transform>();
                Debug.Log("Equiped " + name + " type of " + _wearField + "!");
                rb.freezeRotation = true;
                rb.detectCollisions = true;
                _refField.transform.rotation = space.rotation;
                _refField.transform.position = space.position;
                _refField.transform.SetParent(space);
                _refField.SetActive(true);
                _wearsys.isEquiped = true;
                _invsys.PassItems.Remove(_cellField);
                _invsys.ActItems.Add(_cellField);
                transform.SetParent(_bodyParent);
            }
            else if (!CheckAvalibility(_wearField)) InsertInGrid();
        }
    }
    public void Unequip()
    {
        Debug.Log("Unequiped " + name + "!");
        //TODO : unequip (remove from active and add to passive, reference is coming gone after unequipping)
        if(_invsys.PassItems.Count < _pockets.passspace) {
            _invsys.ActItems.Remove(_cellField);
            _invsys.PassItems.Add(_cellField);
            _wearsys.isEquiped = false;
            Eject();
        }
        
    }
    private void InsertInGrid()
    {
        int closestIndex = 0;

        for (int i = 0; i < _originalParent.transform.childCount; i++)
        {
            if (Vector3.Distance(transform.position, _originalParent.GetChild(i).position) <
                Vector3.Distance(transform.position, _originalParent.GetChild(closestIndex).position))
            {
                closestIndex = i;
            }
        }
        transform.SetParent(_originalParent);
        transform.SetSiblingIndex(closestIndex);

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (In((RectTransform)_originalParent))
        {
            transform.SetParent(_draggingParent);
            _draggedFromPassive = true;
            Debug.Log("Start dragging from passive inventory...");
        }
        else if (In((RectTransform)_bodyParent))
        {
            if (_wearsys.unchangable)
            {

            }
            else if(!_wearsys.unchangable)
            {
                _draggedFromActive = true;
                transform.SetParent(_draggingParent);
                Debug.Log("Start dragging from active inventory...");
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggedFromPassive)
        {
            transform.position = Input.mousePosition;
            rb.freezeRotation = true;
            rb.detectCollisions = true;
            rb.useGravity = false;
            if (In((RectTransform)_originalParent))
            {
                _refField.SetActive(false);
            }
            else if (In((RectTransform)_bodyParent))
            {
                _refField.SetActive(true);
                _refField.transform.position = GameObject.Find(_wearField).transform.position;
            }
            else
            {
                _refField.SetActive(false);
            }
        }
        else if (_draggedFromActive)
        {
            transform.position = Input.mousePosition;
            rb.freezeRotation = true;
            rb.detectCollisions = true;
            rb.useGravity = false;
            if (In((RectTransform)_originalParent))
            {
                _refField.SetActive(false);
            }
            else if (In((RectTransform)_bodyParent))
            {
                _refField.SetActive(true);
                _refField.transform.position = GameObject.Find(_wearField).transform.position;
            }
            else
            {
                _refField.SetActive(false);
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggedFromPassive)
        {
            if (In((RectTransform)_originalParent))
            {
                InsertInGrid();
            }
            else if (In((RectTransform)_bodyParent))
            {
                Equip();
            }
            else ThrowOut();
        _invsys.ActivateRender = true;
        }
        else if (_draggedFromActive)
        {
            if (In((RectTransform)_originalParent))
            {
                Unequip();
            }
            else if (In((RectTransform)_bodyParent))
            {
                ChangedMind();
            }
            else ThrowOut();
        _invsys.ActivateRender = true;
        }
        _draggedFromActive = false;
        _draggedFromPassive = false;
    }
    private void Eject()
    {
        Ejecting?.Invoke(); 
    }
    private bool CheckAvalibility(string clothType)
    {
        foreach(AssetItem armor in _invsys.ActItems)
        {
            if (armor._ref.GetComponent<WearScript>().clothType.ToString() == clothType) return false;
        }
        return true;
    }
    private bool In(RectTransform originalParent)
    {
        return originalParent.rect.Contains(transform.position);
    }
    public void Render(Item item)
    {
        _nameField.text = item.Name;
        _iconField.sprite = item.UIIcon;
    }
    public void Init(Transform draggingParent, Transform bodyParent, AssetItem cell) 
    {
        _originalParent = transform.parent;
        _draggingParent = draggingParent;
        _bodyParent = bodyParent;
        _cellField = cell;
    }
}

