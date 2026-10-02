using System.Reflection;
using System.Text.Json;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers.Server;
using Path = System.IO.Path;

namespace MakeMedsGreatAgain;

/// <summary>
/// Loads config.json once and shares it between the loaders.
/// </summary>
[Injectable(InjectionType.Singleton)]
public class ModConfigLoader(ISptLogger<ModConfigLoader> logger, ModHelper modHelper)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    private ModConfig? _config;
    private bool _loaded;

    /// <summary>
    /// The parsed config, or null if the file is missing or invalid.
    /// </summary>
    public ModConfig? Config
    {
        get
        {
            if (!_loaded)
            {
                _config = Load();
                _loaded = true;
            }

            return _config;
        }
    }

    private ModConfig? Load()
    {
        var path = Path.Combine(modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()), "config.json");

        try
        {
            return JsonSerializer.Deserialize<ModConfig>(File.ReadAllText(path), JsonOptions);
        }
        catch (Exception ex)
        {
            logger.Error($"[MakeMedsGreatAgain] Failed to load {path}: {ex.Message}");
            return null;
        }
    }
}

public class ModConfig
{
    public bool Debug { get; set; }

    // Paracetamol
    public double ParacetamolTraderPrice { get; set; } = 5532;
    public double ParacetamolLootMultiplier { get; set; } = 0.5;

    // Item ids
    public string CalocId { get; set; } = string.Empty;
    public string ArmyBandageId { get; set; } = string.Empty;
    public string AnalginPainkillersId { get; set; } = string.Empty;
    public string AugmentinId { get; set; } = string.Empty;
    public string IbuprofenId { get; set; } = string.Empty;
    public string VaselinId { get; set; } = string.Empty;
    public string GoldenStarId { get; set; } = string.Empty;
    public string AluminiumSplintId { get; set; } = string.Empty;
    public string SurvivalKitId { get; set; } = string.Empty;
    public string CmsId { get; set; } = string.Empty;
    public string GrizzlyId { get; set; } = string.Empty;
    public string Ai2Id { get; set; } = string.Empty;
    public string CarKitId { get; set; } = string.Empty;
    public string SalewaId { get; set; } = string.Empty;
    public string IfakId { get; set; } = string.Empty;
    public string AfakId { get; set; } = string.Empty;

    // Uses (0 = keep the game value)
    public int CalocUsage { get; set; }
    public int ArmyBandageUsage { get; set; }
    public int AnalginPainkillersUsage { get; set; }
    public int AugmentinUsage { get; set; }
    public int IbuprofenUsage { get; set; }
    public int VaselinUsage { get; set; }
    public int GoldenStarUsage { get; set; }
    public int AluminiumSplintUsage { get; set; }
    public int SurvivalKitUsage { get; set; }
    public int CmsUsage { get; set; }

    // Grizzly
    public bool GrizzlyChanges { get; set; }
    public int GrizzlyHP { get; set; }
    public bool GrizzlyCanHealHeavyBleeding { get; set; }
    public int GrizzlyHeavyBleedingHealCost { get; set; }
    public bool GrizzlyCanHealLightBleeding { get; set; }
    public int GrizzlyLightBleedingHealCost { get; set; }
    public bool GrizzlyCanHealFractures { get; set; }
    public int GrizzlyFractureHealCost { get; set; }
    public bool GrizzlyCanDoSurgery { get; set; }
    public int GrizzlySurgeryCost { get; set; }

    // AI-2
    public bool Ai2Changes { get; set; }
    public int Ai2HP { get; set; }
    public bool Ai2CanHealHeavyBleeding { get; set; }
    public int Ai2HeavyBleedingHealCost { get; set; }
    public bool Ai2CanHealLightBleeding { get; set; }
    public int Ai2LightBleedingHealCost { get; set; }
    public bool Ai2CanHealFractures { get; set; }
    public int Ai2FractureHealCost { get; set; }
    public bool Ai2CanDoSurgery { get; set; }
    public int Ai2SurgeryCost { get; set; }

    // Car First Aid Kit
    public bool CarKitChanges { get; set; }
    public int CarKitHP { get; set; }
    public bool CarKitCanHealHeavyBleeding { get; set; }
    public int CarKitHeavyBleedingHealCost { get; set; }
    public bool CarKitCanHealLightBleeding { get; set; }
    public int CarKitLightBleedingHealCost { get; set; }
    public bool CarKitCanHealFractures { get; set; }
    public int CarKitFractureHealCost { get; set; }
    public bool CarKitCanDoSurgery { get; set; }
    public int CarKitSurgeryCost { get; set; }

    // Salewa
    public bool SalewaChanges { get; set; }
    public int SalewaHP { get; set; }
    public bool SalewaCanHealHeavyBleeding { get; set; }
    public int SalewaHeavyBleedingHealCost { get; set; }
    public bool SalewaCanHealLightBleeding { get; set; }
    public int SalewaLightBleedingHealCost { get; set; }
    public bool SalewaCanHealFractures { get; set; }
    public int SalewaFractureHealCost { get; set; }
    public bool SalewaCanDoSurgery { get; set; }
    public int SalewaSurgeryCost { get; set; }

