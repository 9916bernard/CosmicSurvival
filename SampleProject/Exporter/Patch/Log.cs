using System.IO;
using System.Collections.Generic;


namespace Patch
{
    class Log
    {
        static readonly public string LOG_FILE = "log.cfg";

        static public int ErrorCount = 0;
        static List<string> LogList = new List<string>();

        static public void Start(string log, params object[] arg)
        {
            ErrorCount = 0;
            LogList.Clear();
            N(log, arg);
        }

        static public void N(string log)
        {
            LogList.Add(string.Format("[NOTICE] {0}", log));
        }

        static public void N(string log, params object[] arg)
        {
            LogList.Add(string.Format("[NOTICE] {0}", string.Format(log, arg)));
        }

        //static public void E(string log)
        //{
        //    ErrorCount++;

        //    LogList.Add(string.Format("[ERROR] {0} {1}", log, Exporter.CurrentState.GetStr()));
        //}

        //static public void E(string log, params object[] arg)
        //{
        //    ErrorCount++;

        //    LogList.Add(string.Format("[ERROR] {0} {1}", string.Format(log, arg), Exporter.CurrentState.GetStr()));
        //}

        //static public void Flush()
        //{
        //    StreamWriter w = Util.GetNewTextFile(LOG_FILE);

        //    for (int i = 0; i < LogList.Count; ++i)
        //    {
        //        w.WriteLine(LogList[i]);
        //    }

        //    w.WriteLine("");

        //    Util.FlushAndReleaseTextFile(w);
        //}
    }
}
