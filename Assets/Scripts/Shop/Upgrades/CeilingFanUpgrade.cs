using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFanUpgrade : ShopUpgrade
{
    private List<long> requirementCount;

    private void Start()
    {
        Global.InvokeLambda(() => 
        {
            targetItemName = "Ceiling Fan";
            targetShopItem = Global.itemManager.FindShopItem(targetItemName);

            requirementCount = new()
            {
                5,
                10,
                30,
                60,
                80
            };

            upgradeInfoList = new()
            {
                new Upgrade($"{targetItemName} Tier 1", $"Increase [{targetItemName}] tr/s by 1", 500, $"Requires at least {requirementCount[0]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 2", $"Decrease [{targetItemName}] cost by 10", 800, $"Requires at least {requirementCount[1]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 3", $"Increase [{targetItemName}] tr/s by 1", 3000, $"Requires at least {requirementCount[2]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 4", $"Decrease [{targetItemName}] cost by 25", 50_000, $"Requires at least {requirementCount[3]} [{targetItemName}]."),
                new Upgrade($"{targetItemName} Tier 5", $"Increase [{targetItemName}] tr/s by 10", 100_000, $"Requires at least {requirementCount[4]} [{targetItemName}].")
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
                targetShopItem.SetShopItemRate(targetShopItem.GetRate() + 1);
                break;
            case 2:
                targetShopItem.SetShopItemPrice(targetShopItem.GetPrice() - 10);
                break;
            case 3:
                targetShopItem.SetShopItemRate(targetShopItem.GetRate() + 1);
                break;
            case 4:
                targetShopItem.SetShopItemPrice(targetShopItem.GetPrice() - 25);
                break;
            case 5:
                targetShopItem.SetShopItemRate(targetShopItem.GetRate() + 10);
                break;
        }

        SetUpgrade(upgradeInfoList[GetLevel()]);
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
