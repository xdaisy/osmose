public static class ShopLogic {
    public static void BuyItem(Items item, int amount, int totalCost) {
        GameManager.Instance.AddItem(item.ItemName, amount);
        GameManager.Instance.Wallet -= totalCost;
    }

    public static void SellItem(Items item, int amount, int totalCost) {
        if (item.IsItem) {
            GameManager.Instance.RemoveItem(item.ItemName, amount);
        } else {
            GameManager.Instance.RemoveEquipment(item.ItemName, amount);
        }
        GameManager.Instance.Wallet += totalCost;
    }
}
