// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ProphecyDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ProphecyDat : RemoteMemoryObject
  {
    private string flavourText;
    private string id;
    private string name;
    private string predictionText;
    private string predictionText2;

    public int Index { get; set; }

    public string Id => this.id == null ? (this.id = this.M.ReadStringU(this.M.Read<long>(this.Address), (int) byte.MaxValue)) : this.id;

    public string PredictionText => this.predictionText == null ? (this.predictionText = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L), (int) byte.MaxValue)) : this.predictionText;

    public int ProphecyId => this.M.Read<int>(this.Address + 16L);

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 20L))) : this.name;

    public string FlavourText => this.flavourText == null ? (this.flavourText = this.M.ReadStringU(this.M.Read<long>(this.Address + 28L), (int) byte.MaxValue)) : this.flavourText;

    public long ProphecyChainPtr => this.M.Read<long>(this.Address + 68L);

    public int ProphecyChainPosition => this.M.Read<int>(this.Address + 76L);

    public bool IsEnabled => this.M.Read<byte>(this.Address + 80L) > (byte) 0;

    public int SealCost => this.M.Read<int>(this.Address + 81L);

    public string PredictionText2 => this.predictionText2 == null ? (this.predictionText2 = this.M.ReadStringU(this.M.Read<long>(this.Address + 85L), (int) byte.MaxValue)) : this.predictionText2;

    public override string ToString() => this.Name + ", " + this.PredictionText;
  }
}
