﻿using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        /// <summary>Parameterized constructor.
        /// </summary>
        /// <param name="log"></param>
        public Log4NetLogger(ILog log)
        {
            _log = log;
        }

        #region ILogger Members

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }
        public void Debug(object message)
        {
            _log.Debug(message);
        }
        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }
        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }
        public void Info(object message)
        {
            _log.Info(message);
        }
        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }
        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }
        public void Error(object message)
        {
            _log.Error(message);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }
        public void Warn(object message)
        {
            _log.Warn(message);
        }
        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Log(LogSeverity logSeverity, object message, Exception exception)
        {
            switch (logSeverity)
            {
                case LogSeverity.Info:
                    Info(message, exception);
                    break;
                case LogSeverity.Debug:
                    Debug(message, exception);
                    break;
                case LogSeverity.Error:
                    Error(message, exception);
                    break;
                case LogSeverity.Warning:
                    Warn(message, exception);
                    break;
                default:
                    Fatal(message, exception);
                    break;
            }

        }

        public void Log(LogSeverity logSeverity, object message)
        {
            Log(logSeverity, message, null);
        }

        #endregion
    }
}
