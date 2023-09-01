// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.MemoryAllocationState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.Shared.Enums
{
  public enum MemoryAllocationState
  {
    MEM_COMMIT = 4096, // 0x00001000
    MEM_RESERVE = 8192, // 0x00002000
    MEM_RESET = 524288, // 0x00080000
    MEM_TOP_DOWN = 1048576, // 0x00100000
    MEM_PHYSICAL = 4194304, // 0x00400000
    MEM_LARGE_PAGES = 536870912, // 0x20000000
  }
}
