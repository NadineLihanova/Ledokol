using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    
    [Range(1,50)]
    [Header("Controllers")]
    public int panelsCount;
    [Range(1,500)]
    public int panelsOffset;

    [Range(0f, 20f)] 
    public float snapSpeed;
    
    [Header("Other Objects")]
    
    public GameObject panelPrefab;

    private GameObject[] arrPanels;
    private Vector2[] arrPanelsPos;

    private RectTransform contentRect;
    
    private int selectedPanelId;
    private bool isScrolling;
    private Vector2 contentPos;
    
    void Start()
    {
        arrPanels = new GameObject[panelsCount];
        arrPanelsPos = new Vector2[panelsCount];
        contentRect = GetComponent<RectTransform>();
        
        for (int i = 0; i < panelsCount; i++)
        {
            arrPanels[i] = Instantiate(panelPrefab, transform, false);
            if(i == 0) continue;
            
            arrPanels[i].transform.localPosition = new Vector2(arrPanels[i-1].transform.localPosition.x + 
                panelPrefab.GetComponent<RectTransform>().sizeDelta.x + panelsOffset, 
                arrPanels[i].transform.localPosition.y);        
            arrPanelsPos[i] = -arrPanels[i].transform.localPosition;                       
        }
    }


    private void FixedUpdate()
    {
        float nearedPos = float.MaxValue;
        
        for (int i = 0; i < panelsCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - arrPanelsPos[i].x);
            if (distance < nearedPos)
            {
                nearedPos = distance;
                selectedPanelId = i;
            }
        }

        if (isScrolling) return;
        contentPos.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, arrPanelsPos[selectedPanelId].x,
            snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentPos;
    }

    public void CheckScrolling(bool Scroll)
    {
        isScrolling = Scroll;
    }
}
