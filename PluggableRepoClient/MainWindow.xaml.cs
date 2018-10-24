/////////////////////////////////////////////////////////////////////////
// MainWindow.xaml.cs - Client prototype GUI for Pluggable Repository  //
// Language:   C#, Visual Studio 2017                                  //
// Platform:   Windows 7                                            //
// Application: Build Server                                        //
//                                                                  //
// Author Name : Gauri Amberkar                                     //
// Source: Dr. Jim Fawcett                                          //
// CSE681: Software Modeling and Analysis, Fall 2017                //
/////////////////////////////////////////////////////////////////////////
/*  
 *  Purpose:
 *    Prototype for a client for the Pluggable Repository.This application
 *    doesn't connect to the repository - it has no Communication facility.
 *    It simply explores the kinds of user interface elements needed for that.
 *
 *  Public Interface:
 * ================= 
 * void initializeFilesListBox() : Initialize the FilesListBox
 * void initializeDriverListBox() : Initialize the driverListBox
 * void initializeTestReqListBox() : Initialize testReqListBox 
 * List<string> getFiles(String dir, String pattern) : Get files from Repository storage
 * MainWindow() : Initialize the component
 * Window_Loaded(object sender, RoutedEventArgs e) : Initialize the methods after window loaded
 * sendButton_Click(object sender, RoutedEventArgs e) : Send test request to Mother Builder
 * StartBuilder_Click(object sender, RoutedEventArgs e) : Start Mother Builder
 * StopBuillder_Click(object sender, RoutedEventArgs e) : stop Mother Builder
 * TestAddButton_Click(object sender, RoutedEventArgs e): Add test in Test Request
 * CreateRequestButton_Click(object sender, RoutedEventArgs e) : Create test request
 * testReqListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) : Open file after double clicking the file
 * showCodeButton_Click(object sender, RoutedEventArgs e) : Open file after double clicking the file
 * driverListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) : Open file after double clicking the file
 * filesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) : Open file after double clicking the file
 * showFile(string fileName, Window1 popUp) : Show file in window pop up
 * Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) : Closing all the pop ups
 * OnClosed(EventArgs e) : Closes application on close
 * void initializeLogList() : Initialize Log file list
 * public List<string> getFiles(String dir, String pattern) : Get files from storage
 * private void GetFilesButton_Click(object sender, RoutedEventArgs e) : Get files from Repo
 * 
 * Required Files:
 *    MainWindow.xaml, MainWindow.xaml.cs - view into repository and checkin/checkout
 *    Window1.xaml, Window1.xaml.cs       - Code and MetaData view for individual packages
 *
 *
 *  Maintenance History:
 *    ver 1.0 : 15 Jun 2017
 *    - first release
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using TestRequest1;
using Test1;
using MessagePassingComm;
using System.Xml.Linq;
using System.Diagnostics;

namespace PluggableRepoClient
{

    public partial class MainWindow : Window
    {

    List<Window1> popups = new List<Window1>();
    List<Test> testList = new List<Test>();
    Comm c = new Comm("http://localhost", 8090);
    List<string> files = new List<string>();
    string storagePath = "../../../RepoStorage";
    string clientStorage = "../../../ClientFileStore";
    Random random = new Random();

        //Initialize the FilesListBox
        void initializeFilesListBox()
    {
        foreach (string file in files)
        {
                if (file.EndsWith(".cs") && !file.Contains("Driver"))
                filesListBox.Items.Add(file);
        }  
        statusLabel.Text = "Action: Show file text by double clicking on file";
    }
        //Initialize the driverListBox
        void initializeDriverListBox()
    {
        foreach (string file in files)
        {
                if (file.EndsWith(".cs") && file.Contains("Driver"))
                driverListBox.Items.Add(file);
        }  
        statusLabel.Text = "Action: Show file text by double clicking on file";
    }

        //Initialize testReqListBox
        void initializeTestReqListBox()
    {
        List<string> fileXml = getFiles(storagePath, "*.xml");
        foreach (string file in fileXml)
        {
                testReqListBox.Items.Add(file);
        }
        
      statusLabel.Text = "Action: Show file text by double clicking on file";
    }

        //Initialize Log file list
    void initializeLogList()
    {
            List<string> fileLog = getFiles(storagePath, "Log*.txt");
            foreach (string file in fileLog)
            {
                loglist.Items.Add(file);
            }

            statusLabel.Text = "Action: Show file text by double clicking on file";
    }

        //Get files from Repository storage
        public List<string> getFiles(String dir, String pattern)
    {

        List<string> names = new List<string>();
        string[] files = Directory.GetFiles(dir, pattern);
        foreach (string file in files)
        { 
            names.Add(System.IO.Path.GetFileName(file));
        }
            return names;
        }
   
        //Initialize the component
    public MainWindow()
    {
      InitializeComponent();
    }
        //Initialize the methods after window loaded
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      files = getFiles(storagePath, "*.*");
      
      startRepository();
      startMotherBuilder();
      startTestHarness();
      
    }

    //Get files from Repo
    private void GetFilesButton_Click(object sender, RoutedEventArgs e)
    {
            CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.fileList);
            csndMsg1.command = "show";
            csndMsg1.author = "Jim Fawcett";
            csndMsg1.to = "http://localhost:8080/IPluggableComm";
            csndMsg1.from = "http://localhost:8090/IPluggableComm";
            csndMsg1.body = "*.*"  + "|" + clientStorage;
            c.postMessage(csndMsg1);

            filesListBox.Items.Clear();
            driverListBox.Items.Clear();
            testReqListBox.Items.Clear();
            loglist.Items.Clear();
            initializeFilesListBox();
            initializeDriverListBox();
            initializeTestReqListBox();
            initializeLogList();
    }
        

        //Refresh to get latest files
    void refreshButton_Click(object sender, RoutedEventArgs e)
    {
            loglist.Items.Clear();
            initializeLogList();
    }
        
    //Send test request to Mother Builder
    void sendButton_Click(object sender, RoutedEventArgs e)
    {
            String file = (string)testReqListBox.SelectedItem;
            string path = System.IO.Path.Combine(storagePath,file );
            var xDocument = XDocument.Load(path);
            string xml = xDocument.ToString();

            CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.testRequest);
            csndMsg1.command = "show";
            csndMsg1.author = "Jim Fawcett";
            csndMsg1.to = "http://localhost:8080/IPluggableComm";
            csndMsg1.from = "http://localhost:8090/IPluggableComm";
            csndMsg1.body = xml;
            c.postMessage(csndMsg1);
            statusLabel.Text = "Status: Send selected Test Request to Builder";
    }
    //Start Mother Builder
    void StartBuilder_Click(object sender, RoutedEventArgs e)
    {
            string fileName = "..\\..\\..\\MotherBuilder\\bin\\debug\\MotherBuilder.exe";
           if (childNum.Text == "")
                statusLabel.Text = "Status: Please enter value between 0-6";
            else
            {
                try
                {
                    if (Convert.ToInt32(childNum.Text) > 6 || Convert.ToInt32(childNum.Text) <= 0)
                        statusLabel.Text = "Status: Status: Please enter value between 0-6 in text box";
                    else
                    {
                        Process.Start(fileName, childNum.Text);
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Invalid number of proceses" + ex;
                }
            }
    }

    //Start Repository while loading GUI
    void startRepository()
    {
            string fileName = "..\\..\\..\\Repository\\bin\\debug\\Repository.exe";
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n  {0}", ex.Message);
            }
    }

    //Start Mother Builder while loading GUI
    void startMotherBuilder()
        {
            string fileName = "..\\..\\..\\MotherBuilder\\bin\\debug\\MotherBuilder.exe";
            try
            {
                Process.Start(fileName, "2");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n  {0}", ex.Message);
            }
        }
    
    //Start Test Harness while loading GUI
    void startTestHarness() 
    {
            string fileName = "..\\..\\..\\TestHarness\\bin\\debug\\TestHarness.exe";
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n  {0}", ex.Message);
            }
    }


        //stop Mother Builder
        void StopBuillder_Click(object sender, RoutedEventArgs e)
    {
            CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.quit);
            csndMsg1.command = "show";
            csndMsg1.author = "Jim Fawcett";
            csndMsg1.to = "http://localhost:8081/IPluggableComm";
            csndMsg1.from = "http://localhost:8090/IPluggableComm";
            c.postMessage(csndMsg1);
            statusLabel.Text = "Status: Stop the Builder";
        }

        //Add test in Test Request
    private void TestAddButton_Click(object sender, RoutedEventArgs e)
    {
           if (driverListBox.SelectedItem == null || filesListBox.SelectedItems.Count == 0)
            {
                statusLabel.Text = "Status: Added Test Driver and Test Files in Test Request";
            }
            else
            {
                Test test = new Test();
                test.testDriver = driverListBox.SelectedItem.ToString();
                foreach( string testFile in filesListBox.SelectedItems)
                {
                    test.testedFiles.Add(testFile);
                }
                testList.Add(test);
                TestReqt.IsEnabled = true;
                driverListBox.UnselectAll();
                filesListBox.UnselectAll();
                statusLabel.Text = "Status: Added Test in Test Request";
            }
    }
     // Create test request
    private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
    {
            TestRequest testReq = new TestRequest(); 
            testReq.test.AddRange(testList);
            testReq.author = "Jim Fawcett";
            testReq.dateTime = DateTime.Now.ToString();
            String file = "BuildRequest" + random.Next(1, 10000).ToString() + ".xml";
            string savePath = storagePath;
            testReq.createXML(testReq, file, savePath);
            TestReqt.IsEnabled = false;
            testReqListBox.Items.Clear();
            testList.Clear();
            initializeTestReqListBox();
            statusLabel.Text = "Status: Test Request "+ file +"  created";
    }

        
    //Open file after double clicking the file
    private void loglist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window1 codePopup = new Window1();
            codePopup.Show();
            popups.Add(codePopup);

            codePopup.codeView.Blocks.Clear();
            string fileName = (string)loglist.SelectedItem;

            codePopup.codeLabel.Text = "Source code: " + fileName;

            showFile(fileName, codePopup);
            return;
        }
        //Open file after double clicking the file
        private void testReqListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
            Window1 codePopup = new Window1();
            codePopup.Show();
            popups.Add(codePopup);

            codePopup.codeView.Blocks.Clear();
            string fileName = (string)testReqListBox.SelectedItem;

            codePopup.codeLabel.Text = "Source code: " + fileName;

            showFile(fileName, codePopup);
            return;
    }

    //Open file after double clicking the file
    private void showCodeButton_Click(object sender, RoutedEventArgs e)
    {
      Window1 codePopup = new Window1();
      codePopup.Show();
      popups.Add(codePopup);
    }

     //Open file after double clicking the file
    private void driverListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
            Window1 codePopup = new Window1();
            codePopup.Show();
            popups.Add(codePopup);

            codePopup.codeView.Blocks.Clear();
            string fileName = (string)driverListBox.SelectedItem;

            codePopup.codeLabel.Text = "Source code: " + fileName;

            showFile(fileName, codePopup);
            return;
    }

     //Open file after double clicking the file
    private void filesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Window1 codePopup = new Window1();
      codePopup.Show();
      popups.Add(codePopup);

      codePopup.codeView.Blocks.Clear();
      string fileName = (string)filesListBox.SelectedItem;

      codePopup.codeLabel.Text = "Source code: " + fileName;
      
      showFile(fileName, codePopup);
      return;
    }

    // Show file in window pop up
    private void showFile(string fileName, Window1 popUp)
    {
            string path = System.IO.Path.Combine(storagePath, fileName);
            Paragraph paragraph = new Paragraph();
            string fileText = "";
            try
            {
                fileText = System.IO.File.ReadAllText(path);
            }
            finally
            {
                paragraph.Inlines.Add(new Run(fileText));
            }

            // add code text to code view
            popUp.codeView.Blocks.Clear();
            popUp.codeView.Blocks.Add(paragraph);
 
        }

        //Closing all the pop ups
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var popup in popups)
                popup.Close();
        }

        //Closes application on close
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }


    }
}
