// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesFromMemory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using MoreLinq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExileCore.PoEMemory
{
  public class FilesFromMemory
  {
    private readonly IMemory mem;

    public FilesFromMemory(IMemory memory) => this.mem = memory;

    public Dictionary<string, FileInformation> GetAllFiles()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      Dictionary<string, FileInformation> allFilesSync = this.GetAllFilesSync();
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
      interpolatedStringHandler.AppendLiteral("GetAllFiles loaded in ");
      interpolatedStringHandler.AppendFormatted<long>(stopwatch.ElapsedMilliseconds);
      interpolatedStringHandler.AppendLiteral("ms");
      DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 5f);
      return allFilesSync;
    }

    public Dictionary<string, FileInformation> GetAllFilesSync()
    {
      ConcurrentDictionary<string, FileInformation> files = new ConcurrentDictionary<string, FileInformation>();
      byte[] arrBytes = this.mem.ReadBytes(this.mem.AddressOfProcess + this.mem.BaseOffsets[OffsetsName.FileRoot], 640);
      Parallel.For(0, 16, (Action<int>) (i =>
      {
        byte[] numArray = this.mem.ReadBytes(BitConverter.ToInt64(arrBytes, i * 40 + 8), 102400);
        for (int index1 = 0; index1 < 512; ++index1)
        {
          int num1 = index1 * 200;
          for (int index2 = 0; index2 < 8; ++index2)
          {
            if (numArray[num1 + index2] != byte.MaxValue)
            {
              int num2 = 8 + index2 * 24 + 16;
              long int64 = BitConverter.ToInt64(numArray, num1 + num2);
              FileInfo fileInfo = this.mem.Read<FileInfoPadded>(int64).FileInfo;
              files.TryAdd(this.mem.ReadStringU(fileInfo.Name), new FileInformation(int64, fileInfo.AreaChangeCount));
            }
          }
        }
      }));
      return files.ToDictionary<string, FileInformation>();
    }
  }
}
