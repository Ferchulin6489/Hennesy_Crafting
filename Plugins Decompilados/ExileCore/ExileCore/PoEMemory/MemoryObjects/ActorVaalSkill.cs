// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ActorVaalSkill
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ActorVaalSkill : RemoteMemoryObject
  {
    private const int NAMES_POINTER_OFFSET = 0;
    private const int INTERNAL_NAME_OFFSET = 0;
    private const int NAME_OFFSET = 8;
    private const int DESCRIPTION_OFFSET = 16;
    private const int SKILL_NAME_OFFSET = 24;
    private const int ICON_OFFSET = 32;
    private const int MAX_VAAL_SOULS_OFFSET = 16;
    private const int CURRENT_VAAL_SOULS_OFFSET = 20;

    private long NamesPointer => this.M.Read<long>(this.Address);

    public string VaalSkillInternalName => this.M.ReadStringU(this.M.Read<long>(this.NamesPointer));

    public string VaalSkillDisplayName => this.M.ReadStringU(this.M.Read<long>(this.NamesPointer + 8L));

    public string VaalSkillDescription => this.M.ReadStringU(this.M.Read<long>(this.NamesPointer + 16L));

    public string VaalSkillSkillName => this.M.ReadStringU(this.M.Read<long>(this.NamesPointer + 24L));

    public string VaalSkillIcon => this.M.ReadStringU(this.M.Read<long>(this.NamesPointer + 32L));

    public int VaalMaxSouls => this.M.Read<int>(this.Address + 16L);

    [Obsolete("Use VaalMaxSouls instead")]
    public int VaalSoulsPerUse => this.VaalMaxSouls;

    public int CurrVaalSouls => this.M.Read<int>(this.Address + 20L);
  }
}
