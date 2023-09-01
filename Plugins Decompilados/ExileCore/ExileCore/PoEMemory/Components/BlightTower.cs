// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.BlightTower
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System.IO;

namespace ExileCore.PoEMemory.Components
{
  public class BlightTower : Component
  {
    private string _iconFileName;
    private BlightTowerDat _info;

    public BlightTowerDat Info => this._info ?? (this._info = this.TheGame.Files.BlightTowers.GetByAddress(this.M.Read<long>(this.Address + 32L)));

    public string Id => this.Info.Id;

    public string Name => this.Info.Name;

    public string Icon => this.Info.Icon;

    public string IconFileName => this._iconFileName ?? (this._iconFileName = Path.GetFileNameWithoutExtension(this.Icon));
  }
}
