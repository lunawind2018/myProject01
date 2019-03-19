using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UIBag : UIWindow
    {
        [SerializeField]
        private UI_ItemIconSmall itemIconSmall;

        [SerializeField] 
        private Transform itemContent;

        private GridLayoutGroup uiGrid;

        protected override void Awake()
        {
            base.Awake();
            this.uiGrid = itemContent.GetComponent<GridLayoutGroup>();
        }

        public override void Init(params object[]args)
        {
            var items = ItemManager.Instance.GetItemDic();
            foreach (var item in items)
            {
                if (item.Value <= 0) continue;
                var icon = Instantiate(itemIconSmall);
                icon.Init(item.Key, true, 2);
                icon.SetNum(item.Value);
                Utils.SetParent(icon.transform, itemContent);
            }
        }
    }
}