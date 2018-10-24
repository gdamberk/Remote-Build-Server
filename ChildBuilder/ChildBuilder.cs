/////////////////////////////////////////////////////////////////////
// Builder.cs -   This package provides core functionality         //
//                of building files for Build Server system        //
// ver 1.0                                                         //
// Language:    C#, Visual Studio 2017                             //
// Platform:    Windows 7                                          //
// Application: Build Server                                       //
//                                                                 //
// Name : Gauri Amberkar                                          //
// CSE681: Software Modeling and Analysis, Fall 2017               //
/////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * -------------------
 * This Package handles the processing of commands sent by other packages 
 * and builds files sent to it by repository the Comm
 * class to pass messages to a remote endpoint.
 * 
 * Public Interface:
 * ================= 
 * ChildBuilder  : Costructor
 * void recieveMessages() : Recieve Messages using WCF
 * void sendLog(string log,string filename) : Used for sending log file to repository
 * bool parseTest() : Parse the test request recieved from Mother builder
 * void connectWithTestHarness(string childAddr) : Send request to Test Harness
 * void getFilesFromRepo(List<String> files, string child): Send file request to Repo
 * void sentFileMessage(string childAddr) : Sent message to Repo after requesting all the files
 * void sendReadyMessage(string childAddr, int port) : send ready message to mother builder
 * string CreateTestReq(TestRequest testBuild) : Create test request
 * void sendLibToRepo(string name) : send Library to Repo
 * void SendTestRequest(string testReq, string childAddr) : Send Test request to Test Harness

 * 
 * Build Process:
 * --------------
 * Required Files:  Logger.cs MPCommService.cs IMPCommService.cs TestRequest.cs 
   Build Command: csc ChildBuilder.cs  Logger.cs MPCommService.cs IMPCommService.cs TestRequest.cs 

 * Maintenance History:
    - Ver 1.0 Dec 2017 
 * --------------------*/


using MessagePassingComm;
using TestRequest1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Test1;
using Logger1;
using System.Diagnostics;


namespace ChildBuilder
{
    class ChildBuilder
    {
        static String testRequest;
        static String MotherAddr = "http://localhost:8081/IPluggableComm";
        static String RepoAddr = "http://localhost:8080/IPluggableComm";
        static String TestAddr = "http://localhost:8095/IPluggableComm";
        static String RepoPath = "../../../RepoStorage";
        static List<String> fileName = new List<String>();
        static String Dir;
        static Comm c;
        static string logfile = null;
        Logger logger;
        TestRequest parse = null;
        Random random = new Random();
        List<bool> status = new List<bool>();
        string childAddr = null;

        //Constructor
        ChildBuilder(int port)
        {
            c = new Comm("http://localhost", port);
            Dir = "../../../Builder" + port;
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            childAddr =  "http://localhost:" + port + "/IPluggableComm";
            connectWithTestHarness(childAddr);
            recieveMessages(port);    
        }

        //Recieve Messages using WCF
        void recieveMessages(int port)
        {
            while (true)
            {
                CommMessage msg = c.rcvr.getMessage();
                childAddr = "http://localhost:" + port + "/IPluggableComm";
                msg.show();
                if (msg.type == CommMessage.MessageType.testRequest)
                {
                    fileName.Clear();
                    testRequest = msg.body;
                    parse = new TestRequest();
                    parse = parseTest();
                    getFilesFromRepo(fileName, childAddr);
                    sentFileMessage(childAddr);
                }
                else if (msg.type == CommMessage.MessageType.reply)
                {
                    status.Clear();
                    foreach (Test test in parse.test)
                    {
                        bool stat = BuildCs(test, port);
                        status.Add(stat);
                    }
                    if (!(status.Count() == 1 && status[0] == false))
                    {
                        string testRequest = CreateTestReq(parse);
                        SendTestRequest(testRequest, childAddr);
                    }
                    sendReadyMessage( childAddr, port);   
                }
                else if (msg.type == CommMessage.MessageType.fileReq)
                {
                    string body = msg.body;
                    string[] path = body.Split('|');
                    Console.WriteLine("Sending file to Test Harness \"{0}\"", path[0]);
                    bool transferSuccess = c.postFile(path[0], Dir, path[1]);
                }
            }
        }

        //Send request to Test Harness
        void connectWithTestHarness(string childAddr)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "show";
            csndMsg.author = "Jim Fawcett";
            csndMsg.to =  TestAddr;
            csndMsg.from = childAddr;
            csndMsg.body = "Connected with Test Harness";
            c.postMessage(csndMsg);
        }

        //Send file request to Repo
        void getFilesFromRepo(List<String> files, string child)
        {
            foreach (string name in files)
            {
                CommMessage csndMsg2 = new CommMessage(CommMessage.MessageType.fileReq);
                csndMsg2.command = "show";
                csndMsg2.author = "Jim Fawcett";
                csndMsg2.to = RepoAddr;
                csndMsg2.from = child;
                csndMsg2.body = name + "|" + Dir;
                c.postMessage(csndMsg2);
                Console.WriteLine(name + " file receiving from Repository");
            }
        }

