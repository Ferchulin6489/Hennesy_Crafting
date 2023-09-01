// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Models.BaseItemType
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Text;

namespace ExileCore.PoEMemory.Models
{
  public class BaseItemType
  {
    public string Metadata { get; set; }

    public string ClassName { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int DropLevel { get; set; }

    public string BaseName { get; set; }

    public string[] Tags { get; set; }

    public string[] MoreTagsFromPath { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Tags: ");
      foreach (string tag in this.Tags)
      {
        stringBuilder.Append(tag);
        stringBuilder.Append(" ");
      }
      stringBuilder.Append("More Tags: ");
      foreach (string str in this.MoreTagsFromPath)
      {
        stringBuilder.Append(str);
        stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }
  }
}
