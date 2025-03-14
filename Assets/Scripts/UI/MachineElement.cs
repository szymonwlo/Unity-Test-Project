using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MachineElement : MonoBehaviour, IMachineElement
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private Slider Slider;
    [SerializeField] private RectTransform Working;
    [SerializeField] private RectTransform Locked;
    [SerializeField] private TextMeshProUGUI LockedText;
    [SerializeField] private Button CraftButton;
    [SerializeField] private RecipesElement[] RecipesElements = new RecipesElement[2];

    private IMachineView MachineView;
    private IMainMenu MainMenu;
    private List<FullRecipe> FullRecipes;
    private int CurrentRecipesID;

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
       CraftButton.interactable = FullRecipes[CurrentRecipesID].ItemsIn.Count( x => x.CurrentAmount < 1) == 0;
       Locked.gameObject.SetActive(!MachineView.Unlocked);

       if(!MachineView.Unlocked)
            LockedText.text = " Quests: " + MachineView.Quest.Name + "\n<size=40%>" + MachineView.Quest.Description;
    }

    public void Init(IMachineView _MachineView, IMainMenu _MainMenu)
    {
        MachineView = _MachineView;
        MainMenu = _MainMenu;

        Icon.sprite = MachineView.Icon;
        Name.text = MachineView.Name;
        Slider.value = 0;

        Working.gameObject.SetActive(false);
        Locked.gameObject.SetActive(!MachineView.Unlocked);
        if(!MachineView.Unlocked)
            LockedText.text = " Quests: " + MachineView.Quest.Name + "\n<size=40%>" + MachineView.Quest.Description;
        
        CraftButton.interactable = false;


        FullRecipes = _MachineView.GetFullRecipe();

        for(int i = 0; i < RecipesElements.Length; i++)
        {
            RecipesElements[i].Init(i, FullRecipes[i], this);
        }

        ClickRecipes(0);
    }


    public void Update()
    {
        if(MachineView.Working)
        {
            Working.gameObject.SetActive(true);
            Slider.value = MachineView.Progress;
        }
        else
        {
            Slider.value = 0;
            Working.gameObject.SetActive(false);
        }
    }

    public void ClickRecipes(int _ID)
    {
        CurrentRecipesID = _ID;
        for(int i = 0; i < RecipesElements.Length; i++)
        {
            RecipesElements[i].Active(i == _ID);
        }

        CraftButton.interactable = FullRecipes[_ID].ItemsIn.Count( x => x.CurrentAmount < 1) == 0;
    }

    public void ClickCraft()
    {
        MachineView.Craft(FullRecipes[CurrentRecipesID]);
    }
}

public interface IMachineElement
{
    void ClickRecipes(int _ID);
}
