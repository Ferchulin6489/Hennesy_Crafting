// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesContainer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.PoEMemory.FilesInMemory.Ancestor;
using ExileCore.PoEMemory.FilesInMemory.Archnemesis;
using ExileCore.PoEMemory.FilesInMemory.Atlas;
using ExileCore.PoEMemory.FilesInMemory.Harvest;
using ExileCore.PoEMemory.FilesInMemory.Metamorph;
using ExileCore.PoEMemory.FilesInMemory.Sanctum;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExileCore.PoEMemory
{
  public class FilesContainer
  {
    private UniversalFileWrapper<AncestralTrialUnit> _ancestralTrialUnits;
    private UniversalFileWrapper<AncestralTrialItem> _ancestralTrialItems;
    private readonly IMemory _memory;
    private BaseItemTypes _baseItemTypes;
    private UniversalFileWrapper<BetrayalChoiceAction> _betrayalChoiceActions;
    private UniversalFileWrapper<BetrayalChoice> _betrayalChoises;
    private UniversalFileWrapper<ExileCore.PoEMemory.MemoryObjects.BetrayalDialogue> _betrayalDialogue;
    private UniversalFileWrapper<ArchnemesisRecipe> _archnemesisRecipes;
    private UniversalFileWrapper<BetrayalJob> _betrayalJobs;
    private UniversalFileWrapper<BetrayalRank> _betrayalRanks;
    private UniversalFileWrapper<BetrayalReward> _betrayalRewards;
    private UniversalFileWrapper<BetrayalTarget> _betrayalTargets;
    private UniversalFileWrapper<HeistJobRecord> _HeistJobs;
    private UniversalFileWrapper<HeistChestRewardTypeRecord> _HeistChestRewardTypes;
    private UniversalFileWrapper<HeistNpcRecord> _HeistNpcs;
    private UniversalFileWrapper<ClientString> _clientString;
    private StatDescriptionWrapper<StatDescription> _statDescriptions;
    private ModsDat _mods;
    private StatsDat _stats;
    private TagsDat _tags;
    private ItemVisualIdentities _itemVisualIdentities;
    private UniqueItemDescriptions _uniqueItemDescriptions;
    private UniversalFileWrapper<WordEntry> _word;
    private UniversalFileWrapper<AtlasNode> atlasNodes;
    public FilesFromMemory FilesFromMemory;
    private LabyrinthTrials labyrinthTrials;
    private MonsterVarieties monsterVarieties;
    private PassiveSkills passiveSkills;
    private PropheciesDat prophecies;
    private Quests quests;
    private QuestStates questStates;
    private UniversalFileWrapper<ArchnemesisMod> _archnemesisMods;
    private UniversalFileWrapper<LakeRoom> _lakeRooms;
    private UniversalFileWrapper<StampChoice> _stampChoices;
    private UniversalFileWrapper<BlightTowerDat> _blightTowers;
    private UniversalFileWrapper<HarvestSeed> _harvestSeeds;
    private UniversalFileWrapper<HeistChestRecord> _heistChests;
    private UniversalFileWrapper<ChestRecord> _chests;
    private UniversalFileWrapper<QuestReward> _questRewards;
    private UniversalFileWrapper<QuestRewardOffer> _questRewardOffers;
    private UniversalFileWrapper<Character> _characters;
    private UniversalFileWrapper<GrantedEffectPerLevel> _grantedEffectsPerLevel;
    private UniversalFileWrapper<GrantedEffect> _grantedEffects;
    private UniversalFileWrapper<BuffVisual> _buffVisuals;
    private UniversalFileWrapper<BuffDefinition> _buffDefinitions;
    private WorldAreas worldAreas;
    private UniversalFileWrapper<MetamorphMetaSkill> _metamorphMetaSkills;
    private UniversalFileWrapper<MetamorphMetaSkillType> _metamorphMetaSkillTypes;
    private UniversalFileWrapper<MetamorphMetaMonster> _metamorphMetaMonsters;
    private UniversalFileWrapper<MetamorphRewardType> _metamorphRewardTypes;
    private UniversalFileWrapper<ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphRewardTypeItemsClient> _metamorphRewardTypeItemsClient;
    private AtlasRegions _atlasRegions;
    private BestiaryCapturableMonsters _bestiaryCapturableMonsters;
    private UniversalFileWrapper<BestiaryRecipe> _bestiaryRecipes;
    private UniversalFileWrapper<BestiaryRecipeComponent> _bestiaryRecipeComponents;
    private UniversalFileWrapper<BestiaryGroup> _bestiaryGroups;
    private UniversalFileWrapper<BestiaryFamily> _bestiaryFamilies;
    private UniversalFileWrapper<BestiaryGenus> _bestiaryGenuses;
    private UniversalFileWrapper<SanctumRoom> _sanctumRooms;
    private UniversalFileWrapper<SanctumRoomType> _sanctumRoomTypes;
    private UniversalFileWrapper<SanctumPersistentEffect> _sanctumPersistentEffects;
    private UniversalFileWrapper<SanctumDeferredRewardCategory> _sanctumDeferredRewardCategories;

    public UniversalFileWrapper<AncestralTrialUnit> AncestralTrialUnits
    {
      get
      {
        UniversalFileWrapper<AncestralTrialUnit> ancestralTrialUnits1 = this._ancestralTrialUnits;
        if (ancestralTrialUnits1 != null)
          return ancestralTrialUnits1;
        UniversalFileWrapper<AncestralTrialUnit> universalFileWrapper = new UniversalFileWrapper<AncestralTrialUnit>(this._memory, (Func<long>) (() => this.FindFile("Data/AncestralTrialUnits.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<AncestralTrialUnit> ancestralTrialUnits2 = universalFileWrapper;
        this._ancestralTrialUnits = universalFileWrapper;
        return ancestralTrialUnits2;
      }
    }

    public UniversalFileWrapper<AncestralTrialItem> AncestralTrialItems
    {
      get
      {
        UniversalFileWrapper<AncestralTrialItem> ancestralTrialItems1 = this._ancestralTrialItems;
        if (ancestralTrialItems1 != null)
          return ancestralTrialItems1;
        UniversalFileWrapper<AncestralTrialItem> universalFileWrapper = new UniversalFileWrapper<AncestralTrialItem>(this._memory, (Func<long>) (() => this.FindFile("Data/AncestralTrialItems.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<AncestralTrialItem> ancestralTrialItems2 = universalFileWrapper;
        this._ancestralTrialItems = universalFileWrapper;
        return ancestralTrialItems2;
      }
    }

    public FilesContainer(IMemory memory)
    {
      this._memory = memory;
      this.ItemClasses = new ItemClasses();
      this.FilesFromMemory = new FilesFromMemory(this._memory);
      using (new PerformanceTimer("Load files from memory"))
      {
        this.AllFiles = this.FilesFromMemory.GetAllFiles();
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
        interpolatedStringHandler.AppendLiteral("Loaded ");
        interpolatedStringHandler.AppendFormatted<int>(this.AllFiles.Count);
        interpolatedStringHandler.AppendLiteral(" files from memory ");
        interpolatedStringHandler.AppendFormatted<int>(this.AllFiles.Values.Count<FileInformation>((Func<FileInformation, bool>) (x => x.Ptr > 0L)));
        interpolatedStringHandler.AppendLiteral("/");
        interpolatedStringHandler.AppendFormatted<int>(this.AllFiles.Count);
        interpolatedStringHandler.AppendLiteral(" has pointers.");
        Trace.WriteLine(interpolatedStringHandler.ToStringAndClear());
      }
      Task.Run((Action) (() =>
      {
        using (new PerformanceTimer("Preload stats and mods"))
        {
          int count1 = this.Stats.records.Count;
          int count2 = this.Mods.records.Count;
          this.ParseFiles(this.AllFiles);
        }
      }));
    }

    public ItemClasses ItemClasses { get; }

    public BaseItemTypes BaseItemTypes => this._baseItemTypes ?? (this._baseItemTypes = new BaseItemTypes(this._memory, (Func<long>) (() => this.FindFile("Data/BaseItemTypes.dat"))));

    public UniversalFileWrapper<ClientString> ClientStrings => this._clientString ?? (this._clientString = new UniversalFileWrapper<ClientString>(this._memory, (Func<long>) (() => this.FindFile("Data/ClientStrings.dat"))));

    public StatDescriptionWrapper<StatDescription> StatDescriptions => this._statDescriptions ?? (this._statDescriptions = new StatDescriptionWrapper<StatDescription>(this._memory, (Func<long>) (() => this.FindFile("Metadata/StatDescriptions/stat_descriptions.txt"))));

    public ModsDat Mods => this._mods ?? (this._mods = new ModsDat(this._memory, (Func<long>) (() => this.FindFile("Data/Mods.dat")), this.Stats, this.Tags));

    public StatsDat Stats => this._stats ?? (this._stats = new StatsDat(this._memory, (Func<long>) (() => this.FindFile("Data/Stats.dat"))));

    public TagsDat Tags => this._tags ?? (this._tags = new TagsDat(this._memory, (Func<long>) (() => this.FindFile("Data/Tags.dat"))));

    public WorldAreas WorldAreas => this.worldAreas ?? (this.worldAreas = new WorldAreas(this._memory, (Func<long>) (() => this.FindFile("Data/WorldAreas.dat"))));

    public PassiveSkills PassiveSkills => this.passiveSkills ?? (this.passiveSkills = new PassiveSkills(this._memory, (Func<long>) (() => this.FindFile("Data/PassiveSkills.dat"))));

    public LabyrinthTrials LabyrinthTrials => this.labyrinthTrials ?? (this.labyrinthTrials = new LabyrinthTrials(this._memory, (Func<long>) (() => this.FindFile("Data/LabyrinthTrials.dat"))));

    public Quests Quests => this.quests ?? (this.quests = new Quests(this._memory, (Func<long>) (() => this.FindFile("Data/Quest.dat"))));

    public QuestStates QuestStates => this.questStates ?? (this.questStates = new QuestStates(this._memory, (Func<long>) (() => this.FindFile("Data/QuestStates.dat"))));

    public UniversalFileWrapper<QuestReward> QuestRewards => this._questRewards ?? (this._questRewards = new UniversalFileWrapper<QuestReward>(this._memory, (Func<long>) (() => this.FindFile("Data/QuestRewards.dat"))));

    public UniversalFileWrapper<QuestRewardOffer> QuestRewardOffers => this._questRewardOffers ?? (this._questRewardOffers = new UniversalFileWrapper<QuestRewardOffer>(this._memory, (Func<long>) (() => this.FindFile("Data/QuestRewardOffers.dat"))));

    public UniversalFileWrapper<Character> Characters => this._characters ?? (this._characters = new UniversalFileWrapper<Character>(this._memory, (Func<long>) (() => this.FindFile("Data/Characters.dat"))));

    public MonsterVarieties MonsterVarieties => this.monsterVarieties ?? (this.monsterVarieties = new MonsterVarieties(this._memory, (Func<long>) (() => this.FindFile("Data/MonsterVarieties.dat"))));

    public PropheciesDat Prophecies => this.prophecies ?? (this.prophecies = new PropheciesDat(this._memory, (Func<long>) (() => this.FindFile("Data/Prophecies.dat"))));

    public ItemVisualIdentities ItemVisualIdentities => this._itemVisualIdentities ?? (this._itemVisualIdentities = new ItemVisualIdentities(this._memory, (Func<long>) (() => this.FindFile("Data/ItemVisualIdentity.dat"))));

    public UniqueItemDescriptions UniqueItemDescriptions => this._uniqueItemDescriptions ?? (this._uniqueItemDescriptions = new UniqueItemDescriptions(this._memory, (Func<long>) (() => this.FindFile("Data/UniqueStashLayout.dat"))));

    public UniversalFileWrapper<WordEntry> Words => this._word ?? (this._word = new UniversalFileWrapper<WordEntry>(this._memory, (Func<long>) (() => this.FindFile("Data/Words.dat"))));

    public UniversalFileWrapper<AtlasNode> AtlasNodes => this.atlasNodes ?? (this.atlasNodes = (UniversalFileWrapper<AtlasNode>) new ExileCore.PoEMemory.FilesInMemory.Atlas.AtlasNodes(this._memory, (Func<long>) (() => this.FindFile("Data/AtlasNode.dat"))));

    public UniversalFileWrapper<BetrayalTarget> BetrayalTargets => this._betrayalTargets ?? (this._betrayalTargets = new UniversalFileWrapper<BetrayalTarget>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalTargets.dat"))));

    public UniversalFileWrapper<BetrayalJob> BetrayalJobs => this._betrayalJobs ?? (this._betrayalJobs = new UniversalFileWrapper<BetrayalJob>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalJobs.dat"))));

    public UniversalFileWrapper<BetrayalRank> BetrayalRanks => this._betrayalRanks ?? (this._betrayalRanks = new UniversalFileWrapper<BetrayalRank>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalRanks.dat"))));

    public UniversalFileWrapper<BetrayalReward> BetrayalRewards => this._betrayalRewards ?? (this._betrayalRewards = new UniversalFileWrapper<BetrayalReward>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalTraitorRewards.dat"))));

    public UniversalFileWrapper<BetrayalChoice> BetrayalChoises => this._betrayalChoises ?? (this._betrayalChoises = new UniversalFileWrapper<BetrayalChoice>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalChoices.dat"))));

    public UniversalFileWrapper<BetrayalChoiceAction> BetrayalChoiceActions => this._betrayalChoiceActions ?? (this._betrayalChoiceActions = new UniversalFileWrapper<BetrayalChoiceAction>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalChoiceActions.dat"))));

    public UniversalFileWrapper<ExileCore.PoEMemory.MemoryObjects.BetrayalDialogue> BetrayalDialogue => this._betrayalDialogue ?? (this._betrayalDialogue = new UniversalFileWrapper<ExileCore.PoEMemory.MemoryObjects.BetrayalDialogue>(this._memory, (Func<long>) (() => this.FindFile("Data/BetrayalDialogue.dat"))));

    public UniversalFileWrapper<ArchnemesisRecipe> ArchnemesisRecipes => this._archnemesisRecipes ?? (this._archnemesisRecipes = new UniversalFileWrapper<ArchnemesisRecipe>(this._memory, (Func<long>) (() => this.FindFile("Data/ArchnemesisRecipes.dat"))));

    public UniversalFileWrapper<ArchnemesisMod> ArchnemesisMods => this._archnemesisMods ?? (this._archnemesisMods = new UniversalFileWrapper<ArchnemesisMod>(this._memory, (Func<long>) (() => this.FindFile("Data/ArchnemesisMods.dat"))));

    public UniversalFileWrapper<LakeRoom> LakeRooms => this._lakeRooms ?? (this._lakeRooms = new UniversalFileWrapper<LakeRoom>(this._memory, (Func<long>) (() => this.FindFile("Data/LakeRooms.dat"))));

    public UniversalFileWrapper<StampChoice> StampChoices => this._stampChoices ?? (this._stampChoices = new UniversalFileWrapper<StampChoice>(this._memory, (Func<long>) (() => this.FindFile("Data/StampChoice.dat"))));

    public UniversalFileWrapper<BlightTowerDat> BlightTowers => this._blightTowers ?? (this._blightTowers = new UniversalFileWrapper<BlightTowerDat>(this._memory, (Func<long>) (() => this.FindFile("Data/BlightTowers.dat"))));

    public UniversalFileWrapper<HarvestSeed> HarvestSeeds => this._harvestSeeds ?? (this._harvestSeeds = new UniversalFileWrapper<HarvestSeed>(this._memory, (Func<long>) (() => this.FindFile("Data/HarvestSeeds.dat"))));

    public UniversalFileWrapper<GrantedEffectPerLevel> GrantedEffectsPerLevel => this._grantedEffectsPerLevel ?? (this._grantedEffectsPerLevel = new UniversalFileWrapper<GrantedEffectPerLevel>(this._memory, (Func<long>) (() => this.FindFile("Data/GrantedEffectsPerLevel.dat"))));

    public UniversalFileWrapper<GrantedEffect> GrantedEffects => this._grantedEffects ?? (this._grantedEffects = new UniversalFileWrapper<GrantedEffect>(this._memory, (Func<long>) (() => this.FindFile("Data/GrantedEffects.dat"))));

    public UniversalFileWrapper<BuffDefinition> BuffDefinitions => this._buffDefinitions ?? (this._buffDefinitions = new UniversalFileWrapper<BuffDefinition>(this._memory, (Func<long>) (() => this.FindFile("Data/BuffDefinitions.dat"))));

    public UniversalFileWrapper<BuffVisual> BuffVisuals => this._buffVisuals ?? (this._buffVisuals = new UniversalFileWrapper<BuffVisual>(this._memory, (Func<long>) (() => this.FindFile("Data/BuffVisuals.dat"))));

    public UniversalFileWrapper<HeistChestRecord> HeistChests => this._heistChests ?? (this._heistChests = new UniversalFileWrapper<HeistChestRecord>(this._memory, (Func<long>) (() => this.FindFile("Data/HeistChests.dat"))));

    public UniversalFileWrapper<ChestRecord> Chests => this._chests ?? (this._chests = new UniversalFileWrapper<ChestRecord>(this._memory, (Func<long>) (() => this.FindFile("Data/Chests.dat"))));

    public UniversalFileWrapper<HeistJobRecord> HeistJobs => this._HeistJobs ?? (this._HeistJobs = new UniversalFileWrapper<HeistJobRecord>(this._memory, (Func<long>) (() => this.FindFile("Data/HeistJobs.dat"))));

    public UniversalFileWrapper<HeistChestRewardTypeRecord> HeistChestRewardType => this._HeistChestRewardTypes ?? (this._HeistChestRewardTypes = new UniversalFileWrapper<HeistChestRewardTypeRecord>(this._memory, (Func<long>) (() => this.FindFile("Data/HeistChestRewardTypes.dat"))));

    public UniversalFileWrapper<HeistNpcRecord> HeistNpcs => this._HeistNpcs ?? (this._HeistNpcs = new UniversalFileWrapper<HeistNpcRecord>(this._memory, (Func<long>) (() => this.FindFile("Data/HeistNPCs.dat"))));

    public UniversalFileWrapper<MetamorphMetaSkill> MetamorphMetaSkills => this._metamorphMetaSkills ?? (this._metamorphMetaSkills = new UniversalFileWrapper<MetamorphMetaSkill>(this._memory, (Func<long>) (() => this.FindFile("Data/MetamorphosisMetaSkills.dat"))));

    public UniversalFileWrapper<MetamorphMetaSkillType> MetamorphMetaSkillTypes => this._metamorphMetaSkillTypes ?? (this._metamorphMetaSkillTypes = new UniversalFileWrapper<MetamorphMetaSkillType>(this._memory, (Func<long>) (() => this.FindFile("Data/MetamorphosisMetaSkillTypes.dat"))));

    public UniversalFileWrapper<MetamorphMetaMonster> MetamorphMetaMonsters => this._metamorphMetaMonsters ?? (this._metamorphMetaMonsters = new UniversalFileWrapper<MetamorphMetaMonster>(this._memory, (Func<long>) (() => this.FindFile("Data/MetamorphosisMetaMonsters.dat"))));

    public UniversalFileWrapper<MetamorphRewardType> MetamorphRewardTypes => this._metamorphRewardTypes ?? (this._metamorphRewardTypes = new UniversalFileWrapper<MetamorphRewardType>(this._memory, (Func<long>) (() => this.FindFile("Data/MetamorphosisRewardTypes.dat"))));

    public UniversalFileWrapper<ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphRewardTypeItemsClient> MetamorphRewardTypeItemsClient => this._metamorphRewardTypeItemsClient ?? (this._metamorphRewardTypeItemsClient = new UniversalFileWrapper<ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphRewardTypeItemsClient>(this._memory, (Func<long>) (() => this.FindFile("Data/MetamorphosisRewardTypeItemsClient.dat"))));

    public AtlasRegions AtlasRegions => this._atlasRegions ?? (this._atlasRegions = new AtlasRegions(this._memory, (Func<long>) (() => this.FindFile("Data/AtlasRegions.dat"))));

    public Dictionary<string, FileInformation> AllFiles { get; private set; }

    public Dictionary<string, FileInformation> Metadata { get; } = new Dictionary<string, FileInformation>();

    public Dictionary<string, FileInformation> Data { get; private set; } = new Dictionary<string, FileInformation>();

    public Dictionary<string, FileInformation> OtherFiles { get; } = new Dictionary<string, FileInformation>();

    public Dictionary<string, FileInformation> LoadedInThisArea { get; private set; } = new Dictionary<string, FileInformation>(1024);

    public Dictionary<int, List<KeyValuePair<string, FileInformation>>> GroupedByTest2 { get; set; }

    public Dictionary<int, List<KeyValuePair<string, FileInformation>>> GroupedByChangeAction { get; set; }

    public void LoadFiles() => this.AllFiles = this.FilesFromMemory.GetAllFilesSync();

    public event EventHandler<Dictionary<string, FileInformation>> LoadedFiles;

    public void ParseFiles(Dictionary<string, FileInformation> files)
    {
      foreach (KeyValuePair<string, FileInformation> file in files)
      {
        if (!string.IsNullOrEmpty(file.Key))
        {
          if (file.Key.StartsWith("Metadata/", StringComparison.Ordinal))
            this.Metadata[file.Key] = file.Value;
          else if (file.Key.StartsWith("Data/", StringComparison.Ordinal) && file.Key.EndsWith(".dat", StringComparison.Ordinal))
            this.Data[file.Key] = file.Value;
          else
            this.OtherFiles[file.Key] = file.Value;
        }
      }
    }

    public void ParseFiles(int gameAreaChangeCount)
    {
      if (this.AllFiles == null)
        return;
      this.LoadedInThisArea = new Dictionary<string, FileInformation>(1024);
      this.ParseFiles(this.AllFiles);
      EventHandler<Dictionary<string, FileInformation>> loadedFiles = this.LoadedFiles;
      if (loadedFiles == null)
        return;
      loadedFiles((object) this, this.LoadedInThisArea);
    }

    public long FindFile(string name)
    {
      try
      {
        FileInformation fileInformation;
        if (this.AllFiles.TryGetValue(name, out fileInformation))
          return fileInformation.Ptr;
      }
      catch (KeyNotFoundException ex)
      {
        int num = (int) MessageBox.Show(string.Format("Couldn't find the file in memory: {0}\nTry to restart the game.", (object) name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Environment.Exit(1);
      }
      return 0;
    }

    public BestiaryCapturableMonsters BestiaryCapturableMonsters => this._bestiaryCapturableMonsters ?? (this._bestiaryCapturableMonsters = new BestiaryCapturableMonsters(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryCapturableMonsters.dat"))));

    public UniversalFileWrapper<BestiaryRecipe> BestiaryRecipes => this._bestiaryRecipes ?? (this._bestiaryRecipes = new UniversalFileWrapper<BestiaryRecipe>(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryRecipes.dat"))));

    public UniversalFileWrapper<BestiaryRecipeComponent> BestiaryRecipeComponents => this._bestiaryRecipeComponents ?? (this._bestiaryRecipeComponents = new UniversalFileWrapper<BestiaryRecipeComponent>(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryRecipeComponent.dat"))));

    public UniversalFileWrapper<BestiaryGroup> BestiaryGroups => this._bestiaryGroups ?? (this._bestiaryGroups = new UniversalFileWrapper<BestiaryGroup>(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryGroups.dat"))));

    public UniversalFileWrapper<BestiaryFamily> BestiaryFamilies => this._bestiaryFamilies ?? (this._bestiaryFamilies = new UniversalFileWrapper<BestiaryFamily>(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryFamilies.dat"))));

    public UniversalFileWrapper<BestiaryGenus> BestiaryGenuses => this._bestiaryGenuses ?? (this._bestiaryGenuses = new UniversalFileWrapper<BestiaryGenus>(this._memory, (Func<long>) (() => this.FindFile("Data/BestiaryGenus.dat"))));

    public UniversalFileWrapper<SanctumRoom> SanctumRooms
    {
      get
      {
        UniversalFileWrapper<SanctumRoom> sanctumRooms1 = this._sanctumRooms;
        if (sanctumRooms1 != null)
          return sanctumRooms1;
        UniversalFileWrapper<SanctumRoom> universalFileWrapper = new UniversalFileWrapper<SanctumRoom>(this._memory, (Func<long>) (() => this.FindFile("Data/SanctumRooms.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<SanctumRoom> sanctumRooms2 = universalFileWrapper;
        this._sanctumRooms = universalFileWrapper;
        return sanctumRooms2;
      }
    }

    public UniversalFileWrapper<SanctumRoomType> SanctumRoomTypes
    {
      get
      {
        UniversalFileWrapper<SanctumRoomType> sanctumRoomTypes1 = this._sanctumRoomTypes;
        if (sanctumRoomTypes1 != null)
          return sanctumRoomTypes1;
        UniversalFileWrapper<SanctumRoomType> universalFileWrapper = new UniversalFileWrapper<SanctumRoomType>(this._memory, (Func<long>) (() => this.FindFile("Data/SanctumRoomTypes.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<SanctumRoomType> sanctumRoomTypes2 = universalFileWrapper;
        this._sanctumRoomTypes = universalFileWrapper;
        return sanctumRoomTypes2;
      }
    }

    public UniversalFileWrapper<SanctumDeferredRewardCategory> SanctumDeferredRewardCategories
    {
      get
      {
        UniversalFileWrapper<SanctumDeferredRewardCategory> rewardCategories1 = this._sanctumDeferredRewardCategories;
        if (rewardCategories1 != null)
          return rewardCategories1;
        UniversalFileWrapper<SanctumDeferredRewardCategory> universalFileWrapper = new UniversalFileWrapper<SanctumDeferredRewardCategory>(this._memory, (Func<long>) (() => this.FindFile("Data/SanctumDeferredRewardCategories.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<SanctumDeferredRewardCategory> rewardCategories2 = universalFileWrapper;
        this._sanctumDeferredRewardCategories = universalFileWrapper;
        return rewardCategories2;
      }
    }

    public UniversalFileWrapper<SanctumPersistentEffect> SanctumPersistentEffects
    {
      get
      {
        UniversalFileWrapper<SanctumPersistentEffect> persistentEffects1 = this._sanctumPersistentEffects;
        if (persistentEffects1 != null)
          return persistentEffects1;
        UniversalFileWrapper<SanctumPersistentEffect> universalFileWrapper = new UniversalFileWrapper<SanctumPersistentEffect>(this._memory, (Func<long>) (() => this.FindFile("Data/SanctumPersistentEffects.dat")));
        universalFileWrapper.ExcludeZeroAddresses = true;
        UniversalFileWrapper<SanctumPersistentEffect> persistentEffects2 = universalFileWrapper;
        this._sanctumPersistentEffects = universalFileWrapper;
        return persistentEffects2;
      }
    }
  }
}
