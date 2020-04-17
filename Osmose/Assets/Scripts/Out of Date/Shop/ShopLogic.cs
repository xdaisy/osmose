public static class ShopLogic {
    public static void BuyItem(Items item, int amount, int totalCost) {
        if (amount <= 0) {
            return;
        }
        if (item.IsItem) {
            // is item
            GameManager.Instance.AddItem(item.ItemName, amount);
        } else {
            // is equipment
            GameManager.Instance.AddEquipment(item.ItemName, amount);
        }
        GameManager.Instance.Wallet -= totalCost;
    }

    public static void SellItem(Items item, int amount, int totalCost) {
        if (amount <= 0) {
            return;
        }
        if (item.IsItem) {
            // is item
            GameManager.Instance.RemoveItem(item.ItemName, amount);
        } else {
            // is equipment
            GameManager.Instance.RemoveEquipment(item.ItemName, amount);
        }
        GameManager.Instance.Wallet += totalCost;
    }
}
