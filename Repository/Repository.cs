/////////////////////////////////////////////////////////////////////
// Repository.cs - Demonstrate application use of channel          //
// ver 1.0                                                         //
// Language:    C#, Visual Studio 2017                             //
// Platform:    Windows 7                                          //
// Application: Build Server                                       //
//                                                                 //
// Author Name :Gauri Amberkar                                    //
// Source: Dr. Jim Fawcett                                         //
// CSE681: Software Modeling and Analysis, Fall 2017               //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * The this package defines one class, Repository, that uses the Comm
 * class to pass messages to a Mother Builder and Child Builder
 * 
 * Public Interface:
 * ================= 
 * Repository() :  Constructor which Start the messsage passing 
 * void recieveMessages() : Recieve Messages using WCF
 * 
 * Build Process:
 * --------------
 * Required Files:  MessagePassingComm.cs
 * Build Command: csc Repository.cs MessagePassingComm.cs
 *
 * Maintenance History:
 * --------------------
 * Ver 1.0 : 10 Nov 2016
 * - first release 
 *  
 */
using MessagePassingComm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Repository
{
    public class Repository
    {
        String RepoAddr = "http://localhost:8080/IPluggableComm";
        String MotherAddr = "http://localhost:8081/IPluggableComm";
        Comm c = new Comm("http://localhost", 8080);

        //Constructor
        Repository()
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "show";
            csndMsg.author = "Jim Fawcett";
            csndMsg.to = MotherAddr;
            csndMsg.from = RepoAddr;
            c.postMessage(csndMsg);

            List<string> names = TestPCommService.getClientFileList(ClientEnvironment.fileStorage, "BuildRequest*");
            foreach (string name in names)
            {
                Console.WriteLine(name);
                string path = Path.Combine(ClientEnvironment.fileStorage, name);
                Console.WriteLine(path);
                var xDocument = XDocument.Load(path);
                string xml = xDocument.ToString();
                Console.WriteLine(xml);

                CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.testRequest);
                csndMsg1.command = "show";
                csndMsg1.author = "Jim Fawcett";
                csndMsg1.to = MotherAddr;
                csndMsg1.from = RepoAddr;
                csndMsg1.body = xml;
                c.postMessage(csndMsg1);
            }
            recieveMessages();
        }

        //Recieve Messages using WCF
        void recieveMessages()
        {
            while (true)
            {
                CommMessage msg = c.rcvr.getMessage();
                Console.WriteLine("==================================");
                msg.show();
                if (msg.type == CommMessage.MessageType.fileReq)
                {
                    String body = msg.body;
                    string[] path = body.Split('|');
                    bool transferSuccess = c.postFile(path[0], ClientEnvironment.fileStorage, path[1]);
                }
                else if (msg.type == CommMessage.MessageType.fileList)
                {
                    String body = msg.body;
                    string[] path = body.Split('|');
                    List<string> names = TestPCommService.getClientFileList(ClientEnvironment.fileStorage, path[0]);
                    foreach (string name in names)
                    {
                        bool transferSuccess = c.postFile(name, ClientEnvironment.fileStorage, path[1]);
                    }
                }
                else if(msg.type == CommMessage.MessageType.file)
                {   CommMessage MotherMsg = new CommMessage(CommMessage.MessageType.reply);
                    MotherMsg.command = "show";
                    MotherMsg.author = "Jim Fawcett";
                    MotherMsg.to = msg.from;
                    MotherMsg.from = RepoAddr;
                    MotherMsg.body = "File Sent";
                    c.postMessage(MotherMsg);
                }
                else if (msg.type == CommMessage.MessageType.testRequest)
                {
                    CommMessage MotherMsg = new CommMessage(CommMessage.MessageType.testRequest);
                    MotherMsg.command = "show";
                    MotherMsg.author = "Jim Fawcett";
                    MotherMsg.to = MotherAddr;
                    MotherMsg.from = RepoAddr;
                    MotherMsg.body = msg.body;
                    c.postMessage(MotherMsg);
                }
            }

        }
#if (TEST_REPO)
        //Main method
        static void Main(string[] args)
        {
            Console.Title = "Repository";
            Repository repo = new Repository();
            Console.Write("\n\n");
        }
#endif
    }
}
