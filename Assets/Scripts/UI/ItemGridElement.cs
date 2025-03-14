using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemGridElement : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI CurrentAmount;
    private IItem Item;
    private IMainMenu MainMenu;
    private int ID;

    void OnEnable()
    {
        ItemsManager.ItemsManagerView.RefreshItems += RefreshElement;
    }

    void OnDisable()
    {
        ItemsManager.ItemsManagerView.RefreshItems -= RefreshElement;
    }

    public void RefreshElement()
    {
        if(Item != null)
            CurrentAmount.text = Item.CurrentAmount.ToString();
    }

    public void Init(IMainMenu _MainMenu, int _ID)
    {
        MainMenu = _MainMenu;
        ID = _ID;
    }

    public void SetItem(IItem _Item = null)
    {
        if(_Item == null)
        {
            Item = null;
            Icon.sprite = null;
            CurrentAmount.text = string.Empty;
        }
        else
        {
            Item = _Item;
            Icon.sprite = Item.Icon;
            CurrentAmount.text = Item.CurrentAmount.ToString();
        }
    }

    public void PointerEnter()
    {
        MainMenu.SelectItemInInventory(Item);
    }

    public void PointerExit()
    {
        MainMenu.DeselectItemInInventory();
    }

}
