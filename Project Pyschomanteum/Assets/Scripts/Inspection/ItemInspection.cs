using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInspection : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    private InventoryManager inventoryManager;
    private GameObject itemPrefab;

    protected Camera UICamera;
    protected RectTransform RawImageRectTrans;
    protected Camera RenderToTextureCamera;
    //[HideInInspector] 
    public GameObject clueFound;


    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
        UICamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (transform.name == "Item Inspection") { RawImageRectTrans = transform.GetChild(2).GetComponent<RectTransform>(); }
        RenderToTextureCamera = GameObject.Find("Item Inspection Camera").GetComponent<Camera>();
    }
    void Update() {
        if (itemPrefab != null) {
            if (itemPrefab.transform.childCount <= 0) {
                if (transform.name == "Item Inspection") { transform.GetChild(1).gameObject.SetActive(true); }
            }
        }
    }
    //When inspecting an object, create that object to be inspected
    public void OnInspect(ItemData item, bool inv = false) {
        transform.GetChild(0).gameObject.SetActive(true);
        if (!item.collected) { transform.GetChild(2).gameObject.SetActive(true); }
        if (itemPrefab != null) {
            Destroy(itemPrefab.gameObject);
        }
        itemPrefab = Instantiate(Resources.Load(item.itemName), new Vector3(10000, 10000, 10000), Quaternion.identity, GameObject.Find("ItemToInspect").transform) as GameObject;
        if (inv) {
            foreach (Transform child in itemPrefab.transform) { 
                Destroy(child.gameObject);
            }
        }
    }

    //Check if a clue is found
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.name == "Item Inspection")
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RawImageRectTrans, eventData.position, UICamera, out localPoint);
            Vector2 normalizedPoint = Rect.PointToNormalized(RawImageRectTrans.rect, localPoint);

            var renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
            if (Physics.Raycast(renderRay, out var raycastHit))
            {
                clueFound = raycastHit.collider.gameObject;
                if (clueFound.GetComponent<PhysicalClue>() != null)
                {
                    //Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
                    clueFound.GetComponent<PhysicalClue>().CollectClue();
                }
            }
            else
            { Debug.Log("No hit object"); }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        itemPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }
    public void DeleteInspectedObject() {
        if (itemPrefab != null) { 
            Destroy(itemPrefab.gameObject); 
        }
    }

    public void DisableChildren() {
        DeleteInspectedObject();
        itemPrefab = null;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}