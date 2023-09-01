// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Movement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using System.Numerics;

namespace ExileCore.PoEMemory.Components
{
  public class Movement : Component
  {
    public Vector2 MovingToGridPosNum => this.Address == 0L ? new Vector2(0.0f, 0.0f) : this.M.Read<Vector2i>(this.Address + 60L).ToVector2Num();
  }
}
