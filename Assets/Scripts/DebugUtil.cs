using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class DebugUtil
{
    private static Queue<string> _logs = new Queue<string>();
    private static int _logCount = 0;

    private static object locker = new object();

    public static void PrintLogs()
    {
        while (_logs.Count > 0)
        {
            UnityEngine.Debug.Log(_logs.Dequeue());
        }
    }

    public static void Log(string message)
    {
        lock (locker)
        {
            _logCount++;
            StackTrace st = new StackTrace();

            string s = "";

            s += $"Log{_logCount} : " + message + "\n";

            s += "===============================\nStackTrace\n";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);

                s += sf.GetMethod().DeclaringType.Name + ".";
                s += sf.GetMethod().Name;
                s += "\n";
            }

            _logs.Enqueue(s);
        }
    }

    public static void Log(object message)
    {
        Log(message.ToString());
    }

    public static void Log(int message)
    {
        Log(message.ToString());
    }

    public static void Log(float message)
    {
        Log(message.ToString());
    }
}