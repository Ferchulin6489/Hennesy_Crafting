// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ItemMod
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ItemMod : RemoteMemoryObject
  {
    private static readonly char[] Digits = "0123456789".ToCharArray();
    public static readonly int STRUCT_SIZE = 56;
    private string _rawName;
    private ModsDat.ModRecord _record;

    [Obsolete("Use Values instead")]
    public int Value1 => this.Values.Count <= 0 ? 0 : this.Values[0];

    [Obsolete("Use Values instead")]
    public int Value2 => this.Values.Count <= 1 ? 0 : this.Values[1];

    [Obsolete("Use Values instead")]
    public int Value3 => this.Values.Count <= 2 ? 0 : this.Values[2];

    [Obsolete("Use Values instead")]
    public int Value4 => this.Values.Count <= 3 ? 0 : this.Values[3];

    public List<int> Values
    {
      get
      {
        long startAddress = this.M.Read<long>(this.Address);
        long endAddress = this.M.Read<long>(this.Address + 8L);
        long num = (endAddress - startAddress) / 8L;
        return num < 0L || num > 10L ? new List<int>() : this.M.ReadStructsArray<int>(startAddress, endAddress, 4);
      }
    }

    public IntRange[] ValuesMinMax
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        return this._record?.StatRange;
      }
    }

    public string RawName
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        return this._rawName ?? string.Empty;
      }
    }

    public string Name
    {
      get
      {
        string rawName = this.RawName;
        int length = rawName.IndexOfAny(ItemMod.Digits);
        return (length == -1 ? rawName : rawName.Substring(0, length)).Replace("_", "");
      }
    }

    public string Group
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        return this._record?.Group ?? string.Empty;
      }
    }

    public string DisplayName
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        return this._record?.UserFriendlyName ?? string.Empty;
      }
    }

    public int Level
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        ModsDat.ModRecord record = this._record;
        return record == null ? 0 : record.MinLevel;
      }
    }

    public ModsDat.ModRecord ModRecord
    {
      get
      {
        if (this._record == null)
          this.ReadRecord();
        return this._record;
      }
    }

    private void ReadRecord()
    {
      this._record = this.TheGame.Files.Mods.GetModByAddress(this.M.Read<long>(this.Address + 40L));
      this._rawName = this._record?.Key;
    }

    public override string ToString() => this._rawName + " (" + string.Join(", ", this.Values.Take<int>(Math.Min(this.Values.Count, this.ValuesMinMax.Length)).Select<int, string>((Func<int, int, string>) ((x, i) =>
    {
      IntRange intRange = this.ValuesMinMax[i];
      if (intRange.Min == intRange.Max)
        return x.ToString();
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
      interpolatedStringHandler.AppendFormatted<int>(x);
      interpolatedStringHandler.AppendLiteral(" [");
      interpolatedStringHandler.AppendFormatted<int>(intRange.Min);
      interpolatedStringHandler.AppendLiteral("-");
      interpolatedStringHandler.AppendFormatted<int>(intRange.Max);
      interpolatedStringHandler.AppendLiteral("]");
      return interpolatedStringHandler.ToStringAndClear();
    }))) + ")";
  }
}
