// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.ProcessAccessRights
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum ProcessAccessRights
  {
    DELETE = 65536, // 0x00010000
    READ_CONTROL = 131072, // 0x00020000
    WRITE_DAC = 262144, // 0x00040000
    WRITE_OWNER = 524288, // 0x00080000
    SYNCHRONIZE = 1048576, // 0x00100000
    STANDARD_RIGHTS_ALL = WRITE_OWNER | WRITE_DAC | READ_CONTROL | DELETE, // 0x000F0000
    PROCESS_TERMINATE = 1,
    PROCESS_CREATE_THREAD = 2,
    PROCESS_VM_OPERATION = 8,
    PROCESS_VM_READ = 16, // 0x00000010
    PROCESS_VM_WRITE = 32, // 0x00000020
    PROCESS_DUP_HANDLE = 64, // 0x00000040
    PROCESS_CREATE_PROCESS = 128, // 0x00000080
    PROCESS_SET_QUOTA = 256, // 0x00000100
    PROCESS_SET_INFORMATION = 512, // 0x00000200
    PROCESS_QUERY_INFORMATION = 1024, // 0x00000400
    PROCESS_SUSPEND_RESUME = 2048, // 0x00000800
    PROCESS_QUERY_LIMITED_INFORMATION = 4096, // 0x00001000
    PROCESS_ALL_ACCESS = 2035711, // 0x001F0FFF
  }
}
