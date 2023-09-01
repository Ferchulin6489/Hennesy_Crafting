// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BaseItemTypes
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BaseItemTypes : FileInMemory
  {
    public BaseItemTypes(IMemory m, Func<long> address)
      : base(m, address)
    {
      this.LoadItemTypes();
    }

    public Dictionary<string, BaseItemType> Contents { get; } = new Dictionary<string, BaseItemType>();

    public Dictionary<long, BaseItemType> ContentsAddr { get; } = new Dictionary<long, BaseItemType>();

    public BaseItemType GetFromAddress(long address)
    {
      BaseItemType fromAddress;
      this.ContentsAddr.TryGetValue(address, out fromAddress);
      return fromAddress;
    }

    public BaseItemType Translate(string metadata)
    {
      if (this.Contents.Count == 0)
        this.LoadItemTypes();
      if (metadata == null)
        return (BaseItemType) null;
      BaseItemType baseItemType;
      if (this.Contents.TryGetValue(metadata, out baseItemType))
        return baseItemType;
      Console.WriteLine("Key not found in BaseItemTypes: " + metadata);
      return (BaseItemType) null;
    }

    private void LoadItemTypes()
    {
      foreach (long recordAddress in this.RecordAddresses())
      {
        string key = this.M.ReadStringU(this.M.Read<long>(recordAddress));
        BaseItemType baseItemType = new BaseItemType()
        {
          Metadata = key,
          ClassName = this.M.ReadStringU(this.M.Read<long>(recordAddress + 8L, new int[1])),
          Width = this.M.Read<int>(recordAddress + 24L),
          Height = this.M.Read<int>(recordAddress + 28L),
          BaseName = this.M.ReadStringU(this.M.Read<long>(recordAddress + 32L)),
          DropLevel = this.M.Read<int>(recordAddress + 48L),
          Tags = new string[this.M.Read<long>(recordAddress + 104L)]
        };
        long num = this.M.Read<long>(recordAddress + 112L);
        for (int index = 0; index < baseItemType.Tags.Length; ++index)
        {
          long addr = num + (long) (16 * index);
          baseItemType.Tags[index] = this.M.ReadStringU(this.M.Read<long>(addr, new int[1]), (int) byte.MaxValue);
        }
        string[] strArray = key.Split('/');
        if (strArray.Length > 3)
        {
          baseItemType.MoreTagsFromPath = new string[strArray.Length - 3];
          for (int index = 2; index < strArray.Length - 1; ++index)
          {
            string str = Regex.Replace(strArray[index], "(?<!_)([A-Z])", "_$1").ToLower().Remove(0, 1);
            if (str[str.Length - 1] == 's')
              str = str.Remove(str.Length - 1);
            baseItemType.MoreTagsFromPath[index - 2] = str;
          }
        }
        else
        {
          baseItemType.MoreTagsFromPath = new string[1];
          baseItemType.MoreTagsFromPath[0] = "";
        }
        this.ContentsAddr.Add(recordAddress, baseItemType);
        if (!this.Contents.ContainsKey(key))
          this.Contents.Add(key, baseItemType);
      }
    }
  }
}
