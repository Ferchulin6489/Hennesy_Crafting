// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionDetonatorInfo
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.Elements
{
  public class ExpeditionDetonatorInfo : StructuredRemoteMemoryObject<ExpeditionDetonatorInfoOffsets>
  {
    public bool IsExplosivePlacementActive => this.Structure.PlacementMarkerPtr != 0L;

    public Vector2i[] PlacedExplosiveGridPositions => this.M.ReadStdVector<Vector2i>(this.Structure.PlacedExplosives);

    public int TotalExplosiveCount => (int) this.Structure.TotalExplosiveCount;

    public int PlacedExplosiveCount => (int) this.Structure.PlacedExplosives.ElementCount<Vector2i>();

    public int RemainingExplosiveCount => this.TotalExplosiveCount - this.PlacedExplosiveCount;

    public Vector2i DetonatorGridPosition => this.Structure.DetonatorGridPosition;

    public Vector2i PlacementIndicatorGridPosition => this.Structure.PlacementIndicatorGridPosition;
  }
}
