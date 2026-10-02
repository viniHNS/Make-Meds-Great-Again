<div align="center">

# Make Meds Great Again!

Better healing for SPT 4.1.6: walk during surgery, sprint on painkillers, stronger medical kits and a new painkiller.

![Version](https://img.shields.io/badge/version-1.3.0-orange?style=flat)
![SPT](https://img.shields.io/badge/SPT-4.1.6-blue?style=flat)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green?style=flat)

[Features](#features) · [Install](#install) · [Paracetamol](#paracetamol) · [Configuration](#configuration) · [Build](#build-from-source)

**English** · [Português](README_BR.md)

</div>

---

## Features

- **Walk during surgery**: move slowly while using the CMS or Surv12 kits.
- **Sprint on meds**: painkillers and other meds no longer block sprinting.
- **Stronger medical kits**: Grizzly, AI-2, Car kit, Salewa, IFAK and AFAK get more HP and can treat heavy and light bleeding, fractures and destroyed limbs (surgery).
- **More uses** for Caloc-B, Army bandage, Analgin, Augmentin, Ibuprofen, Vaseline, Golden Star, Aluminum splint, Surv12 and CMS.
- **New medication**: [Paracetamol](#paracetamol), sold by Therapist and found in raids.

Every server change is set in `config.json`, and the client options can be switched in the `F12` menu.

---

## Install

Extract `Make-Meds-Great-Again.zip` into your SPT game folder:

```
<game folder>/
├── BepInEx/plugins/Make-Meds-Great-Again.dll
└── SPT_Runtime/user/mods/Make-Meds-Great-Again/
    ├── Make-Meds-Great-Again.dll
    └── config.json
```

The server part changes the items; the client part handles movement while healing. Each part works without the other.

> Upgrading from the SPT 4.0 version? Delete the old `SPT/user/mods/MakeMedsGreatAgainServer/` folder and `BepInEx/plugins/makeMedsGreatAgain.dll`. Your `F12` settings are kept.

---

## Paracetamol

| | |
|---|---|
| Effect | Relieves pain for 60 seconds (15 second fade-out) |
| Uses | 4, same as Analgin |
| Sold by | **Therapist**, loyalty level 1, 5,532 ₽ |
| Found in | Every container that holds Analgin, at half its chance. Bots carry it wherever they carry Analgin |
| Flea price | 6,005 ₽ |

---

## Configuration

### Client (`F12` menu)

| Setting | Default | Description |
|---|---|---|
| Can walk in surgery | `true` | Move while using the CMS or Surv12 |
| Can sprint using meds | `true` | Sprint while using painkillers and other meds |

### Server (`config.json`)

The file is in `SPT_Runtime/user/mods/Make-Meds-Great-Again/`. Restart the server after editing it.

**General**

| Field | Default | Description |
|---|---|---|
| `debug` | `false` | Log every change to the server console |
| `paracetamolTraderPrice` | `5532` | Paracetamol price at Therapist, in roubles |
| `paracetamolLootMultiplier` | `0.5` | Paracetamol's loot chance relative to Analgin. `0` keeps it out of containers |

**Uses** (`<item>Usage`). `0` keeps the game's value.

| Item | Field | Game | Mod |
|---|---|---|---|
| Caloc-B | `calocUsage` | 3 | 5 |
| Army bandage | `armyBandageUsage` | 2 | 4 |
| Analgin | `analginPainkillersUsage` | 4 | 10 |
| Augmentin | `augmentinUsage` | 1 | 15 |
| Ibuprofen | `ibuprofenUsage` | 8 | 20 |
| Vaseline | `vaselinUsage` | 3 | 10 |
| Golden Star | `goldenStarUsage` | 5 | 15 |
| Aluminum splint | `aluminiumSplintUsage` | 5 | 10 |
| Surv12 | `survivalKitUsage` | 9 | 15 |
| CMS | `cmsUsage` | 3 | 10 |

**Medical kits**. Each kit has its own block of fields, with the kit name as prefix (`grizzly`, `ai2`, `carKit`, `salewa`, `ifak`, `afak`):

| Field | Description |
|---|---|
| `<kit>Changes` | `false` leaves the kit untouched |
| `<kit>HP` | Total HP of the kit |
| `<kit>CanHealHeavyBleeding` / `<kit>HeavyBleedingHealCost` | Treat heavy bleeding, and the HP it costs |
| `<kit>CanHealLightBleeding` / `<kit>LightBleedingHealCost` | Treat light bleeding, and the HP it costs |
| `<kit>CanHealFractures` / `<kit>FractureHealCost` | Treat fractures, and the HP it costs |
| `<kit>CanDoSurgery` / `<kit>SurgeryCost` | Restore a destroyed limb, and the HP it costs |

Default values:

| Kit | HP (game → mod) | Heavy bleeding | Light bleeding | Fracture | Surgery |
|---|---|---|---|---|---|
| Grizzly | 1800 → 2250 | 200 | 100 | 200 | 550 |
| AI-2 | 100 → 200 | 100 | 30 | 100 | 200 |
| Car kit | 220 → 250 | 100 | 50 | 100 | 200 |
| Salewa | 400 → 450 | 175 | 40 | 100 | 400 |
| IFAK | 300 → 400 | 175 | 30 | 180 | 350 |
| AFAK | 400 → 420 | 175 | 30 | 180 | 370 |

Kits with changes also treat contusions and radiation exposure.

---

## Build from Source

**Requirements:** .NET 10 SDK and an SPT 4.1.6 install (the client project references the game's DLLs).

```sh
dotnet build Make-Meds-Great-Again.sln -c Release
```

The server project builds the client first and creates `Make-Meds-Great-Again.zip` in the solution folder with both DLLs and `config.json`.

> The client `.csproj` points at `D:\Jogos\SPT4.1` for its references, and both projects copy their build output into that install for testing. Change the `HintPath`s and `SptModsDir` to your own SPT folder. Close the SPT server before building, or the copy fails because the server DLL is in use.

### Project Structure

```
Make-Meds-Great-Again/
├── Make-Meds-Great-Again.sln
├── Server/                         .NET 10 server mod
│   ├── Mod.cs                      mod metadata
│   ├── ModConfig.cs                config.json model and loader
│   ├── MedicalChanges.cs           uses, HP and treatments of the game's meds
│   ├── ParacetamolLoader.cs        creates Paracetamol and adds it to Therapist, loot and bots
│   └── config.json
└── Client/                         BepInEx plugin (netstandard2.1)
    ├── Plugin.cs                   F12 settings
    └── Patches/
        └── PhysicalConditionPatch.cs   movement while healing
```

---

## Credits

- Jehree for the client modding [Quick Start Guide](https://github.com/Jehree/SPTClientModExamples)
- Kobrakon for his [examples](https://github.com/kobrakon/ClientModdingExamples)
- Lacyway and Cj for help with the code
- NoNeedName

---

## Resources

| Resource | URL |
|---|---|
| SPT Server C# | https://github.com/SP-Tushonka/server-csharp |
| Server Mod Examples | https://github.com/SP-Tushonka/server-mod-examples |
| SPT Wiki — Modding Resources | https://wiki.sp-tushonka.com/en/modding/Modding_Resources |
| SPT Scaffold | https://github.com/viniHNS/spt-scaffold |
