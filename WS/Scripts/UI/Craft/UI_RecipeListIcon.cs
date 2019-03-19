using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WS;

public class UI_RecipeListIcon : UI_ListIconBase
{

    public MasterDataCraft masterData { get; private set; }
    public bool unlock { get; private set; }
    public bool canCraft { get; private set; }

    [SerializeField]
    private Text nameTxt;

    public void Init(MasterDataCraft mData)
    {
        this.masterData = mData;
        UpdateUserData();
    }

    public void UpdateUserData()
    {
        var id = this.masterData.id;
        this.unlock = PlayerManager.Instance.playerData.HasUnlockRecipe(id);

        var namestr = "????";
        if (this.unlock)
        {
            var idata = MasterDataManager.Item.GetData(this.masterData.id);
            namestr = idata.name;
        }
        nameTxt.text = namestr;

        var ilist = unlock ? this.masterData.materialItems : this.masterData.showRecipeItems;
        this.canCraft = ItemManager.Instance.CheckItem(ilist);
        nameTxt.color = canCraft ? Color.white : Color.gray;

    }
    public void CheckCanCraft()
    {
        //

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        MyEventSystem.SendEvent(new MyGameEvent(MyGameEvent.UICRAFT_CLICK_RECIPE, this));
        
    }
}
