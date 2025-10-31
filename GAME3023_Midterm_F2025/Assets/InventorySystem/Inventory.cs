using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Grid Settings")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private int columns = 6;
    [SerializeField] private int rows = 5;
    [SerializeField] private float cellSize = 32f;
    [SerializeField] private float spacing = 0f;

    private List<ItemSlot> itemSlots = new();
    private RectTransform gridRect;

    private void Start()
    {
        InitializeSlots();
        SetupGridLayout();
    }


    /// Collect all ItemSlot components inside the inventory panel.
    private void InitializeSlots()
    {
        itemSlots = new List<ItemSlot>(inventoryPanel.GetComponentsInChildren<ItemSlot>());
        gridRect = inventoryPanel.GetComponent<RectTransform>();
    }

 
    /// Configure the GridLayoutGroup based on inventory settings.
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


    /// Convert a screen position to a grid coordinate.
    public Vector2Int GetGridPosition(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridRect, screenPosition, null, out Vector2 localPos);

        float step = cellSize + spacing;
        float startX = -((columns - 1) * step) / 2f;
        float startY = ((rows - 1) * step) / 2f;

        int x = Mathf.RoundToInt((localPos.x - startX) / step);
        int y = Mathf.RoundToInt((startY - localPos.y) / step);

        return new Vector2Int(
            Mathf.Clamp(x, 0, columns - 1),
            Mathf.Clamp(y, 0, rows - 1)
        );
    }


    /// Check if an item can be placed at a given top-left position.
    public bool CanPlaceItem(Item item, Vector2Int topLeft)
    {
        if (item == null) return false;

        for (int y = 0; y < item.height; y++)
        {
            for (int x = 0; x < item.width; x++)
            {
                int gx = topLeft.x + x;
                int gy = topLeft.y + y;

                // Check if inside grid
                if (gx >= columns || gy >= rows) return false;

                // Check if slot is already occupied
                int index = gy * columns + gx;
                if (index < itemSlots.Count && itemSlots[index].item != null) return false;
            }
        }

        return true;
    }


    /// Place an item in the inventory at a given top-left position.
    public void PlaceItem(Item item, Vector2Int topLeft, int count)
    {
        if (!CanPlaceItem(item, topLeft)) return;

        for (int y = 0; y < item.height; y++)
        {
            for (int x = 0; x < item.width; x++)
            {
                int gx = topLeft.x + x;
                int gy = topLeft.y + y;
                int index = gy * columns + gx;

                if (index >= itemSlots.Count) continue;

                ItemSlot slot = itemSlots[index];
                slot.item = item;
                slot.count = count;
                slot.isCoveredSlot = !(x == 0 && y == 0);
                slot.SendMessage("UpdateGraphic", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
