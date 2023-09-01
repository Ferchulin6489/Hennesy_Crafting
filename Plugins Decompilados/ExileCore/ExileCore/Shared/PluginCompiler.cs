// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.PluginCompiler
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ImGuiNET;
using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Locator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore.Shared
{
  public class PluginCompiler : IDisposable
  {
    private static readonly SemaphoreSlim BuildSemaphore = new SemaphoreSlim(1, 1);
    private readonly BuildManager buildManager = new BuildManager("pluginCompiler");

    private PluginCompiler()
    {
    }

    public static PluginCompiler Create() => !MSBuildLocator.IsRegistered ? (PluginCompiler) null : new PluginCompiler();

    public static PluginCompiler CreateOrThrow() => PluginCompiler.Create() ?? throw new Exception("Plugin compilation is disabled");

    public void CompilePlugin(FileInfo csProj, string outputDirectory)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        ["OutputPath"] = outputDirectory,
        ["exapiPackage"] = AppDomain.CurrentDomain.BaseDirectory,
        ["RuntimeIdentifier"] = "win-x64",
        ["SelfContained"] = "False",
        ["PathMap"] = ""
      };
      string str = csProj.Name.Replace(csProj.Extension, "");
      ProjectPropertyElement projectPropertyElement = (ProjectPropertyElement) null;
      try
      {
        ProjectRootElement pre = ProjectRootElement.Open(csProj.FullName);
        PluginCompiler.PatchProject(pre);
        if (pre.HasUnsavedChanges)
          pre.Save();
        projectPropertyElement = pre.Properties.FirstOrDefault<ProjectPropertyElement>((Func<ProjectPropertyElement, bool>) (x => x.Name == "TargetFramework"));
        PluginCompiler.BuildSemaphore.Wait();
        try
        {
          using (ProjectCollection projectCollection = new ProjectCollection())
          {
            MsBuildLogger msBuildLogger = new MsBuildLogger();
            BuildResult buildResult = this.buildManager.Build(new BuildParameters(projectCollection)
            {
              DisableInProcNode = true,
              EnableNodeReuse = true,
              Loggers = (IEnumerable<ILogger>) new MsBuildLogger[1]
              {
                msBuildLogger
              }
            }, new BuildRequestData(ProjectInstance.FromProjectRootElement(pre, new ProjectOptions()
            {
              GlobalProperties = (IDictionary<string, string>) dictionary
            }), new string[2]{ "Restore", "Build" }, (HostServices) null));
            if (buildResult.OverallResult != BuildResultCode.Success)
              throw buildResult.Exception ?? new Exception("Build failed:\n" + string.Join<BuildError>("\n", (IEnumerable<BuildError>) msBuildLogger.Errors));
            projectCollection.UnloadAllProjects();
          }
        }
        finally
        {
          PluginCompiler.BuildSemaphore.Release();
        }
      }
      catch (Exception ex)
      {
        if (projectPropertyElement == null || projectPropertyElement.Value == null)
          DebugWindow.LogError(str + " -> CompilePlugin failed, but you can try running the fix_plugins.ps1 script", 10f);
        else if (projectPropertyElement.Value == "net4.8")
          DebugWindow.LogError(str + " -> CompilePlugin failed, but you can try updating its TargetFramework to net6.0-windows", 10f);
        else
          DebugWindow.LogError(str + " -> CompilePlugin failed");
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
        interpolatedStringHandler.AppendFormatted(str);
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        throw;
      }
    }

    private static void PatchProject(ProjectRootElement pre)
    {
      ProjectItemElement projectItemElement = pre.Items.FirstOrDefault<ProjectItemElement>((Func<ProjectItemElement, bool>) (x => x.ItemType == "PackageReference" && x.Include.Equals("ImGui.NET", StringComparison.OrdinalIgnoreCase)));
      if (projectItemElement != null)
      {
        ProjectMetadataElement projectMetadataElement = projectItemElement.Metadata.FirstOrDefault<ProjectMetadataElement>((Func<ProjectMetadataElement, bool>) (x => x.Name == "Version"));
        if (projectMetadataElement != null)
        {
          string version = Assembly.GetAssembly(typeof (ImGui)).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
          if (version.Trim() != projectMetadataElement.Value.Trim())
            projectMetadataElement.Value = version;
        }
      }
      ProjectPropertyElement projectPropertyElement = pre.Properties.FirstOrDefault<ProjectPropertyElement>((Func<ProjectPropertyElement, bool>) (x => x.Name == "TargetFramework"));
      string str = projectPropertyElement?.Value;
      if (str == null || str.StartsWith("net4") || !(str != "net6.0-windows"))
        return;
      projectPropertyElement.Value = "net6.0-windows";
    }

    public void Dispose()
    {
      this.buildManager.ResetCaches();
      this.buildManager.CancelAllSubmissions();
      this.buildManager.ShutdownAllNodes();
      this.buildManager.Dispose();
    }
  }
}
