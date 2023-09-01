// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionElements.ExpeditionVendorCurrencyInfoElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements.ExpeditionElements
{
  public class ExpeditionVendorCurrencyInfoElement : Element
  {
    public int GwennenRerolls
    {
      get
      {
        int result;
        return !int.TryParse(this.GetChildFromIndices(0, 1)?.Text ?? "", out result) ? 0 : result;
      }
    }

    public int TujenRerolls
    {
      get
      {
        int result;
        return !int.TryParse(this.GetChildFromIndices(1, 1)?.Text ?? "", out result) ? 0 : result;
      }
    }

    public int RogRerolls
    {
      get
      {
        int result;
        return !int.TryParse(this.GetChildFromIndices(2, 1)?.Text ?? "", out result) ? 0 : result;
      }
    }

    public int DannigRerolls
    {
      get
      {
        int result;
        return !int.TryParse(this.GetChildFromIndices(3, 1)?.Text ?? "", out result) ? 0 : result;
      }
    }
  }
}
