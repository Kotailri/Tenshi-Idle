using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrangeaBouquetUpgrade : ShopUpgrade
{
    private List<long> requirementCount;

    private void Start()
    {
        Global.InvokeLambda(() => 
        {
            targetItemName = "Hydrangea Bouquet";
            targetShopItem = Global.itemManager.FindShopItem(targetItemName);

            requirementCount = new()
            {
                1,
                5,
                8,
                9,
                10
            };

            upgradeInfoList = new()
            {
                new Upgrade($"{targetItemName} Tier 1", $"Decrease [{targetItemName}] cost by 5%",        10_000_000_000, $"Requires at least {requirementCount[0]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 2", $"Increase [{targetItemName}] tr/s by 5%",        10_000_000_000, $"Requires at least {requirementCount[1]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 3", $"Decrease [{targetItemName}] cost by 20%",       40_000_000_000, $"Requires at least {requirementCount[2]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 4", $"Increase [{targetItemName}] tr/s by 20%",       40_000_000_000, $"Requires at least {requirementCount[3]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 5", $"Increase [{targetItemName}] tr/s by 8x",    15_000_000_000_000, $"Requires at least {requirementCount[4]} [{targetItemName}].")
            };

            UpdateMaxLevel(upgradeInfoList.Count);
            SetUpgrade(upgradeInfoList[0]);
        }, 0.1f);
    }

    public override void ApplyUpgrade()
    {
        GetComponent<HasHoverInfo>().Refresh();
        base.ApplyUpgrade();

        switch (GetLevel())
        {
            case 1:
                targetShopItem.SetShopItemPrice(targetShopItem.GetPrice() - (long)(targetShopItem.GetPrice() * 0.05));
                break;
            case 2:
                targetShopItem.SetShopItemRate((long)(targetShopItem.GetRate() * 1.05));
                break;
            case 3:
                targetShopItem.SetShopItemPrice(targetShopItem.GetPrice() - (long)(targetShopItem.GetPrice() * 0.2));
                break;
            case 4:
                targetShopItem.SetShopItemRate((long)(targetShopItem.GetRate() * 1.2));
                break;
            case 5:
                targetShopItem.SetShopItemRate((long)(targetShopItem.GetRate() * 8));
                break;
        }

        if (GetLevel() < upgradeInfoList.Count)
        {
            SetUpgrade(upgradeInfoList[GetLevel()]);
        }
    }

    public override bool CheckRequirements()
    {
        GetComponent<HasHoverInfo>().Refresh();

        if (!targetShopItem)
            return false;

        switch (GetLevel())
        {
            case 0:
                return targetShopItem.GetCount() >= requirementCount[0];
            case 1:
                return targetShopItem.GetCount() >= requirementCount[1];
            case 2:
                return targetShopItem.GetCount() >= requirementCount[2];
            case 3:
                return targetShopItem.GetCount() >= requirementCount[3];
            case 4:
                return targetShopItem.GetCount() >= requirementCount[4];
        }
        return true;
    }

}
