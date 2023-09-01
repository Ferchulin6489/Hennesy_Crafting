// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Sockets
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExileCore.PoEMemory.Components
{
  public class Sockets : Component
  {
    private readonly CachedValue<SocketsComponentOffsets> _cachedValue;

    public Sockets() => this._cachedValue = (CachedValue<SocketsComponentOffsets>) this.CreateStructFrameCache<SocketsComponentOffsets>();

    public int LargestLinkSize
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        StdVector linkSizes = this._cachedValue.Value.LinkSizes;
        return linkSizes.ElementCount<byte>() > 6L ? 0 : (int) ((IEnumerable<byte>) this.M.ReadStdVector<byte>(linkSizes)).Max<byte>();
      }
    }

    public List<int[]> Links
    {
      get
      {
        if (this.Address == 0L)
          return new List<int[]>();
        StdVector linkSizes = this._cachedValue.Value.LinkSizes;
        if (linkSizes.ElementCount<byte>() > 6L)
          return new List<int[]>();
        byte[] numArray = this.M.ReadStdVector<byte>(linkSizes);
        List<int> socketList = this.SocketList;
        List<int[]> links = new List<int[]>();
        int start = 0;
        foreach (int num in numArray)
        {
          int end = start + num;
          if (end > socketList.Count)
            return links;
          int[] array = socketList.Take<int>(new Range((Index) start, (Index) end)).ToArray<int>();
          links.Add(array);
          start = end;
        }
        return links;
      }
    }

    public List<int> SocketList
    {
      get
      {
        SocketColorList sockets = this._cachedValue.Value.Sockets;
        return ((IEnumerable<int>) new int[6]
        {
          sockets.Socket1Color,
          sockets.Socket2Color,
          sockets.Socket3Color,
          sockets.Socket4Color,
          sockets.Socket5Color,
          sockets.Socket6Color
        }).Where<int>((Func<int, bool>) (x => x >= 1 && x <= 6)).ToList<int>();
      }
    }

    public int NumberOfSockets => this.SocketList.Count;

    public bool IsRGB => this.Address != 0L && this.Links.Any<int[]>((Func<int[], bool>) (current => current.Length >= 3 && ((IEnumerable<int>) current).Contains<int>(1) && ((IEnumerable<int>) current).Contains<int>(2) && ((IEnumerable<int>) current).Contains<int>(3)));

    public List<string> SocketGroup
    {
      get
      {
        List<string> socketGroup = new List<string>();
        foreach (int[] link in this.Links)
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (int num in link)
          {
            switch (num)
            {
              case 1:
                stringBuilder.Append("R");
                break;
              case 2:
                stringBuilder.Append("G");
                break;
              case 3:
                stringBuilder.Append("B");
                break;
              case 4:
                stringBuilder.Append("W");
                break;
              case 5:
                stringBuilder.Append('A');
                break;
              case 6:
                stringBuilder.Append("O");
                break;
            }
          }
          socketGroup.Add(stringBuilder.ToString());
        }
        return socketGroup;
      }
    }

    public List<Sockets.SocketedGem> SocketedGems
    {
      get
      {
        SocketedGemList socketedGems = this._cachedValue.Value.SocketedGems;
        return ((IEnumerable<long>) new long[6]
        {
          socketedGems.Socket1GemPtr,
          socketedGems.Socket2GemPtr,
          socketedGems.Socket3GemPtr,
          socketedGems.Socket4GemPtr,
          socketedGems.Socket5GemPtr,
          socketedGems.Socket6GemPtr
        }).Select<long, (long, int)>((Func<long, int, (long, int)>) ((pointer, index) => (pointer, index))).Where<(long, int)>((Func<(long, int), bool>) (x => x.pointer != 0L)).Select<(long, int), Sockets.SocketedGem>((Func<(long, int), Sockets.SocketedGem>) (x => new Sockets.SocketedGem()
        {
          SocketIndex = x.index,
          GemEntity = this.GetObject<Entity>(x.pointer)
        })).ToList<Sockets.SocketedGem>();
      }
    }

    public class SocketedGem
    {
      public Entity GemEntity;
      public int SocketIndex;
    }
  }
}
