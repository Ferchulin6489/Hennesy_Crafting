// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.PluginAssemblyLoadContext
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ExileCore.Shared
{
  internal class PluginAssemblyLoadContext : AssemblyLoadContext
  {
    private readonly string _pluginAssemblyLocation;
    private readonly bool _loadFromStream;
    private readonly AssemblyDependencyResolver _resolver;

    public PluginAssemblyLoadContext(string pluginAssemblyLocation, bool loadFromStream)
    {
      this._pluginAssemblyLocation = pluginAssemblyLocation;
      this._loadFromStream = loadFromStream;
      this._resolver = new AssemblyDependencyResolver(pluginAssemblyLocation);
      this.Resolving += new Func<AssemblyLoadContext, AssemblyName, Assembly>(this.ResolvingCallback);
      this.ResolvingUnmanagedDll += new Func<Assembly, string, IntPtr>(this.ResolvingUnmanagedDllCallback);
    }

    private Assembly ResolvingCallback(AssemblyLoadContext context, AssemblyName assemblyName)
    {
      string str1 = this._resolver.ResolveAssemblyToPath(assemblyName);
      if (str1 == null)
      {
        string path = Path.Join(Path.GetDirectoryName(this._pluginAssemblyLocation), assemblyName.Name + ".dll");
        if (File.Exists(path))
          str1 = path;
      }
      if (str1 == null)
        return (Assembly) null;
      if (!this._loadFromStream)
        return context.LoadFromAssemblyPath(str1);
      using (FileStream assembly = File.OpenRead(str1))
      {
        string str2 = str1;
        int length = ".exe".Length;
        string path = str2.Substring(0, str2.Length - length) + ".pdb";
        using (FileStream assemblySymbols = File.Exists(path) ? File.OpenRead(path) : (FileStream) null)
          return context.LoadFromStream((Stream) assembly, (Stream) assemblySymbols);
      }
    }

    private IntPtr ResolvingUnmanagedDllCallback(Assembly assembly, string dllName)
    {
      string path = this._resolver.ResolveUnmanagedDllToPath(dllName);
      return path != null ? this.LoadUnmanagedDllFromPath(path) : IntPtr.Zero;
    }
  }
}
