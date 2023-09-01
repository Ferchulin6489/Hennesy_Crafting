// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphMetaSkillType
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Models;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory.Metamorph
{
  public class MetamorphMetaSkillType : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address), (int) byte.MaxValue);

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L), (int) byte.MaxValue);

    public string Description => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L), (int) byte.MaxValue);

    public BaseItemType BaseItemType => this.TheGame.Files.BaseItemTypes.GetFromAddress(this.M.Read<long>(this.Address + 56L));

    public string BodyPart => this.M.ReadStringU(this.M.Read<long>(this.Address + 64L), (int) byte.MaxValue);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 5);
      interpolatedStringHandler.AppendFormatted(this.BodyPart);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.BaseItemType?.BaseName);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Description);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
