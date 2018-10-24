
//////////////////////////////////////////////////////////////////////
//TestRequest.cs: create and parse test requests                    //
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
 * Description: this package Creates and parses TestRequest XML messages
 *              using XDocument      
 *
 * Public Interface:
 * ================= 
 * void makeRequest() : build XML document that represents a test request 
 * createXML(TestRequest tr, string filename) : Create test request for test harness
 * loadXml(string path) : load TestRequest from XML file
 * saveXml(string path) : save TestRequest to XML file
 * string parse(string propertyName) : parse document for property value 
 * List<Test> parseTest(string propertyName) : Parse the test tag and it's descendants
 * TestRequest parseXML(string savePath, string fileName) : Parse the XML(Test Request)
 *
 * Build Process:
 * --------------
 * Required Files:  Test.cs 
   Build Command: csc TestRequest.cs Test.cs 

 * Maintenance History:
    - Ver 1.0 Oct 2017 
 * --------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Test1;

namespace TestRequest1
{

    public class TestRequest
    {
        public string author { get; set; } = "";
        public string dateTime { get; set; } = "";
        public List<Test> test { get; set; } = new List<Test>();
        public XDocument doc { get; set; } = new XDocument();

        /*----< build XML document that represents a test request >----*/

        public void makeRequest()
        {
            XElement testRequestElem = new XElement("testRequest");
            doc.Add(testRequestElem);

            XElement authorElem = new XElement("author");
            authorElem.Add(author);
            testRequestElem.Add(authorElem);

            XElement dateTimeElem = new XElement("dateTime");
            dateTimeElem.Add(DateTime.Now.ToString());
            testRequestElem.Add(dateTimeElem);

            foreach (Test t in test)
            {
                XElement testElem = new XElement("test");
                testRequestElem.Add(testElem);

                XElement driverElem = new XElement("testDriver");
                driverElem.Add(t.testDriver);
                testElem.Add(driverElem);

                foreach (string file in t.testedFiles)
                {
                    XElement testedElem = new XElement("tested");
                    testedElem.Add(file);
                    testElem.Add(testedElem);

                }
            }
        }

        //Create test request for test harness
        public void createXML(TestRequest tr, string filename, string savePath)
        {
            Console.Write(" \nCreating Test Request \n\n");
            Console.Write(" ============================== \n\n");

            if (!System.IO.Directory.Exists(savePath))
                System.IO.Directory.CreateDirectory(savePath);
            string fileSpec = System.IO.Path.Combine(savePath, filename);
            fileSpec = System.IO.Path.GetFullPath(fileSpec);

            tr.makeRequest();
            tr.saveXml(fileSpec);
        }
        /*----< load TestRequest from XML file >-----------------------*/

        public bool loadXml(string path)

        {
            try
            {
                doc = XDocument.Load(path);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--\n", ex.Message);
                return false;
            }
        }
        /*----< save TestRequest to XML file >-------------------------*/

        public bool saveXml(string path)
        {
            try
            {
                doc.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--\n", ex.Message);
                return false;
            }
        }
        /*----< parse document for property value >--------------------*/

        public string parse(string propertyName)
        {

            string parseStr = doc.Descendants(propertyName).First().Value;

            if (parseStr.Length > 0)
            {
                switch (propertyName)
                {
                    case "author":
                        author = parseStr;
                        break;
                    case "dateTime":
                        dateTime = parseStr;
                        break;
                    default:
                        break;
                }
                return parseStr;
            }
            return "";
        }
        /*----< parse document for property list >---------------------*/
        /*
        * - now, there is only one property list for tested files
        */
        public List<string> parseList(string propertyName)
        {
            List<string> values = new List<string>();

            IEnumerable<XElement> parseElems = doc.Descendants(propertyName);

            if (parseElems.Count() > 0)
            {
                switch (propertyName)
                {
                    case "tested":
                        foreach (XElement elem in parseElems)
                        {
                            values.Add(elem.Value);
                        }
                        break;
                    default:
                        break;
                }
            }
            return values;
        }

        //Parse the test tag and it's descendants
        public List<Test> parseTest(string propertyName)
        {
            List<Test> values = new List<Test>();

            IEnumerable<XElement> parseElems = doc.Root.Descendants(propertyName);

            foreach (XElement elem in parseElems)
            {
                Console.WriteLine("\n\n*************************");

                IEnumerable<XElement> property = elem.Descendants();
                Test t = new Test();
                foreach (XElement prop in property)
                {
                    switch (prop.Name.ToString())
                    {
                        case "tested":
                            t.testedFiles.Add(prop.Value);
                            break;
                        case "testDriver":
                            t.testDriver = prop.Value;
                            Console.Write("\n  testDriver is \"{0}\"", t.testDriver);
                            break;
                        case "testProject":
                            t.testProject = prop.Value;
                            Console.Write("\n  testProject is \"{0}\"", t.testProject);
                            break;
                        default:
                            break;
                    }
                }
                Console.Write("\n  testedFiles are:");
                foreach (string file in t.testedFiles)
                {
                    Console.Write("\n    \"{0}\"", file);
                }
                values.Add(t);
            }
            return values;
        }

        //Parse the XML(Test Request)
        public TestRequest parseXML(string savePath, string fileName)
        {
            Console.Write("\n Parsing TestRequest");
            Console.Write("\n =====================");

            if (!System.IO.Directory.Exists(savePath))
                System.IO.Directory.CreateDirectory(savePath);
            string fileSpec = System.IO.Path.Combine(savePath, fileName);
            fileSpec = System.IO.Path.GetFullPath(fileSpec);

            Console.Write("\n reading from \"{0}\"", fileSpec);

            TestRequest tr2 = new TestRequest();
            tr2.loadXml(fileSpec);

            tr2.parse("author");
            Console.Write("\n author is \"{0}\"", tr2.author);

            tr2.parse("dateTime");
            Console.Write("\n dateTime is \"{0}\"", tr2.dateTime);

            tr2.test.AddRange(tr2.parseTest("test"));

            Console.Write("\n\n");
            return tr2;
        }

#if (TEST_REQMOCK)
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstration of Test Request Mock");
            Console.Write("\n ============================");

            //Parse the Test Request           
            TestRequest parser = new TestRequest();
            TestRequest parsed = new TestRequest();
            parsed = parser.parseXML("../../RepoStorage/", "TestRequest1.xml");
            string savePath = "../../../RepoStorage";
            //Create test Request
            TestRequest testReq = new TestRequest();
            testReq.createXML(parsed, "TestRequest1001.xml", savePath);
        }
#endif
    }
}



