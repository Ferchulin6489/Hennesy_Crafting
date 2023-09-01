// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.MemoryProtectionType
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.Shared.Enums
{
  public enum MemoryProtectionType
  {
    PAGE_NOACCESS = 1,
    PAGE_READONLY = 2,
    PAGE_READWRITE = 4,
    PAGE_WRITECOPY = 8,
    PAGE_EXECUTE = 16, // 0x00000010
    PAGE_EXECUTE_READ = 32, // 0x00000020
    PAGE_EXECUTE_READWRITE = 64, // 0x00000040
    PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080
    PAGE_GUARD = 256, // 0x00000100
    PAGE_NOCACHE = 512, // 0x00000200
    PAGE_WRITECOMBINE = 1024, // 0x00000400
  }
}
