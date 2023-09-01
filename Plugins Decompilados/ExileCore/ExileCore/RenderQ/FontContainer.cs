// Decompiled with JetBrains decompiler
// Type: ExileCore.RenderQ.FontContainer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ImGuiNET;

namespace ExileCore.RenderQ
{
  public readonly struct FontContainer
  {
    public readonly unsafe ImFont* Atlas;
    public readonly string Name;
    public readonly int Size;

    public unsafe FontContainer(ImFont* atlas, string name, int size)
    {
      this.Atlas = atlas;
      this.Name = name;
      this.Size = size;
    }
  }
}
