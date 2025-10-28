using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Grid Settings")]
    [SerializeField] private GameObject inventoryPanel; 
    [SerializeField] private int columns = 6;
    [SerializeField] private int rows = 5;
    [SerializeField] private float cellSize = 80f;
    [SerializeField] private float spacing = 5f;

    private List<ItemSlot> itemSlots = new();
    private RectTransform gridRect;

    void Start()
    {
        itemSlots = new List<ItemSlot>(
           inventoryPanel.GetComponentsInChildren<ItemSlot>()
       );

        gridRect = inventoryPanel.GetComponent<RectTransform>();
        SetupGridLayout();
    }
    private void SetupGridLayout()
    {
        if (inventoryPanel.TryGetComponent(out GridLayoutGroup grid))
        {
            grid.cellSize = new Vector2(cellSize, cellSize);
            grid.spacing = new Vector2(spacing, spacing);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
        }
    }
}
