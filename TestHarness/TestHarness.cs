/////////////////////////////////////////////////////////////////////////////////
// TestHarness.cs : Demonstrate Robust loading and dynamic invocation of       //
//                Dynamic Link Libraries found in specified location           //
// ver 3.0                                                                     //
//                                                                             //
// Platform     : Visual Studio 2017                                           //
// Application  : CSE-681 - BUilder Demo with mock repository and              //
//                mock test harness                                            //
// Author       : ver1.0 & ver2.0 - Jim Fawcett, CST 4-187,                    //
//                jfawcett @twcny.rr.com                                       //
//                ver3.0 - Gauri Amberkar, Syracuse University                 //
//                (gdamberk@syr.edu )                                          //
/////////////////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * -------------------
 * this package demonstrates Robust loading and dynamic invocation of   
 * Dynamic Link Libraries provided in TestRequest2.xml and found in
 * specified location
 * 
 * Public Interface:
 * ================= 
 * TestHarness() : Constructor
 * void recieveMessages() : Recieve Messages using WCF
 * void SendLog(string file) : Used for sending log file to repository
 * TestRequest parseTestRequest() : Parse the test request recieved from Child builder
 * static Assembly LoadFromComponentLibFolder(object sender, ResolveEventArgs args) : library binding error event handler
 * string loadAndExerciseTesters(List<string> library) : load assemblies from testersLocation and run their tests
 * bool runSimulatedTest(Type t, Assembly asm) : run tester t from assembly asm 
 * void runTestHarness(TestRequest testReq) : run demonstration
 * 
 * Build Process:
 * --------------
 * Required Files: TestRequest.cs
   Build Command: csc TestHarness.cs , TestRequest.cs

 * Maintenance History:
    - Ver 1.0 Dec 2017 
 * --------------------*/


using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using TestRequest1;
using Test1;
using MessagePassingComm;
using System.Xml.Linq;
using System.Threading;
using Logger1;

namespace TestHarness
{
    class TestHarness
    {
        static String testRequest;
        static string testersLocation { get; set; } = "../../../TestStorage";
        Comm c;
        string testAddr = "http://localhost:8095/IPluggableComm";
        static String RepoPath = "../../../RepoStorage";
        static List<String> fileName = new List<String>();
        TestRequest parse = null;
        string logfile = null;
        Logger logger;

        //Constructor
        public TestHarness()
        {
            c = new Comm("http://localhost", 8095);
            if (!Directory.Exists(testersLocation))
                Directory.CreateDirectory(testersLocation);
            recieveMessages();
        }

        //Recieve Messages using WCF
        void recieveMessages()
        {
            while (true)
            {
                
                CommMessage msg = c.rcvr.getMessage();
                msg.show();
                if (msg.type == CommMessage.MessageType.testRequest)
                {
                    string logName = "LogTestHarness" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
                    logfile = testersLocation + "/" + logName;
                    this.logger = new Logger(logfile);

                    fileName.Clear();
                    testRequest = msg.body;

                    parse = new TestRequest();
                    parse = parseTestRequest();
                    foreach (string name in fileName)
                    {
                        CommMessage csndMsg2 = new CommMessage(CommMessage.MessageType.fileReq);
                        csndMsg2.command = "show";
                        csndMsg2.author = "Jim Fawcett";
                        csndMsg2.to = msg.from;
                        csndMsg2.from = testAddr;
                        csndMsg2.body = name + "|" + testersLocation;
                        c.postMessage(csndMsg2);
                        Console.WriteLine(name + " file receiving from Child Builder");
                        logger.log(name + " file receiving from Child Builder");
                    }
                    Thread.Sleep(2000);
                    runTestHarness(parse);
                    SendLog(logName);
                }
            }
        }

