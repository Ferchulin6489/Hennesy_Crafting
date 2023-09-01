// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Nodes.FileNode
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Nodes
{
  public sealed class FileNode
  {
    private string value;

    public FileNode()
    {
    }

    public FileNode(string value) => this.Value = value;

    public string Value
    {
      get => this.value;
      set
      {
        this.value = value;
        EventHandler<string> onFileChanged = this.OnFileChanged;
        if (onFileChanged == null)
          return;
        onFileChanged((object) this, value);
      }
    }

    public event EventHandler<string> OnFileChanged;

    public static implicit operator string(FileNode node) => node.Value;

    public static implicit operator FileNode(string value) => new FileNode(value);
  }
}
