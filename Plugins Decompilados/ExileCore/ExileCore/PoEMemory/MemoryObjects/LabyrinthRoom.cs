// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.LabyrinthRoom
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class LabyrinthRoom
  {
    private readonly long Address;
    private readonly IMemory M;

    internal LabyrinthRoom(IMemory m, long address, WorldAreas filesWorldAreas)
    {
      this.FilesWorldAreas = filesWorldAreas;
      this.M = m;
      this.Address = address;
      this.Secret1 = this.ReadSecret(this.M.Read<long>(this.Address + 64L));
      this.Secret2 = this.ReadSecret(this.M.Read<long>(this.Address + 80L));
      this.Section = this.ReadSection(this.M.Read<long>(this.Address + 48L));
      this.Connections = this.M.ReadPointersArray(this.Address, this.Address + 32L).Select<long, LabyrinthRoom>((Func<long, LabyrinthRoom>) (x => x != 0L ? LabyrinthData.GetRoomById(x) : (LabyrinthRoom) null)).ToArray<LabyrinthRoom>();
    }

    public WorldAreas FilesWorldAreas { get; }

    public int Id { get; internal set; }

    public LabyrinthRoom.LabyrinthSecret Secret1 { get; internal set; }

    public LabyrinthRoom.LabyrinthSecret Secret2 { get; internal set; }

    public LabyrinthRoom[] Connections { get; internal set; }

    public LabyrinthRoom.LabyrinthSection Section { get; internal set; }

    internal LabyrinthRoom.LabyrinthSection ReadSection(long addr) => addr == 0L ? (LabyrinthRoom.LabyrinthSection) null : new LabyrinthRoom.LabyrinthSection(this.M, addr, this.FilesWorldAreas);

    private LabyrinthRoom.LabyrinthSecret ReadSecret(long addr)
    {
      if (addr == 0L)
        return (LabyrinthRoom.LabyrinthSecret) null;
      return new LabyrinthRoom.LabyrinthSecret()
      {
        SecretName = this.M.ReadStringU(this.M.Read<long>(addr)),
        Name = this.M.ReadStringU(this.M.Read<long>(addr + 8L))
      };
    }

    public override string ToString()
    {
      string str = "";
      List<LabyrinthRoom> list = ((IEnumerable<LabyrinthRoom>) this.Connections).Where<LabyrinthRoom>((Func<LabyrinthRoom, bool>) (r => r != null)).ToList<LabyrinthRoom>();
      if (list.Count > 0)
        str = "LinkedWith: " + string.Join(", ", list.Select<LabyrinthRoom, string>((Func<LabyrinthRoom, string>) (x => x.Id.ToString())).ToArray<string>());
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 5);
      interpolatedStringHandler.AppendLiteral("Id: ");
      interpolatedStringHandler.AppendFormatted<int>(this.Id);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendLiteral("Secret1: ");
      interpolatedStringHandler.AppendFormatted(this.Secret1 == null ? "None" : this.Secret1.SecretName);
      interpolatedStringHandler.AppendLiteral(", Secret2: ");
      interpolatedStringHandler.AppendFormatted(this.Secret2 == null ? "None" : this.Secret2.SecretName);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(str);
      interpolatedStringHandler.AppendLiteral(", Section: ");
      interpolatedStringHandler.AppendFormatted<LabyrinthRoom.LabyrinthSection>(this.Section);
      return interpolatedStringHandler.ToStringAndClear();
    }

    public class LabyrinthSecret
    {
      public string SecretName { get; internal set; }

      public string Name { get; internal set; }

      public override string ToString() => this.SecretName;
    }

    public class LabyrinthSection
    {
      internal LabyrinthSection(IMemory M, long addr, WorldAreas filesWorldAreas)
      {
        this.FilesWorldAreas = filesWorldAreas;
        this.SectionType = M.ReadStringU(M.Read<long>(addr + 8L, new int[1]));
        int count1 = M.Read<int>(addr + 92L);
        long startAddress1 = M.Read<long>(addr + 100L);
        IList<long> longList = M.ReadSecondPointerArray_Count(startAddress1, count1);
        for (int index = 0; index < count1; ++index)
        {
          LabyrinthSectionOverrides sectionOverrides = new LabyrinthSectionOverrides();
          long addr1 = longList[index];
          sectionOverrides.OverrideName = M.ReadStringU(M.Read<long>(addr1));
          sectionOverrides.Name = M.ReadStringU(M.Read<long>(addr1 + 8L));
          this.Overrides.Add(sectionOverrides);
        }
        this.SectionAreas = new LabyrinthSectionAreas(this.FilesWorldAreas);
        long addr2 = M.Read<long>(addr + 76L);
        this.SectionAreas.Name = M.ReadStringU(M.Read<long>(addr2));
        int count2 = M.Read<int>(addr2 + 8L);
        long startAddress2 = M.Read<long>(addr2 + 16L);
        this.SectionAreas.NormalAreasPtrs = M.ReadSecondPointerArray_Count(startAddress2, count2);
        int count3 = M.Read<int>(addr2 + 24L);
        long startAddress3 = M.Read<long>(addr2 + 32L);
        this.SectionAreas.CruelAreasPtrs = M.ReadSecondPointerArray_Count(startAddress3, count3);
        int count4 = M.Read<int>(addr2 + 40L);
        long startAddress4 = M.Read<long>(addr2 + 48L);
        this.SectionAreas.MercilesAreasPtrs = M.ReadSecondPointerArray_Count(startAddress4, count4);
        int count5 = M.Read<int>(addr2 + 56L);
        long startAddress5 = M.Read<long>(addr2 + 64L);
        this.SectionAreas.EndgameAreasPtrs = M.ReadSecondPointerArray_Count(startAddress5, count5);
      }

      public WorldAreas FilesWorldAreas { get; }

      public string SectionType { get; internal set; }

      public IList<LabyrinthSectionOverrides> Overrides { get; internal set; } = (IList<LabyrinthSectionOverrides>) new List<LabyrinthSectionOverrides>();

      public LabyrinthSectionAreas SectionAreas { get; internal set; }

      public override string ToString()
      {
        string str = "";
        if (this.Overrides.Count > 0)
          str = "Overrides: " + string.Join(", ", this.Overrides.Select<LabyrinthSectionOverrides, string>((Func<LabyrinthSectionOverrides, string>) (x => x.ToString())).ToArray<string>());
        return "SectionType: " + this.SectionType + ", " + str;
      }
    }
  }
}
