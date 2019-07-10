using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{
    [Header("Controllers")] [Range(0f, 20f)]
    public float SnapSpeed;

    [Header("Other Objects")] public GameObject ButtonItemPrefab;

    private GameObject[] _panels;
    private Vector2[] _panelsPos;
    private GameObject[] _buttons;
    private GameObject _buttonsContainer;
    private RectTransform _contentRect;
    private int _panelsCount;
    private int _selectedPanelId;
    private bool _isScrolling;
    private Vector2 _contentPos;

    void Start()
    {
        InitData();
        CheckDisabledButton();
    }

    private void FixedUpdate()
    {
        if (_isScrolling)
        {
            GetNearedPos();
        }
        else ShowPanel();
    }

    private void InitData()
    {
        _panelsCount = transform.childCount;
        _panels = new GameObject[_panelsCount];
        _panelsPos = new Vector2[_panelsCount];
        _buttons = new GameObject[_panelsCount];
        _buttonsContainer = GameObject.Find("ButtonsContainer");
        _contentRect = GetComponent<RectTransform>();
        _selectedPanelId = 0;
        
        for (int i = 0; i < _panelsCount; i++)
        {
            _panels[i] = transform.GetChild(i).gameObject;
            _panelsPos[i] = InitPanelsPos(i);
            _buttons[i] = CreateButton(i);
        }
    }
    
    private Vector2 InitPanelsPos(int item)
    {
        var vector2 = -(new Vector2(_panels[item].transform.localPosition.x, 
            _panels[item].transform.localPosition.y));
        return vector2;
    }

    private GameObject CreateButton(int item)
    {
        var button = Instantiate(ButtonItemPrefab, _buttonsContainer.transform);
        button.GetComponent<Button>().onClick.AddListener(() => _selectedPanelId = item);
        return button;
    }

    private void CheckDisabledButton()
    {
        for (int i = 0; i < _panelsCount; i++)
        {
            var buttonComponent = _buttons[i].GetComponent<Button>();
            if (i == _selectedPanelId)
            {
                buttonComponent.interactable = false;
            }
            else buttonComponent.interactable = true;
        }
    }

    public void CheckScrolling(bool Scroll)
    {
        _isScrolling = Scroll;
    }

    private void GetNearedPos()
    {
        float nearedPos = float.MaxValue;

        for (int i = 0; i < _panelsCount; i++)
        {
            float distance = Mathf.Abs(_contentRect.anchoredPosition.x - _panelsPos[i].x);
            if (distance < nearedPos)
            {
                nearedPos = distance;
                _selectedPanelId = i;
            }
        }
    }

    private void ShowPanel()
    {
        _contentPos.x = Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPos[_selectedPanelId].x,
            SnapSpeed * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentPos;
        CheckDisabledButton();
    }

    public void NextPanel()
    {
        var id = _selectedPanelId + 1;
        if ((id + 1) <= _panelsCount)
        {
            _selectedPanelId = id;
        }
    }

    public void PrevPanel()
    {
        var id = _selectedPanelId - 1;
        if (id >= 0)
        {
            _selectedPanelId = id;
        }
    }
}