// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ServerInventory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using GameOffsets.Native;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ServerInventory : RemoteMemoryObject
  {
    private readonly CachedValue<ServerInventoryOffsets> cachedValue;
    private const int HashReadLimit = 500;

    public ServerInventory() => this.cachedValue = (CachedValue<ServerInventoryOffsets>) new FrameCache<ServerInventoryOffsets>((Func<ServerInventoryOffsets>) (() => this.M.Read<ServerInventoryOffsets>(this.Address)));

    private ServerInventoryOffsets Struct => this.cachedValue.Value;

    public InventoryTypeE InventType => (InventoryTypeE) this.Struct.InventType;

    public InventorySlotE InventSlot => (InventorySlotE) this.Struct.InventSlot;

    public int Columns => this.Struct.Columns;

    public int Rows => this.Struct.Rows;

    public bool IsRequested => this.Struct.IsRequested == (byte) 1;

    public long ItemCount => this.Struct.ItemCount;

    [Obsolete]
    public long CountItems => this.ItemCount;

    [Obsolete]
    public int TotalItemsCounts => (int) this.ItemCount;

    public int ServerRequestCounter => this.Struct.ServerRequestCounter;

    public IList<ServerInventory.InventSlotItem> InventorySlotItems => (IList<ServerInventory.InventSlotItem>) this.ReadHashMap(this.Struct.InventorySlotItemsPtr, 500).Values.ToList<ServerInventory.InventSlotItem>();

    public long Hash => this.Struct.Hash;

    public IList<Entity> Items => (IList<Entity>) this.InventorySlotItems.Select<ServerInventory.InventSlotItem, Entity>((Func<ServerInventory.InventSlotItem, Entity>) (x => x.Item)).ToList<Entity>();

    public ServerInventory.InventSlotItem this[int x, int y]
    {
      get
      {
        long address = this.M.Read<long>(this.Struct.InventoryItemsPtr + (long) ((x + y * this.Columns) * 8));
        return address <= 0L ? (ServerInventory.InventSlotItem) null : this.GetObject<ServerInventory.InventSlotItem>(address);
      }
    }

    public Dictionary<int, ServerInventory.InventSlotItem> ReadHashMap(long pointer, int limitMax)
    {
      Dictionary<int, ServerInventory.InventSlotItem> dictionary = new Dictionary<int, ServerInventory.InventSlotItem>();
      Stack<ServerInventory.HashNode> hashNodeStack = new Stack<ServerInventory.HashNode>();
      ServerInventory.HashNode root = this.GetObject<ServerInventory.HashNode>(pointer).Root;
      hashNodeStack.Push(root);
      while (hashNodeStack.Count != 0)
      {
        ServerInventory.HashNode hashNode = hashNodeStack.Pop();
        if (!hashNode.IsNull)
          dictionary[hashNode.Key] = hashNode.Value1;
        ServerInventory.HashNode previous = hashNode.Previous;
        if (!previous.IsNull)
          hashNodeStack.Push(previous);
        ServerInventory.HashNode next = hashNode.Next;
        if (!next.IsNull)
          hashNodeStack.Push(next);
        if (limitMax-- < 0)
        {
          DebugWindow.LogError("Fixed possible memory leak (ServerInventory.ReadHashMap)");
          break;
        }
      }
      return dictionary;
    }

    public class HashNode : RemoteMemoryObject
    {
      private readonly FrameCache<NativeHashNode> frameCache;

      public HashNode() => this.frameCache = new FrameCache<NativeHashNode>((Func<NativeHashNode>) (() => this.M.Read<NativeHashNode>(this.Address)));

      private NativeHashNode NativeHashNode => this.frameCache.Value;

      public ServerInventory.HashNode Previous => this.GetObject<ServerInventory.HashNode>(this.NativeHashNode.Previous);

      public ServerInventory.HashNode Root => this.GetObject<ServerInventory.HashNode>(this.NativeHashNode.Root);

      public ServerInventory.HashNode Next => this.GetObject<ServerInventory.HashNode>(this.NativeHashNode.Next);

      public bool IsNull => this.NativeHashNode.IsNull > (byte) 0;

      public int Key => this.NativeHashNode.Key;

      public ServerInventory.InventSlotItem Value1 => this.GetObject<ServerInventory.InventSlotItem>(this.NativeHashNode.Value1);
    }

    public class InventSlotItem : RemoteMemoryObject
    {
      [Obsolete]
      public SharpDX.Vector2 InventoryPosition => this.Location.InventoryPosition;

      public System.Numerics.Vector2 InventoryPositionNum => this.Location.InventoryPositionNum;

      private ServerInventory.InventSlotItem.ItemMinMaxLocation Location => this.M.Read<ServerInventory.InventSlotItem.ItemMinMaxLocation>(this.Address + 8L);

      public Entity Item => this.ReadObject<Entity>(this.Address);

      public int PosX => this.M.Read<int>(this.Address + 8L);

      public int PosY => this.M.Read<int>(this.Address + 12L);

      public int SizeX => this.M.Read<int>(this.Address + 16L) - this.PosX;

      public int SizeY => this.M.Read<int>(this.Address + 20L) - this.PosY;

      private RectangleF ClientRect => this.GetClientRect();

      public RectangleF GetClientRect()
      {
        RectangleF clientRect = this.TheGame.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].GetClientRect();
        float cellsize = clientRect.Width / 12f;
        return this.Location.GetItemRect(clientRect.X, clientRect.Y, cellsize);
      }

      public override string ToString()
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
        interpolatedStringHandler.AppendLiteral("InventSlotItem: ");
        interpolatedStringHandler.AppendFormatted<ServerInventory.InventSlotItem.ItemMinMaxLocation>(this.Location);
        interpolatedStringHandler.AppendLiteral(", Item: ");
        interpolatedStringHandler.AppendFormatted<Entity>(this.Item);
        return interpolatedStringHandler.ToStringAndClear();
      }

      private struct ItemMinMaxLocation
      {
        private int XMin { get; set; }

        private int YMin { get; set; }

        private int XMax { get; set; }

        private int YMax { get; set; }

        public RectangleF GetItemRect(float invStartX, float invStartY, float cellsize) => new RectangleF(invStartX + cellsize * (float) this.XMin, invStartY + cellsize * (float) this.YMin, (float) (this.XMax - this.XMin) * cellsize, (float) (this.YMax - this.YMin) * cellsize);

        [Obsolete]
        public SharpDX.Vector2 InventoryPosition => new SharpDX.Vector2((float) this.XMin, (float) this.YMin);

        public System.Numerics.Vector2 InventoryPositionNum => new System.Numerics.Vector2((float) this.XMin, (float) this.YMin);

        public override string ToString()
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 4);
          interpolatedStringHandler.AppendLiteral("(");
          interpolatedStringHandler.AppendFormatted<int>(this.XMin);
          interpolatedStringHandler.AppendLiteral(", ");
          interpolatedStringHandler.AppendFormatted<int>(this.YMin);
          interpolatedStringHandler.AppendLiteral(", ");
          interpolatedStringHandler.AppendFormatted<int>(this.XMax);
          interpolatedStringHandler.AppendLiteral(", ");
          interpolatedStringHandler.AppendFormatted<int>(this.YMax);
          interpolatedStringHandler.AppendLiteral(")");
          return interpolatedStringHandler.ToStringAndClear();
        }
      }
    }
  }
}
