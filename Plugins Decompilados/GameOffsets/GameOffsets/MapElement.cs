// Decompiled with JetBrains decompiler
// Type: GameOffsets.MapElement
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct MapElement
  {
    public const int LargeMapOffset = 640;
    public const int SmallMapOffset = 648;
    public const int MapPropertiesOffset = 672;
    public const int OrangeWordsOffset = 680;
    public const int BlueWordsOffset = 848;
  }
}
