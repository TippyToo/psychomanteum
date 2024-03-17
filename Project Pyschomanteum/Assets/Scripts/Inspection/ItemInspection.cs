using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInspection : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    private InventoryManager inventoryManager;
    private GameObject itemPrefab;

    [SerializeField] protected Camera UICamera;
    [SerializeField] protected RectTransform RawImageRectTrans;
    [SerializeField] protected Camera RenderToTextureCamera;
    //[HideInInspector] 
    public GameObject clueFound;


    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
    }
    void Update() {
        if (itemPrefab != null) {
            if (itemPrefab.transform.childCount <= 0) {
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    //When inspecting an object, create that object to be inspected
    public void OnInspect(ItemData item) {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        if (itemPrefab != null) {
            Destroy(itemPrefab.gameObject);
        }
        itemPrefab = Instantiate(Resources.Load(item.itemName), new Vector3(10000, 10000, 10000), Quaternion.identity, GameObject.Find("ItemToInspect").transform) as GameObject;
    }

    //Check if a clue is found
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RawImageRectTrans, eventData.position, UICamera, out localPoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(RawImageRectTrans.rect, localPoint);

        var renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
        if (Physics.Raycast(renderRay, out var raycastHit))
        {
            clueFound = raycastHit.collider.gameObject;
            if ( clueFound.GetComponent<PhysicalClue>() != null)
            {
                //Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
                clueFound.GetComponent<PhysicalClue>().CollectClue();
            }
        }
        else
        { Debug.Log("No hit object"); }
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