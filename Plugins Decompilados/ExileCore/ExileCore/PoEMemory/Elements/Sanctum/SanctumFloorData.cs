// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.Sanctum.SanctumFloorData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements.Sanctum
{
  public class SanctumFloorData : RemoteMemoryObject
  {
    public NativePtrArray RoomDataArray => this.M.Read<NativePtrArray>(this.Address + 24L);

    public List<SanctumRoomData> RoomData => this.M.ReadStructsArray<SanctumRoomData>(this.RoomDataArray.First, this.RoomDataArray.Last, 112, (RemoteMemoryObject) null);

    public byte[][][] RoomLayout => ((IEnumerable<NativePtrArray>) this.M.ReadStdVectorStride<NativePtrArray>(this.M.Read<NativePtrArray>(this.Address), 32)).Select<NativePtrArray, byte[][]>((Func<NativePtrArray, byte[][]>) (x => ((IEnumerable<NativePtrArray>) this.M.ReadStdVectorStride<NativePtrArray>(x, 56)).Select<NativePtrArray, byte[]>((Func<NativePtrArray, byte[]>) (y => this.M.ReadStdVector<byte>(y))).ToArray<byte[]>())).ToArray<byte[][]>();
  }
}
