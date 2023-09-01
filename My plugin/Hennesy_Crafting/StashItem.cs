namespace Hennesy_Crafting
{
    public class StashItem
    {
        public bool BIdentified;
        public int InventPosX;
        public int InventPosY;
        public string ItemClass;
        public string ItemName;
        public StashItemType ItemType;
        public bool LowLvl;
        public string StashName;

        public bool BInPlayerInventory { get; set; }
    }
}
