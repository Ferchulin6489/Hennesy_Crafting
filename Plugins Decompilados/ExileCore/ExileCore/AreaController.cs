// Decompiled with JetBrains decompiler
// Type: ExileCore.AreaController
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System;

namespace ExileCore
{
  public class AreaController
  {
    public AreaController(TheGame theGameState) => this.TheGameState = theGameState;

    public TheGame TheGameState { get; }

    public AreaInstance CurrentArea { get; private set; }

    public event Action<AreaInstance> OnAreaChange;

    public void ForceRefreshArea(bool areaChangeMultiThread)
    {
      IngameData data = this.TheGameState.IngameState.Data;
      this.CurrentArea = new AreaInstance(data.CurrentArea, this.TheGameState.CurrentAreaHash, data.CurrentAreaLevel);
      if (this.CurrentArea.Name.Length == 0)
        return;
      this.ActionAreaChange();
    }

    public bool RefreshState()
    {
      IngameData data = this.TheGameState.IngameState.Data;
      AreaTemplate currentArea = data.CurrentArea;
      uint currentAreaHash = this.TheGameState.CurrentAreaHash;
      if (this.CurrentArea != null && (int) currentAreaHash == (int) this.CurrentArea.Hash)
        return false;
      this.CurrentArea = new AreaInstance(currentArea, currentAreaHash, data.CurrentAreaLevel);
      if (this.CurrentArea.Name.Length == 0)
        return false;
      this.ActionAreaChange();
      return true;
    }

    private void ActionAreaChange()
    {
      Action<AreaInstance> onAreaChange = this.OnAreaChange;
      if (onAreaChange == null)
        return;
      onAreaChange(this.CurrentArea);
    }
  }
}
