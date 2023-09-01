// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ChatPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class ChatPanel : Element
  {
    public Element ChatTitlePanel => this.ReadObjectAt<Element>(768);

    public Element ChatInputElement => this.ReadObjectAt<Element>(840);

    public PoeChatElement ChatBox => this.ReadObjectAt<Element>(816).ReadObjectAt<PoeChatElement>(912);

    public string InputText => this.ChatInputElement.Text;
  }
}
