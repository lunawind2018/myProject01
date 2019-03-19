using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyEvent;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UICraft : UIWindow
    {
        [SerializeField] private UI_RecipeListIcon recipeIconPrefab;
        [SerializeField] private Transform recipeParent;

        private List<UI_RecipeListIcon> recipeIconList = new List<UI_RecipeListIcon>();
        private UI_RecipeListIcon currSelectRecipe;

        //recipe detail
        [SerializeField] private Text recipeItemNameTxt;
        [SerializeField] private GameObject recipeDetailObj;
        [SerializeField] private List<UI_ItemIconSmall> matIconList;
        [SerializeField] private List<Transform> positionList;
        [SerializeField] private Transform arrow;
        [SerializeField] private Text recipeItemNumTxt;

        //
        [SerializeField] private Button btnCraft;
        private Text btnCraftText;
        private CanvasGroup btnCraftEffect;
        private RectTransform btnCraftEffectTrans;
        private float btnDefaultWidth;
        //

        private List<List<Transform>> positionVecList; 
        protected override void Awake()
        {
            base.Awake();
            recipeDetailObj.SetActive(false);
            positionVecList = new List<List<Transform>>();
            for (int i = 0; i < positionList.Count; i++)
            {
                var l = i + 2;
                var root = positionList[i];
                var posList = new List<Transform>();
                for (int j = 0; j < l; j++)
                {
                    var trans = root.Find("item_" + j);
                    if (trans == null)
                    {
                        Debug.LogError("missing pos "+i+j);
                    }
                    posList.Add(trans);
                }
                var arrow = root.Find("arrow");
                posList.Add(arrow);
                positionVecList.Add(posList);
            }
            foreach (var icon in matIconList)
            {
                icon.SetActive(false);
            }

            this.btnCraftText = btnCraft.transform.Find("Text").GetComponent<Text>();
            var effect = btnCraft.transform.Find("effectmask");
            this.btnCraftEffect = effect.GetComponent<CanvasGroup>();
            this.btnCraftEffectTrans = effect.GetComponent<RectTransform>();
            this.btnDefaultWidth = this.btnCraftEffectTrans.sizeDelta.x;
#if UNITY_EDITOR
            if (btnCraftText == null)
            {
                Debug.LogError(".......");
            }
            if (btnCraftEffect == null)
            {
                Debug.LogError(".......");
            }
#endif
            this.btnCraftEffect.gameObject.SetActive(false);
        }

        protected override void RegisterEvent()
        {
            MyEventSystem.RegistEvent(MyGameEvent.UICRAFT_CLICK_RECIPE, OnClickRecipe);
        }

        protected override void UnRegisterEvent()
        {
            MyEventSystem.UnRegistEvent(MyGameEvent.UICRAFT_CLICK_RECIPE, OnClickRecipe);
        }

        private void OnClickRecipe(MyEvent.MyEvent obj)
        {
            var icon = obj.data as UI_RecipeListIcon;
            //Debug.Log("click " + icon.masterData.id);
            if (currSelectRecipe != null)
            {
                currSelectRecipe.SetSelect(false);
            }
            icon.SetSelect(true);
            this.currSelectRecipe = icon;
            ShowRecipe();
        }

        private void UpdateCraftBtn()
        {
            var recipeUnlock = this.currSelectRecipe.unlock;
            this.btnCraftText.text = recipeUnlock ? ConstTextManager.Get(TextId.Craft_Btn_Craft) : ConstTextManager.Get(TextId.Craft_Btn_Unlock);
            var state = this.currSelectRecipe.canCraft;
            this.btnCraft.enabled = state;
            this.btnCraftText.color = state ? Color.black : Color.gray;
        }

        private void ShowRecipe()
        {
            recipeDetailObj.SetActive(true);
            UpdateCraftBtn();
            this.recipeItemNumTxt.text = this.currSelectRecipe.masterData.get_num + "";
            var recipeUnlock = this.currSelectRecipe.unlock;
            var matlist = recipeUnlock ? this.currSelectRecipe.masterData.materialItems : this.currSelectRecipe.masterData.showRecipeItems;
            this.recipeItemNameTxt.text = recipeUnlock ? MasterDataManager.Item.GetData(this.currSelectRecipe.masterData.id).name : "????";
            var l = matlist.Count;
            for (int i = 0; i <= l; i++)
            {
                matIconList[i].SetActive(true);
                matIconList[i].transform.localPosition = positionVecList[l - 1][i].localPosition;
                if (i == 0)
                {
                    var iid = this.currSelectRecipe.masterData.id;
                    var unlock = ItemManager.Instance.HasUnlock(iid);
                    matIconList[i].Init(iid, unlock);
                    matIconList[i].SetNum(ItemManager.Instance.GetNum(iid));
                }
                else
                {
                    var iid = matlist[i - 1].id;
                    var need = matlist[i - 1].num;
                    var unlock = ItemManager.Instance.HasUnlock(iid);
                    matIconList[i].Init(iid, unlock);
                    matIconList[i].SetNum(ItemManager.Instance.GetNum(iid), need);
                }
            }
            for (int i = l + 1; i < matIconList.Count; i++)
            {
                matIconList[i].SetActive(false);
            }
            arrow.localPosition = positionVecList[l - 1][l + 1].localPosition;

        }

        public override void Init(params object[] args)
        {
            base.Init(args);
            StartCoroutine(InitRecipes(0, OnInitComplete));
        }

        private IEnumerator InitRecipes(int type, System.Action cb )
        {
            var masterList = MasterDataManager.Craft.GetAll();
            foreach (var masterData in masterList)
            {
                var icon = Instantiate(recipeIconPrefab);
                icon.Init(masterData);
                icon.name = "recipe_" + masterData.id;
                Utils.SetParent(icon.transform, recipeParent);
                
                this.recipeIconList.Add(icon);
            }
            yield return 0;
        }

        private void OnInitComplete()
        {
            
        }

        private float craftTime;

        private bool isCrafting;

        private Tweener currTweener;
        public void OnClickCraft()
        {
            if (this.isCrafting)
            {
                CancelCraft();
                UpdateCraftBtn();
                return;
            }
            if (this.Busy) return;
            if (this.currSelectRecipe == null) return;
            this.Busy = true;
            this.craftTime = 0;
            this.btnCraftEffect.gameObject.SetActive(true);
            this.btnCraftText.text = ConstTextManager.Get(TextId.Cancel);
            Utils.SetWidth(this.btnCraftEffectTrans, 0);
            this.isCrafting = true;
            this.currTweener = DOTween.To(() => this.craftTime, x => this.craftTime = x, 5, 5).SetEase(Ease.Linear).OnUpdate(
                () =>
                {
                    Utils.SetWidth(this.btnCraftEffectTrans, this.btnDefaultWidth * this.craftTime / 5f);
                }).OnComplete(
                () =>
                {
                    Debug.Log("......");
                    var t = 0f;
                    this.btnCraftEffectTrans.pivot = new Vector2(0.5f, 0.5f);
                    this.currTweener = DOTween.To((x) => t = x, 1, 0, 0.1f).OnUpdate(() =>
                    {
                        this.btnCraftEffect.alpha = t;
                        this.btnCraftEffect.transform.localScale = (5 - 4 * t) * Vector3.one;
                    }).OnComplete(() =>
                    {
                        Debug.Log(".......2");
                        this.btnCraftEffectTrans.pivot = new Vector2(0f, 0.5f);
                        var param = new Hashtable()
                        {
                            {"id",this.currSelectRecipe.masterData.id}
                        };
                        GameManager.Instance.SendCommand(CommandType.Craft, param, OnCraftComplete);
                        this.currTweener = null;
                    });
                });
        }

        private void CancelCraft()
        {
            if (this.currTweener != null)
            {
                this.currTweener.Kill();
                this.currTweener = null;
            }
            this.isCrafting = false;
            this.Busy = false;
            this.btnCraftEffect.alpha = 1f;
            this.btnCraftEffectTrans.pivot = new Vector2(0f, 0.5f);
            this.btnCraftEffect.transform.localScale = Vector3.one;
            Utils.SetWidth(this.btnCraftEffectTrans, this.btnDefaultWidth);
            this.craftTime = 0;
            this.btnCraftEffect.gameObject.SetActive(false);
        }
        
        private void OnCraftComplete(bool success)
        {
            Debug.Log("craft complete " + success);
            if (success)
            {
                UpdateUI();
            }
            CancelCraft();
        }

        private void UpdateUI()
        {
            foreach (var recipeListIcon in recipeIconList)
            {
                recipeListIcon.UpdateUserData();
            }
            ShowRecipe();
        }
    }
}