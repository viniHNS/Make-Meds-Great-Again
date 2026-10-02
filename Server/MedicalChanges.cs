using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace MakeMedsGreatAgain;

/// <summary>
/// Applies the config.json changes to the vanilla medical items.
/// </summary>
[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class MedicalChanges(
    ISptLogger<MedicalChanges> logger,
    TemplateTable templateTable,
    ModConfigLoader configLoader) : IOnLoad
{
    private bool _debug;

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var config = configLoader.Config;
        if (config == null)
        {
            logger.Error("[MakeMedsGreatAgain] No valid config, medical item changes skipped.");
            return Task.CompletedTask;
        }

        _debug = config.Debug;

        foreach (var (name, id, usage) in config.UsageItems())
        {
            if (usage > 0 && TryGetItem(name, id, out var item))
            {
                item.Properties!.MaxHpResource = usage;
                Debug($"{name}: uses set to {usage}");
            }
        }

        foreach (var kit in config.MedKits())
        {
            if (kit.Enabled && TryGetItem(kit.Name, kit.Id, out var item))
            {
                ApplyKit(kit, item);
            }
        }

        logger.Success("[MakeMedsGreatAgain] Medical items updated.");
        return Task.CompletedTask;
    }

    private void ApplyKit(MedKitSettings kit, TemplateItem item)
    {
        var props = item.Properties!;
        props.MaxHpResource = kit.Hp;
        props.EffectsDamage ??= new Dictionary<DamageEffectType, EffectsDamageProperties>();

        SetEffect(kit, props.EffectsDamage, DamageEffectType.HeavyBleeding, kit.HeavyBleeding);
        SetEffect(kit, props.EffectsDamage, DamageEffectType.LightBleeding, kit.LightBleeding);
        SetEffect(kit, props.EffectsDamage, DamageEffectType.Fracture, kit.Fracture);
        SetEffect(kit, props.EffectsDamage, DamageEffectType.Contusion, new HealEffectSettings(true, 0));
        SetEffect(kit, props.EffectsDamage, DamageEffectType.RadExposure, new HealEffectSettings(true, 0));

        if (kit.Surgery.Enabled)
        {
            props.EffectsDamage[DamageEffectType.DestroyedPart] = new EffectsDamageProperties
            {
                Cost = kit.Surgery.Cost,
                Delay = 0,
                Duration = props.MedUseTime ?? 0,
                FadeOut = 0,
                HealthPenaltyMin = 25,
                HealthPenaltyMax = 45,
            };
            Debug($"{kit.Name}: surgery enabled, cost {kit.Surgery.Cost}");
        }
        else if (props.EffectsDamage.Remove(DamageEffectType.DestroyedPart))
        {
            Debug($"{kit.Name}: surgery removed");
        }

        Debug($"{kit.Name}: HP set to {kit.Hp}");
    }

    private void SetEffect(
        MedKitSettings kit,
        Dictionary<DamageEffectType, EffectsDamageProperties> effects,
        DamageEffectType type,
        HealEffectSettings settings)
    {
        if (settings.Enabled)
        {
            effects[type] = new EffectsDamageProperties { Cost = settings.Cost, Delay = 0, Duration = 0, FadeOut = 0 };
            Debug($"{kit.Name}: {type} enabled, cost {settings.Cost}");
        }
        else if (effects.Remove(type))
        {
            Debug($"{kit.Name}: {type} removed");
        }
    }

    private bool TryGetItem(string name, string id, out TemplateItem item)
    {
        item = null!;
        if (!MongoId.IsValidMongoId(id) || !templateTable.Items.TryGetValue(id, out var found) || found.Properties == null)
        {
            logger.Warning($"[MakeMedsGreatAgain] {name}: item '{id}' not found, skipped.");
            return false;
        }

        item = found;
        return true;
    }

    private void Debug(string message)
    {
        if (_debug)
        {
            logger.Info($"[MakeMedsGreatAgain] {message}");
        }
    }
}
