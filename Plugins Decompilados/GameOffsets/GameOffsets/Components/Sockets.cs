// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.Sockets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct Sockets
  {
    public ComponentHeader Header;
    public long UselessPtr;
    public SocketColors Colors;
    public SocketGems Gems;
    public NativePtrArray ListsPtr;
  }
}
