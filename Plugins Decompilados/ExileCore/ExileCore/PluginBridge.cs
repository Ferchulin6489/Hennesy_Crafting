// Decompiled with JetBrains decompiler
// Type: ExileCore.PluginBridge
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore
{
  public class PluginBridge
  {
    private readonly Dictionary<string, object> methods = new Dictionary<string, object>();

    public T GetMethod<T>(string name) where T : class
    {
      object obj;
      return this.methods.TryGetValue(name, out obj) ? obj as T : default (T);
    }

    public void SaveMethod(string name, object method) => this.methods[name] = method;
  }
}
