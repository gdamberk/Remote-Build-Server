/////////////////////////////////////////////////////////////////////////////////
// Test.cs      : This class is used by TestRequest class                      //
// ver 1.0                                                                     //
//                                                                             //
// Platform     : Visual Studio 2017                                           //
// Application  : CIS681-Software Modeling and Analysis Project Demo           //
// Author       : ver1.0 - Gauri Amberkar, Syracuse University                 //
//                         (gdamberk@syr.edu )                                 //
/////////////////////////////////////////////////////////////////////////////////
/*
 *  Module Operations:
 * -------------------
 * this package contain XML tag test and it's descendants. It is 
 * used TestRequest class while parsing and creating  
 * 
 * Public Interface: None
 * 
 * Build Process:
 * --------------
 * Required Files:  None
   Build Command: csc Test.cs

 * Maintenance History:
    - Ver 1.0 Oct 2017 
 */


using System.Collections.Generic;

namespace Test1
{
    public class Test
    {
        //This class contain Test and descendants of the test tag
        public string testDriver { get; set; } = "";
        public string testProject { get; set; } = "";
        public List<string> testedFiles { get; set; } = new List<string>();

    }
}



