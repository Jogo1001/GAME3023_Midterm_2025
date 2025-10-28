using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item = null;
    public int count = 0;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] TextMeshProUGUI statText;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;


    public bool isCoveredSlot = false;


    public int playerStat = 0;
    public int AddStats = 0;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start() => UpdateGraphic();

    void UpdateGraphic()
    {
        bool hasItem = item != null && count > 0;

        if (isCoveredSlot)
        {
            itemIcon.gameObject.SetActive(false); // don't show icon
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
            statText.text = "" + playerStat;
    }

    public void UseItemInSlot()
    {
        if (item != null && count > 0)
        {

            playerStat += AddStats;
            UpdateStatText();

           

            item.Use();
            if (item.isConsumable) count--;
            UpdateGraphic();
        }
    }

    // === DRAG & DROP ===

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false; // allow dropping
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        rectTransform.position = eventData.position; // follow mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition; // reset position
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (draggedSlot == null || draggedSlot == this) return;

        // swap items
        Item tempItem = item;
        int tempCount = count;

        item = draggedSlot.item;
        count = draggedSlot.count;

        draggedSlot.item = tempItem;
        draggedSlot.count = tempCount;

        UpdateGraphic();
        draggedSlot.UpdateGraphic();
    }
    private void StretchIcon(int widthCells, int heightCells)
    {
        float cellSize = 32f;
        float spacing = 0f;

        float w = widthCells * (cellSize + spacing) - spacing;
        float h = heightCells * (cellSize + spacing) - spacing;

        RectTransform iconRect = itemIcon.rectTransform;

        // Anchor to bottom-right
        iconRect.anchorMin = new Vector2(1, 0);
        iconRect.anchorMax = new Vector2(1, 0);
        iconRect.pivot = new Vector2(1, 0);
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.sizeDelta = new Vector2(w, h);
    }



    private void ResetIcon()
    {
        RectTransform iconRect = itemIcon.rectTransform;

        // Anchor to bottom-right
        iconRect.anchorMin = new Vector2(1, 0);
        iconRect.anchorMax = new Vector2(1, 0);
        iconRect.pivot = new Vector2(1, 0);
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.sizeDelta = new Vector2(80f, 80f);
    }


}
