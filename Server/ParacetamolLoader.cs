using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services.Modding.Custom;

namespace MakeMedsGreatAgain;

/// <summary>
/// Creates the Paracetamol item and adds it to Therapist, static loot and bot loot.
/// Runs at Preload + 1 because new items must exist before profiles load.
/// </summary>
[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class ParacetamolLoader(
    ISptLogger<ParacetamolLoader> logger,
    CustomItemService customItemService,
    TemplateTable templateTable,
    TradersTable tradersTable,
    LocationTable locationTable,
    BotTable botTable,
    ModConfigLoader configLoader) : IOnLoad
{
    // Fixed ids: profiles keep references to them, so they must never change.
    private static readonly MongoId ParacetamolId = new("6990c74d8f7e024b8aee1a40");
    private static readonly MongoId TherapistOfferId = new("6990c875d1965e370c50c144");

    private static readonly MongoId AnalginId = new("544fb37f4bdc2dee738b4567");
    private const string DrugsParentId = "5448f3a14bdc2d27728b4569";
    private const string HandbookPillsId = "5b47574386f77428ca22b337";

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var config = configLoader.Config ?? new ModConfig();

        var result = customItemService.CreateItemFromClone(new NewItemFromCloneDetails
        {
            NewId = ParacetamolId,
            ItemTplToClone = AnalginId,
            ParentId = DrugsParentId,
            NewItemName = "painkiller_paracetamol",
            HandbookParentId = HandbookPillsId,
            HandbookPriceRoubles = 4003,
            FleaPriceRoubles = 6005,
            Locales = new()
            {
                ["en"] = new LocaleDetails
                {
                    Name = "Paracetamol painkillers",
                    ShortName = "Paracetamol",
                    Description = "A common painkiller that can be used to treat pain and fever.",
                },
            },
        });

        if (!result.Success)
        {
            logger.Error($"[MakeMedsGreatAgain] Failed to create Paracetamol: {string.Join("; ", result.Errors ?? [])}");
            return Task.CompletedTask;
        }

        // Applied after the clone instead of through OverrideProperties, which also copies non-nullable defaults.
        var props = templateTable.Items[ParacetamolId].Properties!;
        props.ExaminedByDefault = true;
        props.EffectsDamage = new Dictionary<DamageEffectType, EffectsDamageProperties>
        {
            [DamageEffectType.Pain] = new() { Delay = 0, Duration = 60, FadeOut = 15 },
        };

        AddToTherapist(config.ParacetamolTraderPrice);
        AddToStaticLoot(config.ParacetamolLootMultiplier);
        AddToBotLoot();

        logger.Success("[MakeMedsGreatAgain] Paracetamol added.");
        return Task.CompletedTask;
    }

    private void AddToTherapist(double price)
    {
        if (!tradersTable.TryGetValue(Traders.THERAPIST, out var therapist) || therapist?.Assort == null)
        {
            logger.Warning("[MakeMedsGreatAgain] Therapist not found, Paracetamol will not be sold.");
            return;
        }

        var assort = therapist.Assort;
        if (assort.Items.Any(item => item.Id == TherapistOfferId))
        {
            return;
        }

        assort.Items.Add(new Item
        {
            Id = TherapistOfferId,
            Template = ParacetamolId,
            ParentId = "hideout",
            SlotId = "hideout",
            Upd = new Upd { UnlimitedCount = true, StackObjectsCount = 99 },
        });
        assort.BarterScheme[TherapistOfferId] = [[new BarterScheme { Count = price, Template = Money.ROUBLES }]];
        assort.LoyalLevelItems[TherapistOfferId] = 1;
    }

    /// <summary>
    /// Every static container that can hold Analgin can also hold Paracetamol, weighted relative to Analgin.
    /// </summary>
    private void AddToStaticLoot(double multiplier)
    {
        if (multiplier <= 0)
        {
            return;
        }

        foreach (var location in locationTable.GetDictionary().Values)
        {
            location.StaticLoot?.AddTransformer(staticLoot =>
            {
                if (staticLoot == null)
                {
                    return staticLoot;
                }

                foreach (var container in staticLoot.Values)
                {
                    var distribution = container.ItemDistribution?.ToList();
                    var analgin = distribution?.FirstOrDefault(entry => entry.Tpl == AnalginId);
                    if (distribution == null || analgin == null || distribution.Any(entry => entry.Tpl == ParacetamolId))
                    {
                        continue;
                    }

                    distribution.Add(new ItemDistribution
                    {
                        Tpl = ParacetamolId,
                        RelativeProbability = (float)Math.Max(1, Math.Round((analgin.RelativeProbability ?? 0) * multiplier)),
                    });
                    container.ItemDistribution = distribution;
                }

                return staticLoot;
            });
        }
    }

    /// <summary>
    /// Bots carry Paracetamol wherever they carry Analgin, with the same weight.
    /// </summary>
    private void AddToBotLoot()
    {
        foreach (var bot in botTable.Types.Values)
        {
            var pools = bot?.BotInventory?.Items;
            if (pools == null)
            {
                continue;
            }

            foreach (var pool in new[] { pools.Backpack, pools.Pockets, pools.SecuredContainer, pools.SpecialLoot, pools.TacticalVest })
            {
                if (pool != null && pool.TryGetValue(AnalginId, out var weight))
                {
                    pool[ParacetamolId] = weight;
                }
            }
        }
    }
}
