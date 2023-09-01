// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.SkillGemWrapper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class SkillGemWrapper : RemoteMemoryObject
  {
    private const int ActiveSkillOffset = 99;
    private const int ActiveSkillSubIdOffset1 = 107;
    private const int ActiveSkillSubIdOffset2 = 48;
    private const int ActiveSkillSubIdRecordLength = 233;

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public ActiveSkillWrapper ActiveSkill => this.ReadObject<ActiveSkillWrapper>(this.Address + 99L);

    public long ActiveSkillSubId => (this.ActiveSkill.Address - this.M.Read<long>(this.Address + 107L, 48, 0)) / 233L;
  }
}
