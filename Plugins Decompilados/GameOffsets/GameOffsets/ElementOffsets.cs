// Decompiled with JetBrains decompiler
// Type: GameOffsets.ElementOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ElementOffsets
  {
    public const int OffsetBuffers = 0;
    [FieldOffset(40)]
    public long SelfPointer;
    [FieldOffset(48)]
    public long ChildStart;
    [FieldOffset(48)]
    public NativePtrArray Childs;
    [FieldOffset(56)]
    public long ChildEnd;
    [FieldOffset(168)]
    public Vector2 ScrollOffset;
    [FieldOffset(208)]
    public ushort Type;
    [FieldOffset(216)]
    public long Root;
    [FieldOffset(224)]
    public long Parent;
    [FieldOffset(232)]
    public Vector2 Position;
    [FieldOffset(264)]
    public long Tooltip;
    [FieldOffset(285)]
    public long IsSelected;
    [FieldOffset(440)]
    public bool isHighlighted;
    [FieldOffset(344)]
    public float Scale;
    [FieldOffset(352)]
    public ElementFlags Flags;
    [FieldOffset(384)]
    public Vector2 Size;
    [FieldOffset(744)]
    public NativeUtf16Text Text;
    [FieldOffset(1016)]
    public NativeUtf16Text TextNoTags;
  }
}
