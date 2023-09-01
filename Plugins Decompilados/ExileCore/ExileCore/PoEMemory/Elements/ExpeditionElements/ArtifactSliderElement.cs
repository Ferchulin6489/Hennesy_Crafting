// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionElements.ArtifactSliderElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements.ExpeditionElements
{
  public class ArtifactSliderElement : Element
  {
    private const int CurrentOfferOffset = 644;
    private const int CurrentMinOfferOffset = 656;
    private const int CurrentMaxOfferOffset = 660;
    private const int MaxOfferOffset = 668;

    public int CurrentOffer => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 644L);

    public int CurrentMinOffer => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 656L);

    public int CurrentMaxOffer => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 660L);

    public int MaxOffer => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 668L);
  }
}
