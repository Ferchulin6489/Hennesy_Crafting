// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.ObjectMagicProperties
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class ObjectMagicProperties : Component
  {
    private readonly CachedValue<ObjectMagicPropertiesOffsets> _CachedValue;
    private long _ModsHash;
    private readonly List<string> _ModNamesList = new List<string>();
    private const int MOD_RECORDS_OFFSET = 24;
    private const int MOD_RECORD_SIZE = 56;
    private const int MOD_RECORD_KEY_OFFSET = 16;

    public ObjectMagicProperties() => this._CachedValue = (CachedValue<ObjectMagicPropertiesOffsets>) new FrameCache<ObjectMagicPropertiesOffsets>((Func<ObjectMagicPropertiesOffsets>) (() => this.M.Read<ObjectMagicPropertiesOffsets>(this.Address)));

    public ObjectMagicPropertiesOffsets ObjectMagicPropertiesOffsets => this._CachedValue.Value;

    public MonsterRarity Rarity => this.Address != 0L ? (MonsterRarity) this.ObjectMagicPropertiesOffsets.Rarity : MonsterRarity.Error;

    public long ModsHash => (long) this.ObjectMagicPropertiesOffsets.Mods.GetHashCode();

    public List<string> Mods
    {
      get
      {
        if (this.Address == 0L)
          return (List<string>) null;
        if (this._ModsHash == this.ModsHash)
          return this._ModNamesList;
        long first = this.ObjectMagicPropertiesOffsets.Mods.First;
        long last = this.ObjectMagicPropertiesOffsets.Mods.Last;
        long val2 = this.ObjectMagicPropertiesOffsets.Mods.First + 14336L;
        if (first == 0L || last == 0L || last < first)
          return new List<string>();
        long num = Math.Min(last, val2);
        for (long index = first + 24L; index < num; index += 56L)
        {
          long read = this.M.Read<long>(index + 16L, new int[1]);
          IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ObjectMagicProperties));
          interpolatedStringHandler.AppendFormatted<long>(read);
          string stringAndClear = interpolatedStringHandler.ToStringAndClear();
          Func<string> func = (Func<string>) (() => this.M.ReadStringU(read));
          this._ModNamesList.Add(stringCache.Read(stringAndClear, func));
        }
        if (first == val2)
          DebugWindow.LogMsg("ObjectMagicProperties read mods error address", 2f, Color.OrangeRed);
        this._ModsHash = this.ModsHash;
        return this._ModNamesList;
      }
    }
  }
}
