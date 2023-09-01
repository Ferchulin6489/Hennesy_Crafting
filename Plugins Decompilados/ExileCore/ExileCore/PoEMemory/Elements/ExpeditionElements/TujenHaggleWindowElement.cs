// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionElements.TujenHaggleWindowElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements.ExpeditionElements
{
  public class TujenHaggleWindowElement : Element
  {
    public string WindowTitle => this.GetChildAtIndex(0).Text;

    public Element HaggleTargetItem => this.GetChildAtIndex(1);

    public Element HaggleArtifactType => this.GetChildAtIndex(3);

    public int HaggleArtifactCurrentOfferAmount => int.Parse(this.GetChildAtIndex(4)?.Text ?? "0");

    public ArtifactSliderElement ArtifactOfferSliderElement => this[5].AsObject<ArtifactSliderElement>();

    public Element SameNewOfferIndicator => this.GetChildAtIndex(6);

    public Element ConfirmButton => this.GetChildAtIndex(7);

    public Element ExitWindowButton => this.GetChildAtIndex(8);

    public Element HaggleTargetItemTooltipElement => this.GetChildAtIndex(9);
  }
}
