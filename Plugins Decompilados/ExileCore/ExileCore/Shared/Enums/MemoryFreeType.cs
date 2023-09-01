// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.MemoryFreeType
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum MemoryFreeType
  {
    MEM_DECOMMIT = 16384, // 0x00004000
    MEM_RELEASE = 32768, // 0x00008000
  }
}
