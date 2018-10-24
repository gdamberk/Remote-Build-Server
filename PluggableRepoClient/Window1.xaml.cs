///////////////////////////////////////////////////////////////////////
// Window1.xaml.cs - Client prototype GUI for Pluggable Repository   //
// Language:    C#, Visual Studio 2017                              //
// Platform:     Windows 7                                          //
// Application: Build Server                                        //
//                                                                  //
// Author Name : Dr. Jim Fawcett                                          //
// CSE681: Software Modeling and Analysis, Fall 2017                          //
///////////////////////////////////////////////////////////////////////
/*  
 *  Purpose:
 *    Prototype for a secondary popup window for the Pluggable Repository Client.  
 *    This application doesn't connect to the repository - it has no Communication 
 *    facility.  It simply explores kinds of user interface elements needed for that.
 *
 *  Required Files:
 *    MainWindow.xaml, MainWindow.xaml.cs - view into repository and checkin/checkout
 *    Window1.xaml, Window1.xaml.cs       - Code and MetaData view for individual packages
 *
 *
 *  Maintenance History:
 *    ver 1.0 : 15 Jun 2017
 *    - first release
 */

using System.Windows;

namespace PluggableRepoClient
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    private static double leftOffset = 500.0;
    private static double topOffset = -20.0;
        
   //Constructor
    public Window1()
    {
      InitializeComponent();
      double Left = Application.Current.MainWindow.Left;
      double Top = Application.Current.MainWindow.Top;
      this.Left = Left + leftOffset;
      this.Top = Top + topOffset;
      this.Width = 600.0;
      this.Height = 800.0;
      leftOffset += 20.0;
      topOffset += 20.0;
      if (leftOffset > 700.0)
        leftOffset = 500.0;
      if (topOffset > 180.0)
        topOffset = -20.0;
    }

   
 //Close the pop up 
    private void exitButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