        // Used for sending log file to repository
        void SendLog(string file)
        {
            bool transferSuccess = c.postFile(file, testersLocation, RepoPath);
            Console.WriteLine("Sending Log file " + file + " to Repository");
        }
        //Parse the test request recieved from Child builder
        TestRequest parseTestRequest()
        {
            TestRequest parsed = new TestRequest();
            XDocument doc = XDocument.Parse(testRequest);
            doc.Save(testersLocation+ "/TestRequest.xml");

            TestRequest parser = new TestRequest();
            parsed = parser.parseXML(testersLocation, "TestRequest.xml");
            foreach (Test test in parsed.test)
            {
                foreach (string file in test.testedFiles)
                {
                    fileName.Add(file);
                }
            }
            return parsed;
        }
        /*----< library binding error event handler >------------------*/
        /*
         *  This function is an event handler for binding errors when
         *  loading libraries.  These occur when a loaded library has
         *  dependent libraries that are not located in the directory
         *  where the Executable is running.
         */
        static Assembly LoadFromComponentLibFolder(object sender, ResolveEventArgs args)
        {
            Console.Write("\n  called binding error event handler");
            string folderPath = testersLocation;
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
        //----< load assemblies from testersLocation and run their tests >-----

        string loadAndExerciseTesters(List<string> library)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromComponentLibFolder);

            try
            {
                // load each assembly found in testersLocation

                foreach (string file in library)
                {
                    string filePath = Path.GetFullPath(testersLocation + "/" + file);

                    Assembly asm = Assembly.Load(File.ReadAllBytes(filePath));
                    string fileName = Path.GetFileName(filePath);
                    Console.Write("\n loaded {0} \n", fileName);
                    logger.log("Loaded file : "  + fileName);
                    // exercise each tester found in assembly

                    Type[] types = asm.GetTypes();
                    foreach (Type t in types)
                    {
                        // if type supports ITest interface then run test
                        string itest = t.ToString();
                        string[] Iname = itest.Split('.');
                        if (t.GetInterface(Iname[0] + ".ITest", true) != null)
                        {
                            if (!runSimulatedTest(t, asm))
                            {
                                Console.Write("\n test {0} failed to run", t.ToString());
                                logger.log("Test " + t.ToString()+" failed to run");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Simulated Testing completed";
        }
        //----< run tester t from assembly asm >-------------------------------

        bool runSimulatedTest(Type t, Assembly asm)
        {
            try
            {
                Console.Write("\n attempting to create instance of {0}", t.ToString());
                object obj = asm.CreateInstance(t.ToString());

                // announce test
                MethodInfo method = t.GetMethod("say");
                if (method != null)
                    method.Invoke(obj, new object[0]);

                // run test

                bool status = false;
                method = t.GetMethod("test");
                if (method != null)
                    status = (bool)method.Invoke(obj, new object[0]);

                Func<bool, string> act = (bool pass) =>
                {
                    if (pass)
                        return "passed";
                    return "failed";
                };
                Console.Write("\n test {0}", act(status));
                Console.WriteLine("\n *********************");
                logger.log("Test Result : " + act(status));
                logger.log("*********************");
            }
            catch (Exception ex)
            {
                Console.Write("\n test failed with message \"{0}\"", ex.Message);
                logger.log("Test failed with message " + ex.Message);
                return false;
            }
            return true;
        }
        
        //----< run demonstration >--------------------------------------------
        void runTestHarness(TestRequest testReq)
        {
            Console.Write("\n\n Test Harness");
            Console.Write("\n\n ==================================\n\n");

            // convert testers relative path to absolute path

            TestHarness.testersLocation = Path.GetFullPath(TestHarness.testersLocation);

            List<string> library = new List<string>();
            foreach (Test test in testReq.test )
            {
                library.AddRange(test.testedFiles);
            }
            // run load and tests
            string result = loadAndExerciseTesters(library);

            logger.log(result);
            Console.Write("\n\n  {0}", result);
            Console.Write("\n\n");
        }

#if (TEST_HARNESS)
        static void Main(string[] args)
        {
            Console.Title = "Test Harness";
            //Test the file in Test Harness 
            
            TestHarness test = new TestHarness();
        }
#endif
    }
}

