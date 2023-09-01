// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionDetonator
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class ExpeditionDetonator : Element
  {
    public ExpeditionDetonatorInfo Info => this.ReadObjectAt<ExpeditionDetonatorInfo>(632);

    public int RemainingExplosives => int.Parse(this.GetChildFromIndices(new int[3]).Text);

    public Element RevertExplosiveButton => this.GetChildFromIndices(0, 0, 1);

    public Element ToggleExplosivePlacementButton => this.GetChildFromIndices(0, 1);
  }
}