        //Sent message to Repo after requesting all the files
        void sentFileMessage(string childAddr)
        {
            CommMessage csndMsg3 = new CommMessage(CommMessage.MessageType.file);
            csndMsg3.command = "show";
            csndMsg3.author = "Jim Fawcett";
            csndMsg3.to = RepoAddr;
            csndMsg3.from = childAddr;
            csndMsg3.body = "Send Files";
            c.postMessage(csndMsg3);
        }

        //send ready message to mother builder
        void sendReadyMessage(string childAddr, int port)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.ready);
            csndMsg.command = "show";
            csndMsg.author = "Jim Fawcett";
            csndMsg.to = MotherAddr;
            csndMsg.from = childAddr;
            csndMsg.body = port.ToString();
            c.postMessage(csndMsg);
            
        }

        //This method build the .cs files and create .dll library
        bool BuildCs(Test test, int port)
        {
            string  logName = "LogBuilder" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
            logfile = Dir + "/" + logName;
            logger = new Logger(logfile);

            Console.Write("\n\n Building file");
            Console.Write("\n\n=================\n\n");
            string testFiles = "";
            foreach (string file in test.testedFiles)
            {
                testFiles += file;
                testFiles += " ";
            }
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo.Arguments = "/Ccsc /target:library " + test.testDriver + " " + testFiles;

            p.StartInfo.WorkingDirectory = Dir;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();

            string errors = p.StandardError.ReadToEnd();
            string output = p.StandardOutput.ReadToEnd();

            string[] filename = test.testDriver.Split('.');
            if (!output.Contains("error"))
            {
                Console.WriteLine("Build Successfull for " + filename[0] + ".cs \n\n");
                Console.WriteLine("\n\n output : " + output);
                logger.log("Build Successfull for " + filename[0] + ".cs \n\n");
                logger.log("output : " + output);
                SendLog(logName);
                return (true);
            }
            else{
                logger.log("Build Failed for " + filename[0] + ".cs \n\n");
                logger.log("\n\n Error : " + output);
                Console.WriteLine("\n\n Error : " + output);
                SendLog(logName);
                return (false);
            }   
        }

        // Used for sending log file to repository
        static void SendLog( string file)
        {
            bool transferSuccess = c.postFile(file, Dir, RepoPath);
            Console.WriteLine("Sending Log file " + file + " to Repository");
        }

        //Parse the test request recieved from Mother builder
        TestRequest parseTest()
        {
            TestRequest parsed = new TestRequest();
            XDocument doc = XDocument.Parse(testRequest);
            doc.Save(Dir + "/BuildRequest.xml");
            Console.WriteLine(Dir + "/BuildRequest.xml");

            TestRequest parser = new TestRequest();
            parsed = parser.parseXML(Dir, "BuildRequest.xml");
            foreach (Test test in parsed.test)
            {
                fileName.Add(test.testDriver);
                foreach (string file in test.testedFiles)
                {
                    fileName.Add(file);
                }
            }
            return parsed;
        }

        //Create test request
        string CreateTestReq(TestRequest testBuild)
        {
            TestRequest testReq = new TestRequest();
            int i = 0;
            foreach (Test test in testBuild.test)
            {
                if (status[i])
                {
                    Test testC = new Test();
                    string testD = test.testDriver;
                    string[] libName = testD.Split('.');
                    string name = libName[0] + ".dll";
                    testC.testedFiles.Add(name);
                    testC.testDriver = testD;
                    testReq.test.Add(testC);
                    sendLibToRepo(name);
               }
               i++;
            }
            testReq.author = "Jim Fawcett";
            testReq.dateTime = DateTime.Now.ToString();
            string file = "TestRequest" + random.Next(1, 10000).ToString() + ".xml";
            string savePath = Dir;
            testReq.createXML(testReq, file, Dir);
            return file;
        }

        //send Library to Repo
        void sendLibToRepo(string name)
        {
            bool transferSuccess = c.postFile(name, Dir, RepoPath);
        }

        //Send Test request to Test Harness
        void SendTestRequest(string testReq, string childAddr)
        {
            string path = Path.Combine(Dir, testReq);
            Console.WriteLine(path);
            var xDocument = XDocument.Load(path);
            string xml = xDocument.ToString();
            Console.WriteLine(xml);

            CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.testRequest);
            csndMsg1.command = "show";
            csndMsg1.author = "Jim Fawcett";
            csndMsg1.to = TestAddr;
            csndMsg1.from = childAddr;
            csndMsg1.body = xml;
            c.postMessage(csndMsg1);
        }

#if (TEST_CHILD)
        static void Main(string[] args)
        {
            
            int port;
            if (args.Count() == 0)
            {
                Console.Title = "Child Builder";
                Console.Write("\n  please enter integer value on command line");
                return;
            }
            else
            {
                Console.Title = "Child Builder";
                port = Int32.Parse(args[0]);
                ChildBuilder child = new ChildBuilder(port);
            }

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
#endif
    }
}

