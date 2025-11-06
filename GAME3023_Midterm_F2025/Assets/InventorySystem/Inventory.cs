using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Grid Settings")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private int columns = 6;
    [SerializeField] private int rows = 5;
    [SerializeField] public float cellSize = 32f;
    [SerializeField] public float spacing = 0f;

    private List<ItemSlot> itemSlots = new();
    private RectTransform gridRect;

    [Header("UI References")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;


    void Start()
    {
        // Get all item slots in the inventory panel
        itemSlots = new List<ItemSlot>(inventoryPanel.GetComponentsInChildren<ItemSlot>());
        gridRect = inventoryPanel.GetComponent<RectTransform>();

        // Set up the grid layout
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

    public Vector2Int GetGridPosition(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridRect, screenPos, null, out Vector2 local
        );

        float step = cellSize + spacing;
        float startX = -((columns - 1) * step) / 2f;
        float startY = ((rows - 1) * step) / 2f;

        int x = Mathf.RoundToInt((local.x - startX) / step);
        int y = Mathf.RoundToInt((startY - local.y) / step);

        return new Vector2Int(
            Mathf.Clamp(x, 0, columns - 1),
            Mathf.Clamp(y, 0, rows - 1)
        );
    }

    public bool CanPlaceItem(Item item, Vector2Int topLeft)
    {
        if (item == null) return false;

        for (int y = 0; y < item.height; y++)
        {
            for (int x = 0; x < item.width; x++)
            {
                int gx = topLeft.x + x;
                int gy = topLeft.y + y;

                if (gx >= columns || gy >= rows)
                    return false;

                int index = gy * columns + gx;
                if (index < itemSlots.Count && itemSlots[index].item != null)
                    return false;
            }
        }

        return true;
    }

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

                if (index < itemSlots.Count)
                {
                    ItemSlot slot = itemSlots[index];
                    slot.item = item;
                    slot.count = count;
                    slot.isCoveredSlot = !(x == 0 && y == 0);
                    slot.SendMessage("UpdateGraphic", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
