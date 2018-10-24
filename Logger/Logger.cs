/////////////////////////////////////////////////////////////////////////////////
// Logger.cs    : Logs the output of each action                               //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Visual Studio 2017                                           //
// Application  : CSE-681 - BUilder Demo with mock repository and              //
//                mock test harness                                            //
// Author       : Gauri Amberkar, Syracuse University                          //
//                (gdamberk@syr.edu )                                          //
/////////////////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ================= 
 * this package logs Output of every action and store it in log.txt 
 * placed in RepoStorage directory
 * 
 * Public Interface:
 * ================= 
 *  void logBuilder(string message, string filePath) :  logs the output and Error 
 * 
 *  Build Process:
 * --------------
 * Required Files:  None
 * Build Command: csc Logger.cs
 */

using System;
using System.IO;


namespace Logger1
{
    public class Logger
    {
        string filePath;
        //Constructor
        public Logger(string path)
        {
           this.filePath = path;
        }

        //logs the output and Error 
        public void log(string message)
        {
            StreamWriter log = new StreamWriter(filePath, true);
           
            log.WriteLine(DateTime.Now.ToString() + ":" +message);
            log.Flush();
            log.Close();
        }

#if (TEST_LOGGER)
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstration of Logger");
            Console.Write("\n ============================");

            Logger logger = new Logger("../../../RepoStorage/" + "Log.txt");

            logger.log("Demo Log file");
        }
#endif
    }
}
