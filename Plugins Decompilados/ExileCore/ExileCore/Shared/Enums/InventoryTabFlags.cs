// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.InventoryTabFlags
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum InventoryTabFlags : byte
  {
    Hidden = 128, // 0x80
    MapSeries = 64, // 0x40
    Premium = 4,
    Public = 32, // 0x20
    RemoveOnly = 1,
    Unknown1 = 16, // 0x10
    Unknown2 = 2,
    Unknown3 = 8,
  }
}
