// Decompiled with JetBrains decompiler
// Type: ExileCore.CommandExecutor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExileCore
{
  public static class CommandExecutor
  {
    public const string Offset = "offset";
    public const string OffsetS = "offsets";
    public const string LoaderOffsets = "loader_offsets";
    public const string CompilePlugins = "compile_plugins";
    public const string GameOffsets = "GameOffsets.dll";

    public static void Execute(string cmd)
    {
      switch (cmd)
      {
        case "offset":
        case "offsets":
          CommandExecutor.CreateOffsets(true);
          break;
        case "compile_plugins":
          CommandExecutor.CompilePluginsIntoDll();
          break;
        case "loader_offsets":
          CommandExecutor.CreateOffsets();
          break;
        default:
          if (!cmd.StartsWith("compile_"))
            break;
          DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine("Plugins", "Source"));
          string plugin = cmd.Replace("compile_", "");
          if (directoryInfo.GetDirectories().FirstOrDefaultF<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => x.Name.Equals(plugin, StringComparison.OrdinalIgnoreCase))) == null)
            break;
          CommandExecutor.CompilePluginIntoDll(plugin);
          break;
      }
    }

    private static void CompilePluginIntoDll(string plugin)
    {
      DirectoryInfo info = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Source")).GetDirectories().FirstOrDefaultF<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => x.Name.Equals(plugin, StringComparison.OrdinalIgnoreCase)));
      if (info == null)
      {
        DebugWindow.LogError(plugin + " directory not found.");
      }
      else
      {
        using (PluginCompiler orThrow = PluginCompiler.CreateOrThrow())
          CommandExecutor.CompileSourceIntoDll(orThrow, info);
      }
    }

    private static void CompileSourceIntoDll(PluginCompiler compiler, DirectoryInfo info)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      FileInfo csProj = ((IEnumerable<FileInfo>) info.GetFiles("*.csproj", SearchOption.AllDirectories)).FirstOrDefault<FileInfo>();
      if (csProj == null)
      {
        int num1 = (int) MessageBox.Show(".csproj for plugin " + info.Name + " not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string str = info.FullName.Replace("\\Source\\", "\\Compiled\\");
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        try
        {
          compiler.CompilePlugin(csProj, str);
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
          interpolatedStringHandler.AppendFormatted(info.Name);
          interpolatedStringHandler.AppendLiteral("  >>> Successful <<< (Working time: ");
          interpolatedStringHandler.AppendFormatted<long>(stopwatch.ElapsedMilliseconds);
          interpolatedStringHandler.AppendLiteral(" ms.)");
          int num2 = (int) MessageBox.Show(interpolatedStringHandler.ToStringAndClear());
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show(info.Name + " -> Failed, look in " + info.FullName + "/Errors.txt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          File.WriteAllText(Path.Combine(info.FullName, "Errors.txt"), ex.Message);
        }
      }
    }

    private static void CreateOffsets(bool force = false)
    {
      FileInfo fileInfo = new FileInfo("GameOffsets.dll");
      DirectoryInfo directoryInfo = new DirectoryInfo(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "GameOffsets"));
      if (!fileInfo.Exists && !directoryInfo.Exists)
      {
        int num = (int) MessageBox.Show("Offsets dll and folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Environment.Exit(0);
      }
      else if (!directoryInfo.Exists)
      {
        if (!force)
          return;
        int num = (int) MessageBox.Show("Offsets folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string[] array = ((IEnumerable<FileInfo>) directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories)).Select<FileInfo, string>((Func<FileInfo, string>) (x => x.FullName)).ToArray<string>();
        bool flag = force;
        if (!flag)
        {
          foreach (string fileName in array)
          {
            if (new FileInfo(fileName).LastWriteTimeUtc > fileInfo.LastWriteTimeUtc)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          return;
        FileInfo csProj = new FileInfo(Path.Join(directoryInfo.FullName, "GameOffsets.csproj"));
        using (PluginCompiler orThrow = PluginCompiler.CreateOrThrow())
        {
          try
          {
            orThrow.CompilePlugin(csProj, fileInfo.Directory.FullName);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Environment.Exit(1);
          }
          Assembly.Load(File.ReadAllBytes("GameOffsets.dll"));
        }
      }
    }

    private static void CompilePluginsIntoDll()
    {
      List<DirectoryInfo> list = ((IEnumerable<DirectoryInfo>) new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Source")).GetDirectories()).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => (x.Attributes & FileAttributes.Hidden) == (FileAttributes) 0)).ToList<DirectoryInfo>();
      if (list.Count == 0)
      {
        int num = (int) MessageBox.Show("Plugins/Source/ is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PluginCompiler compiler = PluginCompiler.CreateOrThrow())
          Parallel.ForEach<DirectoryInfo>((IEnumerable<DirectoryInfo>) list, (Action<DirectoryInfo>) (info => CommandExecutor.CompileSourceIntoDll(compiler, info)));
      }
    }
  }
}
