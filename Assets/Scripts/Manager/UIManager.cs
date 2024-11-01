using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] UIInventory uiInventoryPrefab;
    public UIInventory uiInventory;

    void Awake()
    {
        uiInventory = Instantiate(uiInventoryPrefab, transform);
        uiInventory.Initialize();
    }
}
