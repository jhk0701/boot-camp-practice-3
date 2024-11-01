using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] UIInventory uIInventoryPrefab;
    public UIInventory uIInventory;

    void Awake()
    {
        uIInventory = Instantiate(uIInventoryPrefab, transform);    
    }
}
