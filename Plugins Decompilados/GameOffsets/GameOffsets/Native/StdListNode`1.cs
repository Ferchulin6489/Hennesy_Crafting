﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.StdListNode`1
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct StdListNode<TValue> where TValue : struct
  {
    public IntPtr Next;
    public IntPtr Previous;
    public TValue Data;
  }
}