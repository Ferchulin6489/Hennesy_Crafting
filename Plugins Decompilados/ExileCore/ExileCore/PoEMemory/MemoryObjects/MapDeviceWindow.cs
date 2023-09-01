// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.MapDeviceWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class MapDeviceWindow : Element
  {
    public Element OpenMapDialog => this.GetChildAtIndex(3);

    public Element CloseMapDialog => this.GetChildAtIndex(4);

    public Element ChooseZanaMod => this.GetChildAtIndex(5);

    public Element BottomMapSettings => this.GetChildAtIndex(6);

    public Element ActivateButton => this.BottomMapSettings?.GetChildAtIndex(1);

    public Element ChooseMastersMods => this.BottomMapSettings?.GetChildAtIndex(3);
  }
}
