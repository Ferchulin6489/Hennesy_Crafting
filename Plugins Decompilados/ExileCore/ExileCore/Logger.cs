// Decompiled with JetBrains decompiler
// Type: ExileCore.Logger
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;


#nullable enable
namespace ExileCore
{
  public class Logger
  {
    private static 
    #nullable disable
    ILogger _instance;

    public static ILogger Log
    {
      get
      {
        ILogger instance = Logger._instance;
        if (instance != null)
          return instance;
        LoggerSinkConfiguration writeTo = new LoggerConfiguration().MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Verbose)).WriteTo.Logger((Action<LoggerConfiguration>) (l => l.Filter.ByIncludingOnly((Func<LogEvent, bool>) (e => e.Level == LogEventLevel.Information)).WriteTo.File("Logs\\Info.log", rollingInterval: RollingInterval.Day))).WriteTo.Logger((Action<LoggerConfiguration>) (l => l.Filter.ByIncludingOnly((Func<LogEvent, bool>) (e => e.Level == LogEventLevel.Debug)).WriteTo.File("Logs\\Debug.log", rollingInterval: RollingInterval.Day))).WriteTo.Logger((Action<LoggerConfiguration>) (l => l.Filter.ByIncludingOnly((Func<LogEvent, bool>) (e => e.Level == LogEventLevel.Warning)).WriteTo.File("Logs\\Warning.log", rollingInterval: RollingInterval.Day))).WriteTo.Logger((Action<LoggerConfiguration>) (l => l.Filter.ByIncludingOnly((Func<LogEvent, bool>) (e => e.Level == LogEventLevel.Error)).WriteTo.File("Logs\\Error.log", rollingInterval: RollingInterval.Day))).WriteTo.Logger((Action<LoggerConfiguration>) (l => l.Filter.ByIncludingOnly((Func<LogEvent, bool>) (e => e.Level == LogEventLevel.Fatal)).WriteTo.File("Logs\\Fatal.log", rollingInterval: RollingInterval.Day))).WriteTo;
        long? fileSizeLimitBytes = new long?(1073741824L);
        TimeSpan? flushToDiskInterval = new TimeSpan?();
        int? retainedFileCountLimit = new int?(31);
        TimeSpan? retainedFileTimeLimit = new TimeSpan?();
        return Logger._instance = (ILogger) writeTo.File("Logs\\Verbose.log", fileSizeLimitBytes: fileSizeLimitBytes, flushToDiskInterval: flushToDiskInterval, rollingInterval: RollingInterval.Day, retainedFileCountLimit: retainedFileCountLimit, retainedFileTimeLimit: retainedFileTimeLimit).CreateLogger();
      }
    }
  }
}
