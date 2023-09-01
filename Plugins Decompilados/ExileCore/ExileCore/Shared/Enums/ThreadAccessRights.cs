// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.ThreadAccessRights
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum ThreadAccessRights
  {
    DELETE = 65536, // 0x00010000
    READ_CONTROL = 131072, // 0x00020000
    WRITE_DAC = 262144, // 0x00040000
    WRITE_OWNER = 524288, // 0x00080000
    SYNCHRONIZE = 1048576, // 0x00100000
    STANDARD_RIGHTS_ALL = WRITE_OWNER | WRITE_DAC | READ_CONTROL | DELETE, // 0x000F0000
    THREAD_TERMINATE = 1,
    THREAD_SUSPEND_RESUME = 2,
    THREAD_GET_CONTEXT = 8,
    THREAD_SET_CONTEXT = 16, // 0x00000010
    THREAD_SET_INFORMATION = 32, // 0x00000020
    THREAD_QUERY_INFORMATION = 64, // 0x00000040
    THREAD_SET_THREAD_TOKEN = 128, // 0x00000080
    THREAD_IMPERSONATE = 256, // 0x00000100
    THREAD_DIRECT_IMPERSONATION = 512, // 0x00000200
    THREAD_ALL_ACCESS = 2032639, // 0x001F03FF
  }
}
