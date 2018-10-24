//////////////////////////////////////////////////////////////////////
// MotherBuilder.cs -   This package provides functionality for     //
//                      creating process pool components on command //
//                      and manage execution of build requests by   //
//                      sending those to ready child processes.     //
// ver 1.0                                                          //
// Language:    C#, Visual Studio 2017                              //
// Platform:     Windows 7                                          //
// Application: Build Server                                        //
//                                                                  //
// Author Name : Gauri Amberkar                                     //
// Source: Dr. Jim Fawcett                                          //
// CSE681: Software Modeling and Analysis, Fall 2017                //
//////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * -------------------
 * This Package handles the processing of commands sent by client package uses the Comm
 * class to pass messages to a remote endpoint.
 * and manage execution of build requests by sending those to ready child processes.
 * 
 * Public Interface:
 * ================= 
 * MotherBuilder(): Constructor
 * createProcess(int port) : Function for creating new child process 
 * void recieveMessages() : Recieve Messages using WCF
 * void connectWithRepo() : Send Request message to Repo
 * void connectWithChild(int portNum) : Send request message to CHild
 * bool KillProcess(int id, bool waitForExit = false) : Kill Child process
 * 
 * Build Process:
 * --------------
 * Required Files: BlockingQueue.cs MessagePassingComm.cs 
   Build Command: csc MotherBuilder.cs BlockingQueue.cs MessagePassingComm.cs 

 * Maintenance History:
    - Ver 1.0 Dec 2017 
 * --------------------*/
using MessagePassingComm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace MotherBuilder
{
    public class MotherBuilder
    {
        static SWTools.BlockingQueue<string> ready = new SWTools.BlockingQueue<string>();
        static SWTools.BlockingQueue<string> testQueue = new SWTools.BlockingQueue<string>();
        static int portNumber = 8081;
        Comm c = new Comm("http://localhost", 8081);
        string fromAddr = "http://localhost:8081/IPluggableComm";
        string RepoAddr = "http://localhost:8080/IPluggableComm";
        static List<int> processID = new List<int>();


        //Costructor
        MotherBuilder(int count)
        {
            connectWithRepo();
            for (int i = 1; i <= count; ++i)
            {
                int portNum = portNumber + i;
                connectWithChild(portNum); 
            }
            if (ready == null)
               ready = new SWTools.BlockingQueue<string>();
            if(testQueue == null)
                testQueue = new SWTools.BlockingQueue<string>();

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    if(ready.size() != 0  && testQueue.size() != 0)
                    {
                        string port = ready.deQ();
                        CommMessage csndMsg2 = new CommMessage(CommMessage.MessageType.testRequest);
                        csndMsg2.command = "show";
                        csndMsg2.author = "Jim Fawcett";
                        csndMsg2.to = "http://localhost:"+port+"/IPluggableComm";
                        csndMsg2.from = fromAddr;
                        csndMsg2.body = testQueue.deQ();
                        c.postMessage(csndMsg2);
                    }
                }  
            });
            t.Start();
            recieveMessages(count);
        }

        //Send Request message to Repo
        void connectWithRepo()
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "show";
            csndMsg.author = "Jim Fawcett";
            csndMsg.to = RepoAddr;
            csndMsg.from = fromAddr;
            csndMsg.body = "Connected with Mother BUilder";
            c.postMessage(csndMsg);
        }

        //Send request message to CHild
        void connectWithChild(int portNum)
        {
            CommMessage csndMsg2 = new CommMessage(CommMessage.MessageType.request);
            csndMsg2.command = "show";
            csndMsg2.author = "Jim Fawcett";
            csndMsg2.to = "http://localhost:" + portNum + "/IPluggableComm";
            csndMsg2.from = fromAddr;
            csndMsg2.body = "Connected with Child Builder";
            c.postMessage(csndMsg2);
        }
        //Recieve Messages using WCF
        void recieveMessages(int count)
        {
            while (true)
            {
                CommMessage msg = c.rcvr.getMessage();
                Console.WriteLine("==================================");
                msg.show();
                if (msg.type == CommMessage.MessageType.testRequest)
                {
                    testQueue.enQ(msg.body);
                }
                else if (msg.type == CommMessage.MessageType.ready)
                {
                    Console.WriteLine(" Child Builder with port number = " + msg.body + " is ready");
                    ready.enQ(msg.body);
                }
                else if (msg.type == CommMessage.MessageType.quit)
                {
                    msg.from = fromAddr;
                    for (int i = 1; i <= count; i++)
                    {
                        int portNum = portNumber + i;
                        CommMessage csndMsg3 = new CommMessage(CommMessage.MessageType.quit);
                        csndMsg3.command = "show";
                        csndMsg3.author = "Jim Fawcett";
                        csndMsg3.to = "http://localhost:" + portNum + "/IPluggableComm";
                        csndMsg3.from = fromAddr;
                        csndMsg3.body = "Quitting Child Builder";
                        c.postMessage(csndMsg3);
                    }
                    foreach (int pid in processID)
                    {
                        KillProcess(pid);
                    }
                    Process.GetCurrentProcess().Kill();
                }
            }
        }


        //Kill Child process
        bool KillProcess(int id, bool waitForExit = false)
        {
            using (Process proc = Process.GetProcessById(id))
            {
                if (proc == null || proc.HasExited)
                    return false;
                proc.Kill();
                if (waitForExit)
                {
                    proc.WaitForExit();
                }
                return true;
            }
        }

        //Function for creating new child process 
        static bool createProcess(int portNum)
        {
            Process proc = new Process();
            string fileName = "..\\..\\..\\ChildBuilder\\bin\\debug\\ChildBuilder.exe";
            string absFileSpec = Path.GetFullPath(fileName);
            Console.WriteLine(absFileSpec);

            Console.WriteLine("\n  attempting to start {0}", absFileSpec);
            string commandline = portNum.ToString();
            try
            {
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.Arguments = commandline;
                proc.Start();
                processID.Add(proc.Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("\n  {0}", ex.Message);
                return false;
            }
            return true;
        }

#if (TEST_MOTHER)
        static void Main(string[] args)
        {
            Console.Title = "Mother Builder";
            int count = 0;
            if (args.Count() == 0)
            {
                Console.WriteLine("\n  please enter number of processes to create on command line");
                return;
            }
            else
            { 
                count = Int32.Parse(args[0]);
                for (int i = 1; i <= count; ++i)
                {
                    createProcess(portNumber + i);
                    Console.WriteLine(portNumber + i);
                    ready.enQ((portNumber + i).ToString());
                }
            }
            MotherBuilder mother = new MotherBuilder(count);
        }
#endif
    }
}
