// Decompiled with JetBrains decompiler
// Type: GameOffsets.StashElementOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct StashElementOffsets
  {
    public const int StashTabContainerOffset2 = 632;
    [FieldOffset(712)]
    public long StashTitlePanelPtr;
    [FieldOffset(720)]
    public long ExitButtonPtr;
    [FieldOffset(752)]
    public long StashTabContainerPtr1;
  }
}
