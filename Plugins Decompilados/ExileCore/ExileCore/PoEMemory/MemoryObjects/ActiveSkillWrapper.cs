// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ActiveSkillWrapper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ActiveSkillWrapper : RemoteMemoryObject
  {
    public string InternalName => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string DisplayName => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string Description => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public string SkillName => this.M.ReadStringU(this.M.Read<long>(this.Address + 24L));

    public string Icon => this.M.ReadStringU(this.M.Read<long>(this.Address + 32L));

    public List<int> CastTypes
    {
      get
      {
        List<int> castTypes = new List<int>();
        int num = this.M.Read<int>(this.Address + 40L);
        long addr = this.M.Read<long>(this.Address + 48L);
        for (int index = 0; index < num; ++index)
        {
          castTypes.Add(this.M.Read<int>(addr));
          addr += 4L;
        }
        return castTypes;
      }
    }

    public List<int> SkillTypes
    {
      get
      {
        List<int> skillTypes = new List<int>();
        int num = this.M.Read<int>(this.Address + 56L);
        long addr = this.M.Read<long>(this.Address + 64L);
        for (int index = 0; index < num; ++index)
        {
          skillTypes.Add(this.M.Read<int>(addr));
          addr += 4L;
        }
        return skillTypes;
      }
    }

    public string LongDescription => this.M.ReadStringU(this.M.Read<long>(this.Address + 80L));

    public string AmazonLink => this.M.ReadStringU(this.M.Read<long>(this.Address + 96L));
  }
}
