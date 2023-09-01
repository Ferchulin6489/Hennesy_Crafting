// Decompiled with JetBrains decompiler
// Type: ExileCore.ActionOverlay
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ClickableTransparentOverlay;
using System;
using System.Threading.Tasks;


#nullable enable
namespace ExileCore
{
  public class ActionOverlay : Overlay
  {
    public ActionOverlay(
    #nullable disable
    string windowTitle)
      : base(windowTitle)
    {
    }

    public Action RenderAction { get; set; }

    public Action PostFrameAction { get; set; }

    public Func<Task> PostInitializedAction { get; set; }

    protected override void Render()
    {
      Action renderAction = this.RenderAction;
      if (renderAction == null)
        return;
      renderAction();
    }

    protected override async Task PostInitialized()
    {
      if (this.PostInitializedAction == null)
        return;
      await this.PostInitializedAction();
    }

    protected override void PostFrame()
    {
      Action postFrameAction = this.PostFrameAction;
      if (postFrameAction == null)
        return;
      postFrameAction();
    }
  }
}
