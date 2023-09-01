// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.Utilities.POINT
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using SharpDX;

namespace FullRareSetManager.Utilities
{
  public struct POINT
  {
    public int X;
    public int Y;

    public static implicit operator Point(POINT point) => new Point(point.X, point.Y);
  }
}
