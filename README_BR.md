<div align="center">

# Make Meds Great Again!

Cura melhor no SPT 4.1.6: andar durante a cirurgia, correr sob efeito de analgésico, kits médicos mais fortes e um analgésico novo.

![Version](https://img.shields.io/badge/version-1.3.0-orange?style=flat)
![SPT](https://img.shields.io/badge/SPT-4.1.6-blue?style=flat)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green?style=flat)

[Funcionalidades](#funcionalidades) · [Instalação](#instalação) · [Paracetamol](#paracetamol) · [Configuração](#configuração) · [Build](#build-a-partir-do-código)

[English](README.md) · **Português**

</div>

---

## Funcionalidades

- **Andar durante a cirurgia**: dá para se mover devagar usando o CMS ou o Surv12.
- **Correr usando medicamentos**: analgésicos e outros medicamentos não impedem mais a corrida.
- **Kits médicos mais fortes**: Grizzly, AI-2, Car kit, Salewa, IFAK e AFAK ganham mais HP e tratam sangramento forte e leve, fraturas e membros destruídos (cirurgia).
- **Mais usos** para Caloc-B, Army bandage, Analgin, Augmentin, Ibuprofen, Vaseline, Golden Star, Aluminum splint, Surv12 e CMS.
- **Medicamento novo**: [Paracetamol](#paracetamol), vendido pelo Therapist e encontrado nas raids.

Tudo do lado do server se configura no `config.json`, e as opções do client ficam no menu `F12`.

---

## Instalação

Extraia o `Make-Meds-Great-Again.zip` na pasta do jogo SPT:

```
<pasta do jogo>/
├── BepInEx/plugins/Make-Meds-Great-Again.dll
└── SPT_Runtime/user/mods/Make-Meds-Great-Again/
    ├── Make-Meds-Great-Again.dll
    └── config.json
```

A parte do server altera os itens; a do client cuida do movimento durante a cura. Cada parte funciona sem a outra.

> Atualizando da versão do SPT 4.0? Apague a pasta antiga `SPT/user/mods/MakeMedsGreatAgainServer/` e o `BepInEx/plugins/makeMedsGreatAgain.dll`. Suas opções do `F12` são mantidas.

---

## Paracetamol

| | |
|---|---|
| Efeito | Tira a dor por 60 segundos (15 segundos de fade-out) |
| Usos | 4, igual ao Analgin |
| Vendido por | **Therapist**, nível de lealdade 1, 5.532 ₽ |
| Onde achar | Em todo container que tem Analgin, com metade da chance dele. Os bots carregam onde carregariam Analgin |
| Preço no flea | 6.005 ₽ |

---

## Configuração

### Client (menu `F12`)

| Opção | Padrão | Descrição |
|---|---|---|
| Can walk in surgery | `true` | Se mover usando o CMS ou o Surv12 |
| Can sprint using meds | `true` | Correr usando analgésicos e outros medicamentos |

### Server (`config.json`)

O arquivo fica em `SPT_Runtime/user/mods/Make-Meds-Great-Again/`. Reinicie o server depois de editar.

**Geral**

| Campo | Padrão | Descrição |
|---|---|---|
| `debug` | `false` | Mostra cada alteração no console do server |
| `paracetamolTraderPrice` | `5532` | Preço do Paracetamol no Therapist, em rublos |
| `paracetamolLootMultiplier` | `0.5` | Chance do Paracetamol no loot em relação ao Analgin. `0` tira ele dos containers |

**Usos** (`<item>Usage`). `0` mantém o valor do jogo.

| Item | Campo | Jogo | Mod |
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

**Kits médicos**. Cada kit tem seu próprio grupo de campos, com o nome do kit como prefixo (`grizzly`, `ai2`, `carKit`, `salewa`, `ifak`, `afak`):

| Campo | Descrição |
|---|---|
| `<kit>Changes` | `false` deixa o kit como no jogo |
| `<kit>HP` | HP total do kit |
| `<kit>CanHealHeavyBleeding` / `<kit>HeavyBleedingHealCost` | Trata sangramento forte, e quanto HP isso gasta |
| `<kit>CanHealLightBleeding` / `<kit>LightBleedingHealCost` | Trata sangramento leve, e quanto HP isso gasta |
| `<kit>CanHealFractures` / `<kit>FractureHealCost` | Trata fraturas, e quanto HP isso gasta |
| `<kit>CanDoSurgery` / `<kit>SurgeryCost` | Recupera membro destruído, e quanto HP isso gasta |

Valores padrão:

| Kit | HP (jogo → mod) | Sangramento forte | Sangramento leve | Fratura | Cirurgia |
|---|---|---|---|---|---|
| Grizzly | 1800 → 2250 | 200 | 100 | 200 | 550 |
| AI-2 | 100 → 200 | 100 | 30 | 100 | 200 |
| Car kit | 220 → 250 | 100 | 50 | 100 | 200 |
| Salewa | 400 → 450 | 175 | 40 | 100 | 400 |
| IFAK | 300 → 400 | 175 | 30 | 180 | 350 |
| AFAK | 400 → 420 | 175 | 30 | 180 | 370 |

Os kits alterados também tratam concussão e exposição à radiação.

---

## Build a partir do código

**Requisitos:** .NET 10 SDK e uma instalação do SPT 4.1.6 (o projeto do client referencia as DLLs do jogo).

```sh
dotnet build Make-Meds-Great-Again.sln -c Release
```

O projeto do server compila o client antes e gera o `Make-Meds-Great-Again.zip` na pasta da solution, com as duas DLLs e o `config.json`.

> O `.csproj` do client aponta para `D:\Jogos\SPT4.1` nas referências, e os dois projetos copiam o build para essa instalação para teste. Troque os `HintPath`s e o `SptModsDir` pela sua pasta do SPT. Feche o server do SPT antes de compilar, senão a cópia falha porque a DLL do server está em uso.

### Estrutura do projeto

```
Make-Meds-Great-Again/
├── Make-Meds-Great-Again.sln
├── Server/                         server mod .NET 10
│   ├── Mod.cs                      metadata do mod
│   ├── ModConfig.cs                modelo e leitura do config.json
│   ├── MedicalChanges.cs           usos, HP e tratamentos dos medicamentos do jogo
│   ├── ParacetamolLoader.cs        cria o Paracetamol e coloca no Therapist, no loot e nos bots
│   └── config.json
└── Client/                         plugin BepInEx (netstandard2.1)
    ├── Plugin.cs                   opções do F12
    └── Patches/
        └── PhysicalConditionPatch.cs   movimento durante a cura
```

---

## Créditos

- Jehree pelo [Quick Start Guide](https://github.com/Jehree/SPTClientModExamples) de mods de client
- Kobrakon pelos [exemplos](https://github.com/kobrakon/ClientModdingExamples)
- Lacyway e Cj pela ajuda com o código
- NoNeedName

---

## Recursos

| Recurso | URL |
|---|---|
| SPT Server C# | https://github.com/SP-Tushonka/server-csharp |
| Exemplos de server mod | https://github.com/SP-Tushonka/server-mod-examples |
| SPT Wiki — Modding Resources | https://wiki.sp-tushonka.com/en/modding/Modding_Resources |
| SPT Scaffold | https://github.com/viniHNS/spt-scaffold |