    // IFAK
    public bool IfakChanges { get; set; }
    public int IfakHP { get; set; }
    public bool IfakCanHealHeavyBleeding { get; set; }
    public int IfakHeavyBleedingHealCost { get; set; }
    public bool IfakCanHealLightBleeding { get; set; }
    public int IfakLightBleedingHealCost { get; set; }
    public bool IfakCanHealFractures { get; set; }
    public int IfakFractureHealCost { get; set; }
    public bool IfakCanDoSurgery { get; set; }
    public int IfakSurgeryCost { get; set; }

    // AFAK
    public bool AfakChanges { get; set; }
    public int AfakHP { get; set; }
    public bool AfakCanHealHeavyBleeding { get; set; }
    public int AfakHeavyBleedingHealCost { get; set; }
    public bool AfakCanHealLightBleeding { get; set; }
    public int AfakLightBleedingHealCost { get; set; }
    public bool AfakCanHealFractures { get; set; }
    public int AfakFractureHealCost { get; set; }
    public bool AfakCanDoSurgery { get; set; }
    public int AfakSurgeryCost { get; set; }

    public IEnumerable<(string Name, string Id, int Usage)> UsageItems() =>
    [
        ("Caloc-B", CalocId, CalocUsage),
        ("Army bandage", ArmyBandageId, ArmyBandageUsage),
        ("Analgin", AnalginPainkillersId, AnalginPainkillersUsage),
        ("Augmentin", AugmentinId, AugmentinUsage),
        ("Ibuprofen", IbuprofenId, IbuprofenUsage),
        ("Vaseline", VaselinId, VaselinUsage),
        ("Golden Star", GoldenStarId, GoldenStarUsage),
        ("Aluminum splint", AluminiumSplintId, AluminiumSplintUsage),
        ("Surv12", SurvivalKitId, SurvivalKitUsage),
        ("CMS", CmsId, CmsUsage),
    ];

    public IEnumerable<MedKitSettings> MedKits() =>
    [
        new("Grizzly", GrizzlyId, GrizzlyChanges, GrizzlyHP,
            new(GrizzlyCanHealHeavyBleeding, GrizzlyHeavyBleedingHealCost),
            new(GrizzlyCanHealLightBleeding, GrizzlyLightBleedingHealCost),
            new(GrizzlyCanHealFractures, GrizzlyFractureHealCost),
            new(GrizzlyCanDoSurgery, GrizzlySurgeryCost)),
        new("AI-2", Ai2Id, Ai2Changes, Ai2HP,
            new(Ai2CanHealHeavyBleeding, Ai2HeavyBleedingHealCost),
            new(Ai2CanHealLightBleeding, Ai2LightBleedingHealCost),
            new(Ai2CanHealFractures, Ai2FractureHealCost),
            new(Ai2CanDoSurgery, Ai2SurgeryCost)),
        new("Car kit", CarKitId, CarKitChanges, CarKitHP,
            new(CarKitCanHealHeavyBleeding, CarKitHeavyBleedingHealCost),
            new(CarKitCanHealLightBleeding, CarKitLightBleedingHealCost),
            new(CarKitCanHealFractures, CarKitFractureHealCost),
            new(CarKitCanDoSurgery, CarKitSurgeryCost)),
        new("Salewa", SalewaId, SalewaChanges, SalewaHP,
            new(SalewaCanHealHeavyBleeding, SalewaHeavyBleedingHealCost),
            new(SalewaCanHealLightBleeding, SalewaLightBleedingHealCost),
            new(SalewaCanHealFractures, SalewaFractureHealCost),
            new(SalewaCanDoSurgery, SalewaSurgeryCost)),
        new("IFAK", IfakId, IfakChanges, IfakHP,
            new(IfakCanHealHeavyBleeding, IfakHeavyBleedingHealCost),
            new(IfakCanHealLightBleeding, IfakLightBleedingHealCost),
            new(IfakCanHealFractures, IfakFractureHealCost),
            new(IfakCanDoSurgery, IfakSurgeryCost)),
        new("AFAK", AfakId, AfakChanges, AfakHP,
            new(AfakCanHealHeavyBleeding, AfakHeavyBleedingHealCost),
            new(AfakCanHealLightBleeding, AfakLightBleedingHealCost),
            new(AfakCanHealFractures, AfakFractureHealCost),
            new(AfakCanDoSurgery, AfakSurgeryCost)),
    ];
}

public record HealEffectSettings(bool Enabled, int Cost);

public record MedKitSettings(
    string Name,
    string Id,
    bool Enabled,
    int Hp,
    HealEffectSettings HeavyBleeding,
    HealEffectSettings LightBleeding,
    HealEffectSettings Fracture,
    HealEffectSettings Surgery);
