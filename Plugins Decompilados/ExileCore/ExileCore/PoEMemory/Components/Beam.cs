// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Beam
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.PoEMemory.Components
{
  public class Beam : Component
  {
    private const int BeamStartOffset = 80;
    private const int BeamEndOffset = 92;

    [Obsolete]
    public SharpDX.Vector3 BeamStart => this.M.Read<SharpDX.Vector3>(this.Address + 80L);

    [Obsolete]
    public SharpDX.Vector3 BeamEnd => this.M.Read<SharpDX.Vector3>(this.Address + 92L);

    public System.Numerics.Vector3 BeamStartNum => this.M.Read<System.Numerics.Vector3>(this.Address + 80L);

    public System.Numerics.Vector3 BeamEndNum => this.M.Read<System.Numerics.Vector3>(this.Address + 92L);

    public int Unknown1 => this.M.Read<int>(this.Address + 64L);

    public int Unknown2 => this.M.Read<int>(this.Address + 68L);
  }
}
