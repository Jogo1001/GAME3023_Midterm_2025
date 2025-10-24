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
        itemIcon.gameObject.SetActive(hasItem);
        itemCountText.gameObject.SetActive(hasItem);

        if (hasItem)
        {
            itemIcon.sprite = item.icon;
            itemCountText.text = count.ToString();
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
}
