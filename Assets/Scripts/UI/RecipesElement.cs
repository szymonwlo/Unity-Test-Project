using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RecipesElement : MonoBehaviour
{
    [SerializeField] private Button ButtonElement;
    [SerializeField] private TextMeshProUGUI Time;
    [SerializeField] private TextMeshProUGUI Success;
    [SerializeField] private RectTransform RecipesContainer;
    [SerializeField] private GameObject Icon;
    [SerializeField] private GameObject Text;

    private FullRecipe FullRecipe;
    private IMachineElement MachineElement;
    private Color DefaultButtonColor;
    private Image ButtonImage;
    private int ID;

    public void Init(int _ID, FullRecipe _FullRecipe, IMachineElement _MachineElement)
    {
        ID = _ID;
        FullRecipe = _FullRecipe;
        MachineElement = _MachineElement;


        ButtonImage = GetComponent<Image>();


        DefaultButtonColor = ButtonImage.color;

        BonusItem bonusItem = ItemsManager.ItemsManagerView.GetGlobalBonus();

        Time.text = Mathf.Max(_FullRecipe.Time - bonusItem.ReducesCraftingTime,0) + "s";
        Success.text = (int)Math.Round(Mathf.Clamp(_FullRecipe.Success + bonusItem.IncreasesSuccessRate,0,1)  * 100) + "%";

        for(int i = 0; i < FullRecipe.ItemsIn.Count; i++)
        {
            bool isLast = i < FullRecipe.ItemsIn.Count-1;
            if(!isLast && i > 0)
                SpawnText("+");

            SpawnIcon(FullRecipe.ItemsIn[i].Icon);
            
        }

        SpawnText("=");
        SpawnIcon(FullRecipe.ItemOut.Icon);
    }

    private void SpawnIcon(Sprite sprite)
    {
        GameObject gm = Instantiate(Icon, RecipesContainer);
        Image image = gm.GetComponent<Image>();
        image.sprite = sprite;
    }

    private void SpawnText(string text)
    {
        GameObject gm = Instantiate(Text, RecipesContainer);
        TextMeshProUGUI textMeshPro = gm.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = text;
    }

    public void Active(bool result)
    {
        ButtonImage.color = !result ? DefaultButtonColor : Color.red;
    }

    public void ClickElement()
    {
        MachineElement.ClickRecipes(ID);
    }
}
