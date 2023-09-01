// Decompiled with JetBrains decompiler
// Type: ExileCore.EntityListWrapper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


#nullable enable
namespace ExileCore
{
  public class EntityListWrapper
  {
    private readonly 
    #nullable disable
    CoreSettings _settings;
    private readonly int coroutineTimeWait = 100;
    private readonly ConcurrentDictionary<uint, Entity> entityCache;
    private readonly GameController gameController;
    private readonly Queue<uint> keysForDelete = new Queue<uint>(24);
    private readonly Coroutine parallelUpdateDictionary;
    private readonly Stack<Entity> Simple = new Stack<Entity>(512);
    private readonly Coroutine updateEntity;
    private readonly EntityCollectSettingsContainer entityCollectSettingsContainer;
    private static EntityListWrapper _instance;
    private static readonly DebugInformation CollectEntitiesDebug = new DebugInformation("Collect Entities");

    public EntityListWrapper(
      GameController gameController,
      CoreSettings settings,
      MultiThreadManager multiThreadManager)
    {
      EntityListWrapper entityListWrapper = this;
      EntityListWrapper._instance = this;
      this.gameController = gameController;
      this._settings = settings;
      this.entityCache = new ConcurrentDictionary<uint, Entity>();
      gameController.Area.OnAreaChange += new Action<AreaInstance>(this.AreaChanged);
      this.EntitiesVersion = 0U;
      this.updateEntity = new Coroutine(new Action(this.RefreshState), (ExileCore.Shared.IYieldBase) new WaitTime(this.coroutineTimeWait), (IPlugin) null, "Update Entity")
      {
        Priority = CoroutinePriority.High,
        SyncModWork = true
      };
      this.entityCollectSettingsContainer = new EntityCollectSettingsContainer()
      {
        Simple = this.Simple,
        KeyForDelete = this.keysForDelete,
        EntityCache = this.entityCache,
        MultiThreadManager = multiThreadManager,
        ParseEntitiesInMultiThread = (Func<bool>) (() => (bool) settings.PerformanceSettings.ParseEntitiesInMultiThread),
        EntitiesCount = (Func<long>) (() => gameController.IngameState.Data.EntitiesCount),
        EntitiesVersion = this.EntitiesVersion,
        CollectEntitiesInParallelWhenMoreThanX = settings.PerformanceSettings.CollectEntitiesInParallelWhenMoreThanX,
        DebugInformation = EntityListWrapper.CollectEntitiesDebug
      };
      this.parallelUpdateDictionary = new Coroutine(Test(), (IPlugin) null, "Collect entities")
      {
        SyncModWork = true
      };
      this.UpdateCondition(1000 / (int) settings.PerformanceSettings.EntitiesFps);
      settings.PerformanceSettings.EntitiesFps.OnValueChanged += (EventHandler<int>) ((sender, i) => entityListWrapper.UpdateCondition(1000 / i));
      this.PlayerUpdate += (EventHandler<Entity>) ((sender, entity) => Entity.Player = entity);

      IEnumerator Test()
      {
        while (true)
        {
          yield return (object) gameController.IngameState.Data.EntityList.CollectEntities(entityListWrapper.entityCollectSettingsContainer);
          yield return (object) new WaitTime(1000 / (int) settings.PerformanceSettings.EntitiesFps);
          entityListWrapper.parallelUpdateDictionary.UpdateTicks((uint) ((ulong) entityListWrapper.parallelUpdateDictionary.Ticks + 1UL));
        }
      }
    }

    public ICollection<Entity> Entities => this.entityCache.Values;

    public uint EntitiesVersion { get; }

    public Entity Player { get; private set; }

    public List<Entity> OnlyValidEntities { get; private set; } = new List<Entity>();

    public List<Entity> NotOnlyValidEntities { get; private set; } = new List<Entity>();

    public Dictionary<uint, Entity> NotValidDict { get; private set; } = new Dictionary<uint, Entity>();

    public Dictionary<EntityType, List<Entity>> ValidEntitiesByType { get; private set; } = EntityListWrapper.PrepareEntityDictTemplate();

    public void StartWork()
    {
      Core.MainRunner.Run(this.updateEntity);
      Core.ParallelRunner.Run(this.parallelUpdateDictionary);
    }

    private void UpdateCondition(int coroutineTimeWait = 50)
    {
      this.parallelUpdateDictionary.UpdateCondtion((ExileCore.Shared.IYieldBase) new WaitTime(coroutineTimeWait));
      this.updateEntity.UpdateCondtion((ExileCore.Shared.IYieldBase) new WaitTime(coroutineTimeWait));
    }

    public event Action<Entity> EntityAdded;

    public event Action<Entity> EntityAddedAny;

    public event Action<Entity> EntityIgnored;

    public event Action<Entity> EntityRemoved;

