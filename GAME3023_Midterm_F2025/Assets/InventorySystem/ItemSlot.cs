using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Item Info")]
    public Item item = null;
    public int count = 0;

    [Header("UI Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private TextMeshProUGUI statText;

    [Header("Slot Settings")]
    public bool isCoveredSlot = false;
    public ItemSlot linkedSlot;

    [Header("Player Stats")]
    public int playerStat = 0;
    public int AddStats = 0;

    // references
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;
    private Inventory inventory;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventory = GetComponentInParent<Inventory>();
        UpdateGraphic();
    }

    //UI Updates
    void UpdateGraphic()
    {
        bool hasItem = item != null && count > 0;

        if (isCoveredSlot)
        {
            itemIcon.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
            return;
        }

        itemIcon.gameObject.SetActive(hasItem);
        itemCountText.gameObject.SetActive(hasItem);

        if (hasItem)
        {
            itemIcon.sprite = item.icon;
            itemCountText.text = count.ToString();
            StretchIcon(item.width, item.height);
        }
        else
        {
            ResetIcon();
        }
    }

    void UpdateStatText()
    {
        if (statText != null)
            statText.text = playerStat.ToString();
    }

    //Item Usage
    public void UseItemInSlot()
    {
        if (item != null && count > 0)
        {
            playerStat += AddStats;
            UpdateStatText();

            /* item.Use();*/
            if (inventory != null)
                item.Use(inventory.itemNameText, inventory.itemDescriptionText);

            if (item.isConsumable)
                count--;

            UpdateGraphic();
        }
    }

    //  DRAG & DROP
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        if (linkedSlot != null)
        {
            linkedSlot.originalParent = linkedSlot.transform.parent;
            linkedSlot.originalPosition = linkedSlot.rectTransform.anchoredPosition;
        }

        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;

        if (linkedSlot != null)
            linkedSlot.transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;

        rectTransform.position = eventData.position;

        if (linkedSlot != null)
            linkedSlot.rectTransform.position = eventData.position + new Vector2(-rectTransform.rect.width, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;

        if (linkedSlot != null)
        {
            linkedSlot.transform.SetParent(transform.parent, false);
            int mainIndex = transform.GetSiblingIndex();
            linkedSlot.transform.SetSiblingIndex(mainIndex);
            linkedSlot.rectTransform.anchoredPosition = linkedSlot.originalPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (draggedSlot == null || draggedSlot == this) return;

        Item tempItem = item;
        int tempCount = count;

        item = draggedSlot.item;
        count = draggedSlot.count;

        draggedSlot.item = tempItem;
        draggedSlot.count = tempCount;

        UpdateGraphic();
        draggedSlot.UpdateGraphic();
    }

    //ICON MANAGEMENT
    private void StretchIcon(int widthCells, int heightCells)
    {
        float cellSize = 32f;
        float spacing = 0f;

        float w = widthCells * (cellSize + spacing) - spacing;
        float h = heightCells * (cellSize + spacing) - spacing;

        RectTransform iconRect = itemIcon.rectTransform;
        iconRect.anchorMin = new Vector2(1, 0);
        iconRect.anchorMax = new Vector2(1, 0);
        iconRect.pivot = new Vector2(1, 0);
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.sizeDelta = new Vector2(w, h);
    }

    private void ResetIcon()
    {
        RectTransform iconRect = itemIcon.rectTransform;
        iconRect.anchorMin = new Vector2(1, 0);
        iconRect.anchorMax = new Vector2(1, 0);
        iconRect.pivot = new Vector2(1, 0);
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.sizeDelta = new Vector2(80f, 80f);
    }
}
