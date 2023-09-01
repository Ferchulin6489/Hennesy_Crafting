// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Entity
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class Entity : RemoteMemoryObject
  {
    internal static readonly int IdOffset = Extensions.GetOffset<EntityOffsets>((Expression<Func<EntityOffsets, object>>) (x => (object) x.Id));
    internal static readonly int FlagsOffset = Extensions.GetOffset<EntityOffsets>((Expression<Func<EntityOffsets, object>>) (x => (object) x.Flags));
    private readonly object _cacheComponentsLock = new object();
    private readonly Dictionary<System.Type, Component> _cacheComponents = new Dictionary<System.Type, Component>(4);
    private readonly CachedValue<bool> _hiddenCheckCache;
    private System.Numerics.Vector3 _boundsCenterPos = System.Numerics.Vector3.Zero;
    private Dictionary<string, long> _cacheComponents2;
    private float _distancePlayer = float.MaxValue;
    private EntityOffsets? _entityOffsets;
    private System.Numerics.Vector2 _gridPos = System.Numerics.Vector2.Zero;
    private long? _id;
    private uint? _inventoryId;
    private bool _isAlive;
    private bool _isDead;
    private CachedValue<bool> _isHostile;
    private bool _isOpened;
    private bool _isTargetable;
    private string _metadata;
    private string _path;
    private System.Numerics.Vector3 _pos = System.Numerics.Vector3.Zero;
    private MonsterRarity? _rarity;
    private string _renderName = "Empty";
    private Dictionary<GameStat, int> _stats;
    private readonly ValidCache<List<Buff>> buffCache;
    private bool isHidden;
    private readonly object locker = new object();
    private int pathReadErrorTimes;

    public Entity()
    {
      this._hiddenCheckCache = (CachedValue<bool>) new LatancyCache<bool>((Func<bool>) (() =>
      {
        if (this.IsValid)
        {
          ExileCore.PoEMemory.Components.Buffs component = this.GetComponent<ExileCore.PoEMemory.Components.Buffs>();
          this.isHidden = component != null && component.HasBuff("hidden_monster");
        }
        return this.isHidden;
      }), 50);
      this.buffCache = this.ValidCache<List<Buff>>((Func<List<Buff>>) (() => this.GetComponent<ExileCore.PoEMemory.Components.Buffs>()?.BuffsList));
    }

    public static Entity Player { get; set; }

    private static Dictionary<string, string> ComponentsName { get; } = new Dictionary<string, string>();

    public float DistancePlayer
    {
      get
      {
        if (Entity.Player == null)
          return this._distancePlayer;
        int num = this.IsValid ? 1 : 0;
        this._distancePlayer = Entity.Player.GridPosNum.Distance(this.GridPosNum);
        return this._distancePlayer;
      }
    }

    public EntityOffsets EntityOffsets
    {
      get
      {
        if (this._entityOffsets.HasValue)
          return this._entityOffsets.Value;
        if (this.Address != 0L)
          this._entityOffsets = new EntityOffsets?(this.M.Read<EntityOffsets>(this.Address));
        if (this._entityOffsets.HasValue)
          return this._entityOffsets.Value;
        this.IsValid = false;
        return new EntityOffsets();
      }
    }

    public EntityType Type { get; private set; }

    public LeagueType League { get; private set; }

    public StdVector ComponentList => this.EntityOffsets.ComponentList;

    public EntityFlags Flags => this.M.Read<EntityFlags>(this.Address + (long) Entity.FlagsOffset);

    public bool IsHidden => this._hiddenCheckCache.Value;

    public string Debug
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
        interpolatedStringHandler.AppendFormatted<long>(this.EntityOffsets.Head.MainObject, "X");
        interpolatedStringHandler.AppendLiteral(" List: ");
        interpolatedStringHandler.AppendFormatted<StdVector>(this.EntityOffsets.ComponentList);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    public uint Version { get; set; }

    public bool IsValid { get; set; }

    public bool IsAlive
    {
      get
      {
        if (!this.IsValid)
          return this._isAlive;
        Life component = this.GetComponent<Life>();
        if (component == null || component.OwnerAddress != this.Address)
        {
          if ((double) this._distancePlayer < 70.0)
            this._isAlive = false;
          return this._isAlive;
        }
        this._isAlive = component.CurHP > 0;
        return this._isAlive;
      }
    }

    [Obsolete]
    public SharpDX.Vector3 Pos => this.PosNum.ToSharpDx();

    public System.Numerics.Vector3 PosNum
    {
      get
      {
        if (!this.IsValid)
          return this._pos;
        Render component1 = this.GetComponent<Render>();
        if (component1 != null)
        {
          this._pos.X = component1.X;
          this._pos.Y = component1.Y;
          this._pos.Z = component1.Z + component1.BoundsNum.Z;
        }
        else
        {
          Positioned component2 = this.GetComponent<Positioned>();
          if (component2 != null)
          {
            this._pos.X = component2.WorldX;
            this._pos.Y = component2.WorldY;
          }
        }
        return this._pos;
      }
    }

    [Obsolete]
    public SharpDX.Vector3 BoundsCenterPos => this.BoundsCenterPosNum.ToSharpDx();

    public System.Numerics.Vector3 BoundsCenterPosNum
    {
      get
      {
        if (!this.IsValid)
          return this._boundsCenterPos;
        Render component = this.GetComponent<Render>();
        if (component == null)
          return this._boundsCenterPos;
        this._boundsCenterPos = component.InteractCenterNum;
        return this._boundsCenterPos;
      }
    }

    [Obsolete]
    public SharpDX.Vector2 GridPos => this.GridPosNum.ToSharpDx();

    public System.Numerics.Vector2 GridPosNum
    {
      get
      {
        if (!this.IsValid)
          return this._gridPos;
        Positioned component = this.GetComponent<Positioned>();
        if (component == null || component.OwnerAddress != this.Address)
          return this._gridPos;
        this._gridPos = component.GridPosNum;
        return this._gridPos;
      }
    }

    public string RenderName
    {
      get
      {
        if (!this.IsValid)
          return this._renderName;
        Render component = this.GetComponent<Render>();
        if (component == null)
          return this._renderName;
        this._renderName = component.NameNoCache;
        return this._renderName;
      }
    }

    public MonsterRarity Rarity
    {
      get
      {
        MonsterRarity? nullable = this._rarity;
        int num;
        if (!nullable.HasValue)
        {
          ObjectMagicProperties component = this.GetComponent<ObjectMagicProperties>();
          num = component != null ? (int) component.Rarity : 0;
        }
        else
          num = (int) nullable.GetValueOrDefault();
        this._rarity = nullable = new MonsterRarity?((MonsterRarity) num);
        nullable = nullable;
        return nullable.Value;
      }
    }

    public bool IsOpened
    {
      get
      {
        if (!this.IsValid)
          return this._isOpened;
        Chest component1 = this.GetComponent<Chest>();
        if (component1 == null)
          return this._isOpened;
        Targetable component2 = this.GetComponent<Targetable>();
        if (component2 == null)
          return this._isOpened;
        this._isOpened = !component2.isTargetable || component1.IsOpened;
        return this._isOpened;
      }
    }

    public bool IsDead
    {
      get
      {
        if (!this.IsValid)
          return this._isDead;
        this._isDead = !this._isAlive;
        return this._isDead;
      }
    }

    public Dictionary<GameStat, int> Stats
    {
      get
      {
        if (!this.IsValid)
          return this._stats;
        ExileCore.PoEMemory.Components.Stats stats = this.GetComponent<ExileCore.PoEMemory.Components.Stats>();
        if (stats == null)
          return this._stats;
        if (stats.OwnerAddress != this.Address)
        {
          stats = this.GetComponentFromMemory<ExileCore.PoEMemory.Components.Stats>();
          if (stats.OwnerAddress != this.Address)
            return this._stats;
        }
        Dictionary<GameStat, int> statDictionary = stats.StatDictionary;
        if (statDictionary.Count == 0 && (this._stats == null || this._stats.Count != 0))
          return this._stats;
        this._stats = statDictionary;
        return this._stats;
      }
    }

    public bool IsTargetable
    {
      get
      {
        if (!this.IsValid)
        {
          if (this._isTargetable && (double) this.DistancePlayer < 200.0)
            this._isTargetable = false;
          return this._isTargetable;
        }
        Targetable component = this.GetComponent<Targetable>();
        this._isTargetable = component != null && component.isTargetable;
        return this._isTargetable;
      }
    }

    public bool IsTransitioned => this.IsTransitionedHelper();

    private bool IsTransitionedHelper()
    {
      byte? flag1 = this.GetComponent<Transitionable>()?.Flag1;
      return flag1.HasValue && flag1.Value == (byte) 2;
    }

    public List<Buff> Buffs => this.buffCache.Value;

    private string CachePath { get; set; }

    public string Path
    {
      get
      {
        if (this._path == null)
        {
          if (this.EntityOffsets.Head.MainObject == 0L)
          {
            if (this.CachePath != null)
              return this.CachePath;
            this.IsValid = false;
            return (string) null;
          }
          PathEntityOffsets p = this.M.Read<PathEntityOffsets>(this.EntityOffsets.Head.MainObject);
          if (p.Path.Ptr == 0L)
          {
            if (this.CachePath != null)
              return this.CachePath;
            this.IsValid = false;
            return (string) null;
          }
          IStaticCache<string> stringCache1 = RemoteMemoryObject.Cache.StringCache;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted<long>(p.Path.Ptr);
          interpolatedStringHandler.AppendFormatted<long>(p.Length);
          string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func = (Func<string>) (() => p.ToString(this.M));
          this._path = stringCache1.Read(stringAndClear1, func);
          if (!this._path.StartsWith("Metadata"))
          {
            this._path = this.M.Read<PathEntityOffsets>(this.EntityOffsets.Head.MainObject).ToString(this.M);
            IStaticCache<string> stringCache2 = RemoteMemoryObject.Cache.StringCache;
            interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
            interpolatedStringHandler.AppendFormatted<long>(p.Path.Ptr);
            interpolatedStringHandler.AppendFormatted<long>(p.Length);
            string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
            stringCache2.Remove(stringAndClear2);
          }
          if (this._path.Length > 0 && this._path[0] != 'M')
          {
            ++this.pathReadErrorTimes;
            this.IsValid = false;
            this._path = (string) null;
            if (this.pathReadErrorTimes > 10)
            {
              this._path = "ERROR PATH";
              DebugWindow.LogError("Entity path error.");
            }
          }
          else
            this.CachePath = this._path;
        }
        return this._path;
      }
    }

    public string Metadata
    {
      get
      {
        if (this._metadata == null && this.Path != null)
        {
          int length = this.Path.IndexOf("@", StringComparison.Ordinal);
          if (length == -1)
            return this.Path;
          this._metadata = this.Path.Substring(0, length);
        }
        return this._metadata;
      }
    }

    public uint Id
    {
      get
      {
        long valueOrDefault = this._id.GetValueOrDefault();
        long id;
        if (!this._id.HasValue)
        {
          long num = (long) this.M.Read<uint>(this.Address + (long) Entity.IdOffset);
          this._id = new long?(num);
          id = num;
        }
        else
          id = valueOrDefault;
        return (uint) id;
      }
    }

    public uint InventoryId
    {
      get
      {
        uint valueOrDefault = this._inventoryId.GetValueOrDefault();
        if (this._inventoryId.HasValue)
          return valueOrDefault;
        uint inventoryId = this.M.Read<uint>(this.Address + 112L);
        this._inventoryId = new uint?(inventoryId);
        return inventoryId;
      }
    }

    public Dictionary<string, long> CacheComp => this._cacheComponents2 ?? (this._cacheComponents2 = this.GetComponents());

    public bool IsHostile
    {
      get
      {
        CachedValue<bool> isHostile = this._isHostile;
        return isHostile == null ? (this._isHostile = (CachedValue<bool>) new TimeCache<bool>((Func<bool>) (() =>
        {
          byte? reaction = this.GetComponent<Positioned>()?.Reaction;
          int? nullable = reaction.HasValue ? new int?((int) reaction.GetValueOrDefault() & (int) sbyte.MaxValue) : new int?();
          int num = 1;
          return !(nullable.GetValueOrDefault() == num & nullable.HasValue);
        }), 100L)).Value : isHostile.Value;
      }
    }

    private Dictionary<System.Type, object> PluginData { get; } = new Dictionary<System.Type, object>();

    public event EventHandler<Entity> OnUpdate;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 4);
      interpolatedStringHandler.AppendLiteral("<");
      interpolatedStringHandler.AppendFormatted<EntityType>(this.Type);
      interpolatedStringHandler.AppendLiteral("> (");
      interpolatedStringHandler.AppendFormatted<MonsterRarity>(this.Rarity);
      interpolatedStringHandler.AppendLiteral(") ");
      interpolatedStringHandler.AppendFormatted(this.Metadata);
      interpolatedStringHandler.AppendLiteral(": (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }

    public float Distance(Entity entity) => this.GridPosNum.Distance(entity.GridPosNum);

    protected override void OnAddressChange()
    {
      this._entityOffsets = new EntityOffsets?(this.M.Read<EntityOffsets>(this.Address));
      this._inventoryId = new uint?();
      this._pos = System.Numerics.Vector3.Zero;
      this._cacheComponents.Clear();
      this._cacheComponents2 = (Dictionary<string, long>) null;
      if (this.Type == EntityType.Error)
      {
        this.Type = this.ParseType();
        if (this.Type != EntityType.Error)
          this.IsValid = true;
      }
      EventHandler<Entity> onUpdate = this.OnUpdate;
      if (onUpdate == null)
        return;
      onUpdate((object) this, this);
    }

    public bool Check(uint entityId)
    {
      if (this._id.HasValue)
      {
        long? id = this._id;
        long num = (long) entityId;
        if (!(id.GetValueOrDefault() == num & id.HasValue))
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 3);
          interpolatedStringHandler.AppendLiteral("Was ID: ");
          interpolatedStringHandler.AppendFormatted<uint>(this.Id);
          interpolatedStringHandler.AppendLiteral(" New ID: ");
          interpolatedStringHandler.AppendFormatted<uint>(entityId);
          interpolatedStringHandler.AppendLiteral(" To Path: ");
          interpolatedStringHandler.AppendFormatted(this.Path);
          DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 3f);
          this._id = new long?((long) entityId);
          this._path = (string) null;
          this._metadata = (string) null;
          this.Type = this.ParseType();
        }
      }
      if (this.Type == EntityType.Error)
        return false;
      if (this.Type == EntityType.Effect || this.Type == EntityType.Daemon)
        return true;
      return this.CacheComp != null && (int) this.Id == (int) entityId && this.CheckRarity();
    }

    private bool CheckRarity() => this.Rarity >= MonsterRarity.White && this.Rarity <= MonsterRarity.Unique;

    public void UpdatePointer(long newAddress) => this.Address = newAddress;

    private Dictionary<string, long> GetComponents()
    {
      lock (this.locker)
      {
        try
        {
          Dictionary<string, long> components = new Dictionary<string, long>();
          long[] numArray = this.M.ReadStdVector<long>(this.ComponentList);
          ComponentLookUpStruct componentLookUpStruct = this.M.Read<ComponentLookUpStruct>(this.M.Read<ObjectHeaderOffsets>(this.EntityOffsets.EntityDetailsPtr).ComponentLookUpPtr);
          foreach (ComponentArrayStructure componentArrayStructure in this.M.ReadMem<ComponentArrayStructure>(componentLookUpStruct.ComponentArray, ((int) componentLookUpStruct.Capacity + 1) / 8))
          {
            if ((int) componentArrayStructure.Flag0 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer0.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer0.Index]);
            }
            if ((int) componentArrayStructure.Flag1 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer1.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer1.Index]);
            }
            if ((int) componentArrayStructure.Flag2 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer2.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer2.Index]);
            }
            if ((int) componentArrayStructure.Flag3 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer3.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer3.Index]);
            }
            if ((int) componentArrayStructure.Flag4 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer4.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer4.Index]);
            }
            if ((int) componentArrayStructure.Flag5 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer5.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer5.Index]);
            }
            if ((int) componentArrayStructure.Flag6 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer6.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer6.Index]);
            }
            if ((int) componentArrayStructure.Flag7 != (int) ComponentArrayStructure.InValidPointerFlagValue)
            {
              long strPtr = componentArrayStructure.Pointer7.NamePtr;
              IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
              interpolatedStringHandler.AppendFormatted(nameof (Entity));
              interpolatedStringHandler.AppendFormatted<long>(strPtr);
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              Func<string> func = (Func<string>) (() => this.M.ReadString(strPtr));
              string key = stringCache.Read(stringAndClear, func);
              if (!string.IsNullOrEmpty(key) && !components.ContainsKey(key))
                components.Add(key, numArray[componentArrayStructure.Pointer7.Index]);
            }
          }
          return components;
        }
        catch (Exception ex)
        {
          return (Dictionary<string, long>) null;
        }
      }
    }

    public bool HasComponent<T>() where T : Component, new()
    {
      long num;
      return this.CacheComp != null && this.CacheComp.TryGetValue(typeof (T).Name, out num) && num != 0L;
    }

    public T GetComponentOld<T>() where T : Component, new()
    {
      lock (this._cacheComponentsLock)
      {
        Component componentOld1;
        if (this._cacheComponents.TryGetValue(typeof (T), out componentOld1))
          return (T) componentOld1;
        long address;
        if (this.CacheComp == null || !this.CacheComp.TryGetValue(typeof (T).Name, out address))
          return default (T);
        T componentOld2 = this.GetObject<T>(address);
        this._cacheComponents[typeof (T)] = (Component) componentOld2;
        return componentOld2;
      }
    }

    public T GetComponent<T>() where T : Component, new()
    {
      lock (this._cacheComponentsLock)
      {
        Component component = (Component) null;
        long address;
        if (!this._cacheComponents.TryGetValue(typeof (T), out component) && this.CacheComp != null && this.CacheComp.TryGetValue(typeof (T).Name, out address))
          component = (Component) this.GetObject<T>(address);
        if (component != null && (component.OwnerAddress != this.Address || !(component is T)))
        {
          this._cacheComponents.Remove(typeof (T));
          return default (T);
        }
        this._cacheComponents[typeof (T)] = component;
        return (T) component;
      }
    }

    public bool TryGetComponent<T>(out T component) where T : Component, new()
    {
      component = this.GetComponent<T>();
      return (object) component != null;
    }

    public bool CheckComponentForValid<T>() where T : Component, new() => this.GetComponent<T>().OwnerAddress == this.Address || this.GetComponentFromMemory<T>().OwnerAddress == this.Address;

    public T GetComponentFromMemory<T>() where T : Component, new()
    {
      lock (this._cacheComponentsLock)
      {
        long address;
        if (!this.CacheComp.TryGetValue(typeof (T).Name, out address))
          return default (T);
        T componentFromMemory = this.GetObject<T>(address);
        this._cacheComponents[typeof (T)] = (Component) componentFromMemory;
        return componentFromMemory;
      }
    }

    private EntityType ParseType()
    {
      string metadata = this.Metadata;
      if (metadata == null || metadata.Length == 0)
        return EntityType.Error;
      if (metadata.StartsWith("Metadata/Effects/", StringComparison.Ordinal))
        return EntityType.Effect;
      if (metadata.StartsWith("Metadata/Monsters/Daemon/", StringComparison.Ordinal))
        return EntityType.Daemon;
      if (this.Version > 0U && this.Id > (uint) int.MaxValue)
        return EntityType.ServerObject;
      if (this.HasComponent<WorldItem>())
        return EntityType.WorldItem;
      if (this.HasComponent<Monster>())
      {
        if (metadata.StartsWith("Metadata/Monsters/LeagueHeist/", StringComparison.Ordinal))
          this.League = LeagueType.Heist;
        if (metadata.StartsWith("Metadata/Monsters/LegionLeague/", StringComparison.Ordinal))
          this.League = LeagueType.Legion;
        if (metadata.StartsWith("Metadata/Monsters/LeagueAffliction/", StringComparison.Ordinal))
          this.League = LeagueType.Delirium;
        return EntityType.Monster;
      }
      if (this.HasComponent<Chest>())
      {
        if (metadata.StartsWith("Metadata/Chests/DelveChests", StringComparison.Ordinal))
        {
          this.League = LeagueType.Delve;
          return EntityType.Chest;
        }
        if (metadata.StartsWith("Metadata/Chests/Incursion", StringComparison.Ordinal))
        {
          this.League = LeagueType.Incursion;
          return EntityType.Chest;
        }
        if (metadata.StartsWith("Metadata/Chests/Legion", StringComparison.Ordinal))
        {
          this.League = LeagueType.Legion;
          return EntityType.Chest;
        }
        if (!metadata.StartsWith("Metadata/Chests/LeagueHeist/HeistChest", StringComparison.Ordinal))
          return EntityType.Chest;
        this.League = LeagueType.Heist;
        return EntityType.Chest;
      }
      if (metadata.StartsWith("Metadata/NPC", StringComparison.Ordinal) && this.HasComponent<NPC>())
        return EntityType.Npc;
      if (this.HasComponent<Shrine>())
        return EntityType.Shrine;
      if (this.HasComponent<ExileCore.PoEMemory.Components.Player>())
        return EntityType.Player;
      if (metadata.StartsWith("Metadata/MiscellaneousObjects/Harvest", StringComparison.Ordinal) || metadata.StartsWith("Metadata/Terrain/Leagues/Harvest", StringComparison.Ordinal))
      {
        this.League = LeagueType.Harvest;
        return EntityType.MiscellaneousObjects;
      }
      if (this.HasComponent<MinimapIcon>())
      {
        if (metadata.Equals("Metadata/Terrain/Missions/Hideouts/Objects/HideoutCraftingBench", StringComparison.Ordinal))
          return EntityType.CraftUnlock;
        if (this.HasComponent<AreaTransition>())
          return EntityType.AreaTransition;
        if (metadata.EndsWith("Waypoint", StringComparison.Ordinal))
          return EntityType.Waypoint;
        if (this.HasComponent<Portal>())
          return EntityType.TownPortal;
        if (this.HasComponent<Monolith>())
          return EntityType.Monolith;
        if (this.HasComponent<Transitionable>() && metadata.StartsWith("Metadata/MiscellaneousObjects/Abyss"))
        {
          this.League = LeagueType.Abyss;
          return EntityType.MiscellaneousObjects;
        }
        if (metadata.Equals("Metadata/Terrain/Leagues/Legion/Objects/LegionInitiator", StringComparison.Ordinal))
          return EntityType.LegionMonolith;
        if (metadata.Equals("Metadata/MiscellaneousObjects/Stash", StringComparison.Ordinal))
          return EntityType.Stash;
        if (metadata.Equals("Metadata/MiscellaneousObjects/GuildStash", StringComparison.Ordinal))
          return EntityType.GuildStash;
        if (metadata.Equals("Metadata/MiscellaneousObjects/Delve/DelveCraftingBench", StringComparison.Ordinal))
          return EntityType.DelveCraftingBench;
        if (metadata.Equals("Metadata/MiscellaneousObjects/Breach/BreachObject", StringComparison.Ordinal))
          return EntityType.Breach;
        return metadata.Equals("Metadata/Terrain/Leagues/Delve/Objects/DelveMineral") ? EntityType.Resource : EntityType.IngameIcon;
      }
      if (this.HasComponent<Portal>())
        return EntityType.Portal;
      if (this.HasComponent<HideoutDoodad>())
        return EntityType.HideoutDecoration;
      if (this.HasComponent<Monolith>())
        return EntityType.MiniMonolith;
      if (this.HasComponent<ClientBetrayalChoice>())
        return EntityType.BetrayalChoice;
      if (this.HasComponent<RenderItem>())
        return EntityType.Item;
      if (metadata.StartsWith("Metadata/MiscellaneousObjects/Lights", StringComparison.Ordinal))
        return EntityType.Light;
      if (metadata.StartsWith("Metadata/Terrain/Labyrinth/Objects/Puzzle_Parts/Switch_Once", StringComparison.Ordinal))
        return EntityType.DoorSwitch;
      if (metadata.StartsWith("Metadata/Terrain", StringComparison.Ordinal))
        return EntityType.Terrain;
      if (metadata.StartsWith("Metadata/Pet", StringComparison.Ordinal))
        return EntityType.Pet;
      if (metadata.StartsWith("Metadata/MiscellaneousObjects/Door", StringComparison.Ordinal))
        return EntityType.Door;
      if (metadata.StartsWith("Metadata/MiscellaneousObjects", StringComparison.Ordinal))
        return EntityType.MiscellaneousObjects;
      return this.HasComponent<TriggerableBlockage>() && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssNodeSmall", StringComparison.OrdinalIgnoreCase) && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssFinalNodeChest", StringComparison.OrdinalIgnoreCase) && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssFinalNodeChest2", StringComparison.OrdinalIgnoreCase) && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssFinalNodeChest3", StringComparison.OrdinalIgnoreCase) && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssFinalNodeChest4", StringComparison.OrdinalIgnoreCase) && !metadata.Contains("AbyssFinalNodeSubArea", StringComparison.OrdinalIgnoreCase) && !metadata.Equals("Metadata/MiscellaneousObjects/Abyss/AbyssNodeLarge", StringComparison.OrdinalIgnoreCase) ? EntityType.TriggerableBlockage : EntityType.None;
    }

    public T GetHudComponent<T>() where T : class
    {
      object obj;
      return this.PluginData.TryGetValue(typeof (T), out obj) ? (T) obj : default (T);
    }

    public void SetHudComponent<T>(T data)
    {
      lock (this.locker)
        this.PluginData[typeof (T)] = (object) data;
    }
  }
}