    private void AreaChanged(AreaInstance area)
    {
      try
      {
        this.entityCollectSettingsContainer.Break = true;
        Entity localPlayer = this.gameController.Game.IngameState.Data.LocalPlayer;
        if (this.Player == null)
        {
          if (localPlayer != null && localPlayer.Path != null && localPlayer.Path.StartsWith("Meta"))
          {
            this.Player = localPlayer;
            this.Player.IsValid = true;
            EventHandler<Entity> playerUpdate = this.PlayerUpdate;
            if (playerUpdate != null)
              playerUpdate((object) this, this.Player);
          }
        }
        else if (this.Player.Address != localPlayer.Address && localPlayer.Path.StartsWith("Meta"))
        {
          this.Player = localPlayer;
          this.Player.IsValid = true;
          EventHandler<Entity> playerUpdate = this.PlayerUpdate;
          if (playerUpdate != null)
            playerUpdate((object) this, this.Player);
        }
        this.entityCache.Clear();
        this.OnlyValidEntities.Clear();
        this.NotOnlyValidEntities.Clear();
        foreach (KeyValuePair<EntityType, List<Entity>> keyValuePair in this.ValidEntitiesByType)
          keyValuePair.Value.Clear();
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
        interpolatedStringHandler.AppendFormatted(nameof (EntityListWrapper));
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
    }

    private void UpdateEntityCollections()
    {
      while (this.keysForDelete.Count > 0)
      {
        uint key = this.keysForDelete.Dequeue();
        Entity entity;
        if (this.entityCache.TryGetValue(key, out entity))
        {
          Action<Entity> entityRemoved = this.EntityRemoved;
          if (entityRemoved != null)
            entityRemoved(entity);
          this.entityCache.TryRemove(key, out Entity _);
        }
      }
      Dictionary<EntityType, List<Entity>> dictionary1 = EntityListWrapper.PrepareEntityDictTemplate();
      List<Entity> entityList1 = new List<Entity>();
      List<Entity> entityList2 = new List<Entity>();
      Dictionary<uint, Entity> dictionary2 = new Dictionary<uint, Entity>();
      foreach (KeyValuePair<uint, Entity> keyValuePair in this.entityCache)
      {
        Entity entity = keyValuePair.Value;
        if (entity.IsValid)
        {
          entityList1.Add(entity);
          dictionary1[entity.Type].Add(entity);
        }
        else
        {
          entityList2.Add(entity);
          dictionary2[entity.Id] = entity;
        }
      }
      this.ValidEntitiesByType = dictionary1;
      this.OnlyValidEntities = entityList1;
      this.NotOnlyValidEntities = entityList2;
      this.NotValidDict = dictionary2;
    }

    private static Dictionary<EntityType, List<Entity>> PrepareEntityDictTemplate()
    {
      Dictionary<EntityType, List<Entity>> dictionary = new Dictionary<EntityType, List<Entity>>();
      foreach (EntityType key in Enum.GetValues<EntityType>())
        dictionary[key] = new List<Entity>();
      return dictionary;
    }

    public void RefreshState()
    {
      if (this.gameController.Area.CurrentArea == null || this.entityCollectSettingsContainer.NeedUpdate || this.Player == null || !this.Player.IsValid)
        return;
      while (this.Simple.Count > 0)
      {
        Entity entity = this.Simple.Pop();
        if (entity == null)
        {
          DebugWindow.LogError("EntityListWrapper.RefreshState entity is null. (Very strange).");
        }
        else
        {
          uint id = entity.Id;
          if (!this.entityCache.TryGetValue(id, out Entity _) && (id < (uint) int.MaxValue || (bool) this._settings.PerformanceSettings.ParseServerEntities) && entity.Type != EntityType.Error && (entity.League != LeagueType.Legion || entity.Stats != null))
          {
            Action<Entity> entityAddedAny = this.EntityAddedAny;
            if (entityAddedAny != null)
              entityAddedAny(entity);
            if (entity.Type >= EntityType.Monster)
            {
              Action<Entity> entityAdded = this.EntityAdded;
              if (entityAdded != null)
                entityAdded(entity);
            }
            this.entityCache[id] = entity;
          }
        }
      }
      this.UpdateEntityCollections();
      this.entityCollectSettingsContainer.NeedUpdate = true;
    }

    public event EventHandler<Entity> PlayerUpdate;

    public static Entity GetEntityById(uint id)
    {
      Entity entity;
      return !EntityListWrapper._instance.entityCache.TryGetValue(id, out entity) ? (Entity) null : entity;
    }

    public string GetLabelForEntity(Entity entity)
    {
      HashSet<long> longSet = new HashSet<long>();
      long addr = this.gameController.Game.IngameState.EntityLabelMap;
      do
      {
        longSet.Add(addr);
        if (this.gameController.Memory.Read<long>(addr + 16L) != entity.Address)
          addr = this.gameController.Memory.Read<long>(addr);
        else
          goto label_4;
      }
      while (!longSet.Contains(addr) && addr != 0L && addr != -1L);
      return (string) null;
label_4:
      return this.gameController.Game.ReadObject<EntityLabel>(this.gameController.Memory.Read<long>(addr + 24L, 448) + 744L).Text;
    }
  }
}
