// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.MsBuildLogger
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

namespace ExileCore.Shared
{
  public class MsBuildLogger : Logger
  {
    public IList<BuildTarget> Targets { get; private set; }

    public IList<BuildError> Errors { get; private set; }

    public IList<BuildWarning> Warnings { get; private set; }

    public IList<string> BuildDetails { get; private set; }

    public virtual void Initialize(IEventSource eventSource)
    {
      this.BuildDetails = (IList<string>) new List<string>();
      this.Targets = (IList<BuildTarget>) new List<BuildTarget>();
      this.Errors = (IList<BuildError>) new List<BuildError>();
      this.Warnings = (IList<BuildWarning>) new List<BuildWarning>();
      eventSource.ProjectStarted += new ProjectStartedEventHandler(this.EventSource_ProjectStarted);
      eventSource.TargetFinished += new TargetFinishedEventHandler(this.EventSource_TargetFinished);
      eventSource.ErrorRaised += new BuildErrorEventHandler(this.EventSource_ErrorRaised);
      eventSource.WarningRaised += new BuildWarningEventHandler(this.EventSource_WarningRaised);
      eventSource.ProjectFinished += new ProjectFinishedEventHandler(this.EventSource_ProjectFinished);
    }

    private void EventSource_ProjectStarted(object sender, ProjectStartedEventArgs e) => this.BuildDetails.Add(e.Message);

    private void EventSource_TargetFinished(object sender, TargetFinishedEventArgs e) => this.Targets.Add(new BuildTarget()
    {
      Name = e.TargetName,
      File = e.TargetFile,
      Succeeded = e.Succeeded,
      Outputs = e.TargetOutputs
    });

    private void EventSource_ErrorRaised(object sender, BuildErrorEventArgs e) => this.Errors.Add(new BuildError()
    {
      File = e.File,
      Timestamp = e.Timestamp,
      LineNumber = e.LineNumber,
      ColumnNumber = e.ColumnNumber,
      Code = e.Code,
      Message = e.Message
    });

    private void EventSource_WarningRaised(object sender, BuildWarningEventArgs e) => this.Warnings.Add(new BuildWarning()
    {
      File = e.File,
      Timestamp = e.Timestamp,
      LineNumber = e.LineNumber,
      ColumnNumber = e.ColumnNumber,
      Code = e.Code,
      Message = e.Message
    });

    private void EventSource_ProjectFinished(object sender, ProjectFinishedEventArgs e) => this.BuildDetails.Add(e.Message);
  }
}
