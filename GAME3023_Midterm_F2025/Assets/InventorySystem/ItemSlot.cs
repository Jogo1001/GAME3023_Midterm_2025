using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Holds reference and count of items, manages their visibility in the Inventory panel
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item = null;

    [SerializeField]
    private int count = 0;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            UpdateGraphic();
        }
    }

    [SerializeField]
    Image itemIcon;

    [SerializeField]
    TextMeshProUGUI itemCountText;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;

    // Start is called before the first frame update

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    void Start()
    {
        UpdateGraphic();
    }

    //Change Icon and count
    void UpdateGraphic()
    {
        if (count < 1 || item == null)
        {
            count = 0;
            item = null;
            itemIcon.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
        else
        {
            //set sprite to the one from the item
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = count.ToString();
        }
    }

    public void UseItemInSlot()
    {
        if (CanUseItem())
        {
            item.Use();
            if (item.isConsumable)
            {
                Count--;
            }
        }
    }

    private bool CanUseItem()
    {
        return (item != null && count > 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        rectTransform.position = eventData.position;
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

