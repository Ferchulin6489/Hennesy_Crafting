// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.IngameUIElements
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.ExpeditionElements;
using ExileCore.PoEMemory.Elements.Sanctum;
using ExileCore.PoEMemory.MemoryObjects.Ancestor;
using ExileCore.PoEMemory.MemoryObjects.Metamorph;
using ExileCore.Shared.Cache;
using GameOffsets;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class IngameUIElements : Element
  {
    private SyndicatePanel _syndicatePanel;
    private readonly CachedValue<IngameUIElementsOffsets> _cachedValue;
    private CraftBenchWindow _CraftBench;
    private Cursor _cursor;
    private IncursionWindow _IncursionWindow;
    private Map _map;
    private Element _SynthesisWindow;
    private Element _UnveilWindow;
    private Element _ZanaMissionChoice;
    private readonly CachedValue<Dictionary<string, KeyValuePair<Quest, QuestState>>> _cachedQuestStates;
    private RitualWindow _ritualWindow;

    public IngameUIElements()
    {
      this._cachedValue = (CachedValue<IngameUIElementsOffsets>) new FrameCache<IngameUIElementsOffsets>((Func<IngameUIElementsOffsets>) (() => this.M.Read<IngameUIElementsOffsets>(this.Address)));
      this._cachedQuestStates = (CachedValue<Dictionary<string, KeyValuePair<Quest, QuestState>>>) new TimeCache<Dictionary<string, KeyValuePair<Quest, QuestState>>>(new Func<Dictionary<string, KeyValuePair<Quest, QuestState>>>(this.GenerateQuestStates), 1000L);
    }

    public IngameUIElementsOffsets IngameUIElementsStruct => this._cachedValue.Value;

    public GameUi GameUI => this.GetObject<GameUi>(this.IngameUIElementsStruct.GameUI);

    public SellWindow SellWindow => this.GetObject<SellWindow>(this.IngameUIElementsStruct.SellWindow);

    public SellWindowHideout SellWindowHideout => this.GetObject<SellWindowHideout>(this.IngameUIElementsStruct.SellWindowHideout);

    public MapDeviceWindow MapDeviceWindow => this.GetObject<MapDeviceWindow>(this.IngameUIElementsStruct.MapDeviceWindow);

    public TradeWindow TradeWindow => this.GetObject<TradeWindow>(this.IngameUIElementsStruct.TradeWindow);

    public NpcDialog NpcDialog => this.GetObject<NpcDialog>(this.IngameUIElementsStruct.NpcDialog);

    public BanditDialog BanditDialog => this.GetObject<BanditDialog>(this.IngameUIElementsStruct.BanditDialog);

    public Element SocialPanel => this.GetObject<Element>(this.IngameUIElementsStruct.SocialPanel);

    public PurchaseWindow PurchaseWindow => this.GetObject<PurchaseWindow>(this.IngameUIElementsStruct.PurchaseWindow);

    public PurchaseWindow PurchaseWindowHideout => this.GetObject<PurchaseWindow>(this.IngameUIElementsStruct.PurchaseWindowHideout);

    public SubterraneanChart DelveWindow => this.GetObject<SubterraneanChart>(this.IngameUIElementsStruct.DelveWindow);

    public SkillBarElement SkillBar => this.GetObject<SkillBarElement>(this.IngameUIElementsStruct.SkillBar);

    public SkillBarElement HiddenSkillBar => this.GetObject<SkillBarElement>(this.IngameUIElementsStruct.HiddenSkillBar);

    public ChatPanel ChatPanel => this.GetObject<ChatPanel>(this.IngameUIElementsStruct.ChatBox);

    public Element ChatTitlePanel => this.ChatPanel.ReadObjectAt<Element>(768);

    public PoeChatElement ChatBox => this.ChatPanel.ReadObjectAt<Element>(816).ReadObjectAt<PoeChatElement>(912);

    public IList<string> ChatMessages => (IList<string>) this.ChatBox.Messages;

    public Element QuestTracker => this.GetObject<Element>(this.IngameUIElementsStruct.QuestTracker);

    public QuestRewardWindow QuestRewardWindow => this.GetObject<QuestRewardWindow>(this.IngameUIElementsStruct.QuestRewardWindow);

    public Element OpenLeftPanel => this.GetObject<Element>(this.IngameUIElementsStruct.OpenLeftPanel);

    public Element OpenRightPanel => this.GetObject<Element>(this.IngameUIElementsStruct.OpenRightPanel);

    public StashElement StashElement => this.GetObject<StashElement>(this.IngameUIElementsStruct.StashElement);

    public StashElement GuildStashElement => this.GetObject<StashElement>(this.IngameUIElementsStruct.GuildStashElement);

    public InventoryElement InventoryPanel => this.GetObject<InventoryElement>(this.IngameUIElementsStruct.InventoryPanel);

    public TreePanel TreePanel => this.GetObject<TreePanel>(this.IngameUIElementsStruct.TreePanel);

    public TreePanel AtlasTreePanel => this.GetObject<TreePanel>(this.IngameUIElementsStruct.AtlasSkillPanel);

    public Element PVPTreePanel => this.GetChildAtIndex(26);

    public ExileCore.PoEMemory.Elements.AtlasPanel Atlas => this.GetObject<ExileCore.PoEMemory.Elements.AtlasPanel>(this.IngameUIElementsStruct.AtlasPanel);

    public Element SettingsPanel => this.GetObject<Element>(this.IngameUIElementsStruct.SettingsPanel);

    public Element HelpWindow => this.GetObject<Element>(this.IngameUIElementsStruct.HelpWindow);

    public Element SentinelWindow => this.GetObject<Element>(this.IngameUIElementsStruct.SentinelWindow);

    public Map Map => this._map ?? (this._map = this.GetObject<Map>(this.IngameUIElementsStruct.Map));

    public ItemsOnGroundLabelElement ItemsOnGroundLabelElement => this.GetObject<ItemsOnGroundLabelElement>(this.IngameUIElementsStruct.itemsOnGroundLabelRoot);

    public IList<LabelOnGround> ItemsOnGroundLabels => (IList<LabelOnGround>) this.ItemsOnGroundLabelElement.LabelsOnGround;

    public IList<LabelOnGround> ItemsOnGroundLabelsVisible
    {
      get
      {
        List<LabelOnGround> labelsOnGround = this.ItemsOnGroundLabelElement.LabelsOnGround;
        return (labelsOnGround != null ? (IList<LabelOnGround>) labelsOnGround.Where<LabelOnGround>((Func<LabelOnGround, bool>) (x => x.IsVisible)).ToList<LabelOnGround>() : (IList<LabelOnGround>) null) ?? (IList<LabelOnGround>) new List<LabelOnGround>();
      }
    }

    public GemLvlUpPanel GemLvlUpPanel => this.GetObject<GemLvlUpPanel>(this.IngameUIElementsStruct.GemLvlUpPanel);

    public Element InvitesPanel => this.GetObject<Element>(this.IngameUIElementsStruct.InvitesPanel);

    public ItemOnGroundTooltip ItemOnGroundTooltip => this.GetObject<ItemOnGroundTooltip>(this.IngameUIElementsStruct.ItemOnGroundTooltip);

    public MapStashTabElement MapStashTab => this.ReadObject<MapStashTabElement>(this.IngameUIElementsStruct.MapTabWindowStartPtr + 2720L);

    public Element Sulphit => this.GetObject<Element>(this.IngameUIElementsStruct.Map).GetChildAtIndex(3);

    public Element MapSideUI => this.GetObject<Element>(this.IngameUIElementsStruct.MapSideUI);

    public Cursor Cursor => this._cursor ?? (this._cursor = this.GetObject<Cursor>(this.IngameUIElementsStruct.Mouse));

    public Element SyndicateTree => this.GetObject<Element>(this.M.Read<long>(this.SyndicatePanel.Address + 2640L));

    public Element UnveilWindow => this._UnveilWindow ?? (this._UnveilWindow = this.GetObject<Element>(this.IngameUIElementsStruct.UnveilWindow));

    public Element ZanaMissionChoice => this._ZanaMissionChoice ?? (this._ZanaMissionChoice = this.GetObject<Element>(this.IngameUIElementsStruct.ZanaMissionChoice));

    public IncursionWindow IncursionWindow => this._IncursionWindow ?? (this._IncursionWindow = this.GetObject<IncursionWindow>(this.IngameUIElementsStruct.IncursionWindow));

    public Element SynthesisWindow => this._SynthesisWindow ?? (this._SynthesisWindow = this.GetObject<Element>(this.IngameUIElementsStruct.SynthesisWindow));

    public Element AnointingWindow => this.GetObject<Element>(this.IngameUIElementsStruct.AnointingWindow);

    public CraftBenchWindow CraftBench => this._CraftBench ?? (this._CraftBench = this.GetObject<CraftBenchWindow>(this.IngameUIElementsStruct.CraftBenchWindow));

    [Obsolete]
    public bool IsDndEnabled => this.M.Read<byte>(this.Address + 3986L) == (byte) 1;

    [Obsolete]
    public string DndMessage => this.M.ReadStringU(this.M.Read<long>(this.Address + 3992L));

    public WorldMapElement AreaInstanceUi => this.GetObject<WorldMapElement>(this.IngameUIElementsStruct.AreaInstanceUi);

    public WorldMapElement WorldMap => this.GetObject<WorldMapElement>(this.IngameUIElementsStruct.WorldMap);

    public MetamorphWindowElement MetamorphWindow => this.GetObject<MetamorphWindowElement>(this.IngameUIElementsStruct.MetamorphWindow);

    public SyndicatePanel SyndicatePanel => this._syndicatePanel ?? (this._syndicatePanel = this.GetObject<SyndicatePanel>(this.IngameUIElementsStruct.BetrayalWindow));

    public InstanceManagerPanel InstanceManagerPanel => this.GetObject<InstanceManagerPanel>(this.IngameUIElementsStruct.InstanceManagerPanel);

    public ResurrectPanel ResurrectPanel => this.GetObject<ResurrectPanel>(this.IngameUIElementsStruct.ResurrectPanel);

    public MapReceptacleWindow MapReceptacleWindow => this.GetObject<MapReceptacleWindow>(this.IngameUIElementsStruct.MapReceptacleWindow);

    public Element PopUpWindow => this.GetObject<Element>(this.IngameUIElementsStruct.PopUpWindow);

    public CardTradeWindow CardTradeWindow => this.GetObject<CardTradeWindow>(this.IngameUIElementsStruct.CardTradeWindow);

    public RitualWindow RitualWindow => this._ritualWindow ?? (this._ritualWindow = this.GetObject<RitualWindow>(this.IngameUIElementsStruct.RitualWindow));

    public Element TrialPlaquePanel => this.GetObject<Element>(this.IngameUIElementsStruct.TrialPlaquePanel);

    public Element LabyrinthSelectPanel => this.GetObject<Element>(this.IngameUIElementsStruct.LabyrinthSelectPanel);

    public Element LabyrinthMapPanel => this.GetObject<Element>(this.IngameUIElementsStruct.LabyrinthMapPanel);

    public Element AscendancySelectPanel => this.GetObject<Element>(this.IngameUIElementsStruct.AscendancySelectPanel);

    public Element LabyrinthDivineFontPanel => this.GetObject<Element>(this.IngameUIElementsStruct.LabyrinthDivineFontPanel);

    public Element ChallengesPanel => this.GetObject<Element>(this.IngameUIElementsStruct.ChallengePanel);

    public ExpeditionVendorElement HaggleWindow => this.GetObject<ExpeditionVendorElement>(this.IngameUIElementsStruct.HaggleWindow);

    public Element ExpeditionNpcDialog => this.GetObject<Element>(this.IngameUIElementsStruct.ExpeditionNpcDialog);

    public Element ExpeditionWindow => this.GetObject<Element>(this.IngameUIElementsStruct.ExpeditionWindow);

    public Element ExpeditionWindowEmpty => this.GetObject<Element>(this.IngameUIElementsStruct.ExpeditionWindowEmpty);

    public Element ExpeditionLockerElement => this.GetObject<Element>(this.IngameUIElementsStruct.ExpeditionLockerElement);

    public SanctumFloorWindow SanctumFloorWindow => this.GetObject<SanctumFloorWindow>(this.IngameUIElementsStruct.SanctumFloorWindow);

    public Element HeistWindow => this.GetObject<Element>(this.IngameUIElementsStruct.HeistWindow);

    public Element BlueprintWindow => this.GetObject<Element>(this.IngameUIElementsStruct.BlueprintWindow);

    public Element HeistLockerElement => this.GetObject<Element>(this.IngameUIElementsStruct.HeistLockerElement);

    public Element AllyEquipmentWindow => this.GetObject<Element>(this.IngameUIElementsStruct.AllyEquipmentWindow);

    public AncestorFightSelectionWindow AncestorFightSelectionWindow => this.GetObject<AncestorFightSelectionWindow>(this.IngameUIElementsStruct.AncestorFightSelectionWindow);

    public AncestorMainShopWindow AncestorMainShopWindow => this.GetObject<AncestorMainShopWindow>(this.IngameUIElementsStruct.AncestorMainShopWindow);

    public Element GrandHeistWindow => this.GetObject<Element>(this.IngameUIElementsStruct.GrandHeistWindow);

    public Element CurrencyShiftClickMenu => this.GetObject<Element>(this.IngameUIElementsStruct.CurrencyShiftClickMenu);

    public Element PartyElement => this.GetObject<Element>(this.IngameUIElementsStruct.PartyElement);

    public HarvestWindow HorticraftingStationWindow => this.GetObject<HarvestWindow>(this.IngameUIElementsStruct.HorticraftingStationWindow);

    public UltimatumPanel UltimatumPanel => this.GetObject<UltimatumPanel>(this.IngameUIElementsStruct.UltimatumPanel);

    public KalandraTabletWindow KalandraTabletWindow => this.GetObject<KalandraTabletWindow>(this.IngameUIElementsStruct.KalandraTabletWindow);

    public Element HighlightedElement => this.Root?.GetChildFromIndices(1, 6, 1, 0);

    public IList<Tuple<Quest, int>> GetUncompletedQuests => (IList<Tuple<Quest, int>>) this.GetQuestStates.Where<KeyValuePair<string, KeyValuePair<Quest, QuestState>>>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, bool>) (q => q.Value.Value != null && q.Value.Value.QuestStateId != 0)).Select<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, Tuple<Quest, int>>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, Tuple<Quest, int>>) (x =>
    {
      KeyValuePair<Quest, QuestState> keyValuePair = x.Value;
      Quest key = keyValuePair.Key;
      keyValuePair = x.Value;
      int questStateId = keyValuePair.Value.QuestStateId;
      return new Tuple<Quest, int>(key, questStateId);
    })).ToList<Tuple<Quest, int>>();

    public IList<Tuple<Quest, int>> GetCompletedQuests => (IList<Tuple<Quest, int>>) this.GetQuestStates.Where<KeyValuePair<string, KeyValuePair<Quest, QuestState>>>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, bool>) (q => q.Value.Value != null && q.Value.Value.QuestStateId == 0)).Select<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, Tuple<Quest, int>>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, Tuple<Quest, int>>) (x =>
    {
      KeyValuePair<Quest, QuestState> keyValuePair = x.Value;
      Quest key = keyValuePair.Key;
      keyValuePair = x.Value;
      int questStateId = keyValuePair.Value.QuestStateId;
      return new Tuple<Quest, int>(key, questStateId);
    })).ToList<Tuple<Quest, int>>();

    public Dictionary<Quest, QuestState> GetUncompletedQuests2 => this.GetQuestStates.Where<KeyValuePair<string, KeyValuePair<Quest, QuestState>>>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, bool>) (q => q.Value.Value != null && q.Value.Value.QuestStateId > 0 && q.Value.Value.QuestStateId < (int) byte.MaxValue)).Select<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, QuestState>((Func<KeyValuePair<string, KeyValuePair<Quest, QuestState>>, QuestState>) (x => x.Value.Value)).ToDictionary<QuestState, Quest>((Func<QuestState, Quest>) (x => x.Quest));

    public Dictionary<string, KeyValuePair<Quest, QuestState>> GetQuestStates => this._cachedQuestStates.Value;

    public Element LeagueMechanicButtons => this.GetObject<Element>(this.IngameUIElementsStruct.LeagueMechanicButtons);

    public ExpeditionDetonator ExpeditionDetonatorElement => this.GetObject<ExpeditionDetonator>(this.IngameUIElementsStruct.ExpeditionDetonatorElement);

    [Obsolete("Removed from the game")]
    public ArchnemesisPanelElement ArchnemesisInventoryPanel => this.GetObject<ArchnemesisPanelElement>(0L);

    [Obsolete("Removed from the game")]
    public ArchnemesisAltarElement ArchnemesisAltarPanel => this.GetObject<ArchnemesisAltarElement>(0L);

    [Obsolete("Removed from the game")]
    public HarvestWindow HarvestWindow => this.GetObject<HarvestWindow>(0L);

    [Obsolete("Use PopUpWindow instead")]
    public Element DestroyConfirmationWindow => this.PopUpWindow;

    [Obsolete("Use SyndicatePanel instead")]
    public Element BetrayalWindow => (Element) this.SyndicatePanel;

    [Obsolete("Use Atlas instead")]
    public Element AtlasPanel => (Element) this.Atlas;

    private Dictionary<string, KeyValuePair<Quest, QuestState>> GenerateQuestStates()
    {
      if (this.IngameUIElementsStruct.GetQuests == 0L)
        return new Dictionary<string, KeyValuePair<Quest, QuestState>>();
      Dictionary<string, KeyValuePair<Quest, QuestState>> questStates = new Dictionary<string, KeyValuePair<Quest, QuestState>>();
      foreach ((Quest, int) getQuest in (IEnumerable<(Quest, int)>) this.GetQuests)
      {
        if (getQuest.Item1 != null)
        {
          QuestState questState = this.TheGame.Files.QuestStates.GetQuestState(getQuest.Item1.Id, getQuest.Item2);
          questStates.TryAdd(getQuest.Item1.Id, new KeyValuePair<Quest, QuestState>(getQuest.Item1, questState));
        }
      }
      return questStates;
    }

    public IList<(long, int)> ReadQuestsList(long address)
    {
      List<(long, int)> valueTupleList = new List<(long, int)>();
      long addr = this.M.Read<long>(address);
      IngameUIElements.QuestListNode questListNode = this.M.Read<IngameUIElements.QuestListNode>(addr);
      valueTupleList.Add((questListNode.Ptr2_Key, (int) questListNode.Value));
      Stopwatch stopwatch = Stopwatch.StartNew();
      while (addr != questListNode.Next)
      {
        if (stopwatch.ElapsedMilliseconds > 2000L)
        {
          ILogger logger = Core.Logger;
          if (logger != null)
          {
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
            interpolatedStringHandler.AppendLiteral("ReadQuestsList error result count: ");
            interpolatedStringHandler.AppendFormatted<int>(valueTupleList.Count);
            logger.Error(interpolatedStringHandler.ToStringAndClear());
          }
          return (IList<(long, int)>) new List<(long, int)>();
        }
        questListNode = this.M.Read<IngameUIElements.QuestListNode>(questListNode.Next);
        valueTupleList.Add((questListNode.Ptr2_Key, (int) questListNode.Value));
      }
      if (valueTupleList.Count > 0)
        valueTupleList.RemoveAt(valueTupleList.Count - 1);
      return (IList<(long, int)>) valueTupleList;
    }

    public IList<(Quest, int)> GetQuests => this.IngameUIElementsStruct.GetQuests == 0L ? (IList<(Quest, int)>) new List<(Quest, int)>() : (IList<(Quest, int)>) this.ReadQuestsList(this.IngameUIElementsStruct.GetQuests).Select<(long, int), (Quest, int)>((Func<(long, int), (Quest, int)>) (x => (this.TheGame.Files.Quests.GetByAddress(x.Item1), x.Item2))).ToList<(Quest, int)>();

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QuestListNode
    {
      public long Next;
      public long Prev;
      public long Ptr2_Key;
      public long Ptr1_Unused;
      public char Value;
    }
  }
}
