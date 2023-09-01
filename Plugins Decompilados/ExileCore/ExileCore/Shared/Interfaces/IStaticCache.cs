// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Interfaces.IStaticCache
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.Shared.Interfaces
{
  public interface IStaticCache
  {
    int Count { get; }

    int DeletedCache { get; }

    int ReadCache { get; }

    int ReadMemory { get; }

    string CoeffString { get; }

    float Coeff { get; }

    void UpdateCache();

    bool Remove(string key);
  }
}
