﻿using System;

namespace CSharpSnippets.Logging
{
    interface ILogger
    {
        void Trace(string msg);
        void Trace(string msg, params object[] args);
        void Trace(Exception e, string msg, params object[] args);
        void Debug(string msg);
        void Debug(string msg, params object[] args);
        void Debug(Exception e, string msg, params object[] args);
        void Info(string msg);
        void Info(string msg, params object[] args);
        void Info(Exception e, string msg, params object[] args);
        void Warn(string msg);
        void Warn(string msg, params object[] args);
        void Warn(Exception e, string msg, params object[] args);
        void Error(string msg);
        void Error(string msg, params object[] args);
        void Error(Exception e, string msg, params object[] args);
        void Fatal(string msg);
        void Fatal(string msg, params object[] args);
        void Fatal(Exception e, string msg, params object[] args);
    }
}
