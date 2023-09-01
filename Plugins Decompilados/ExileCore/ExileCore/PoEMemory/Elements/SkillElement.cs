// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SkillElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements
{
  public class SkillElement : Element
  {
    private const int SkillPtrOffset = 632;

    public bool isValid => this.unknown1 != 0L;

    public bool IsAssignedKeyOrIsActive => this.M.Read<int>(this.unknown1 + 8L) > 3;

    public string SkillIconPath => this.M.ReadStringU(this.M.Read<long>(this.unknown1 + 16L), 100).TrimEnd('0');

    public int totalUses => this.M.Read<int>(this.unknown3 + 80L);

    public bool isUsing => this.M.Read<byte>(this.unknown3 + 8L) > (byte) 2;

    private long unknown1 => this.M.Read<long>(this.Address + 580L);

    private long unknown3 => this.M.Read<long>(this.Address + 812L);

    public ActorSkill Skill => this.ReadObjectAt<ActorSkill>(632).SetActor(this.TheGame.IngameState.Data.LocalPlayer.GetComponent<Actor>());
  }
}
