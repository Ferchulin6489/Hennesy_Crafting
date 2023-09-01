// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Pathfinding
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class Pathfinding : Component
  {
    private readonly CachedValue<PathfindingComponentOffsets> _cachedValue;

    public Pathfinding() => this._cachedValue = (CachedValue<PathfindingComponentOffsets>) new FrameCache<PathfindingComponentOffsets>((Func<PathfindingComponentOffsets>) (() => this.M.Read<PathfindingComponentOffsets>(this.Address)));

    private PathfindingComponentOffsets Offsets => this._cachedValue.Value;

    public Vector2i TargetMovePos => new Vector2i();

    public Vector2i PreviousMovePos => new Vector2i();

    public Vector2i WantMoveToPosition => this.Offsets.WantMoveToPosition;

    public bool IsMoving => this.Offsets.DestinationNodes > 0;

    public int DestinationNodes => this.Offsets.DestinationNodes;

    public float StayTime => this.Offsets.StayTime;

    public IList<Vector2i> PathingNodes
    {
      get
      {
        if (this.Address == 0L)
          return (IList<Vector2i>) null;
        int destinationNodes = this.Offsets.DestinationNodes;
        if (destinationNodes < 0 || destinationNodes > 30)
          return (IList<Vector2i>) new List<Vector2i>();
        int size = destinationNodes * 4 * 2;
        byte[] numArray = this.M.ReadMem(this.Address + (long) PathfindingComponentOffsets.PathNodeStart, size);
        List<Vector2i> pathingNodes = new List<Vector2i>();
        for (int startIndex = 0; startIndex < size; startIndex += 8)
        {
          int int32_1 = BitConverter.ToInt32(numArray, startIndex);
          int int32_2 = BitConverter.ToInt32(numArray, startIndex + 4);
          pathingNodes.Add(new Vector2i(int32_1, int32_2));
        }
        pathingNodes.Reverse();
        return (IList<Vector2i>) pathingNodes;
      }
    }
  }
}
