   M S B A M L   `   `   `  ����N  JPluggableRepoClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:PluggableRepoClientPluggableRepoClient  5   	   $    PluggableRepoClient.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:PluggableRepoClient     5   	   -   5          � Title$  Client��5
   	   $	��650��6   $	��500��6%    � 	Resources 5       �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� (    '�� �    '�� R    '�     '�� X    �� 5'   
   " ��6   .�5(      ��  � ��
 ��  �6+   �� 5)       � ��$
 Bold"�6+   �� 5*       � ��$ 14(�6)   5+      �� 5,   
   " ��6   .�5-      ��  � ��
 ��  �6+   �� 5.       � ��$
 Bold"�6+   �� 5/       � ��$ 14(�6)   50      �� 51   
   " ��6   .�52      ��  � ��
 ��  �6+   �� 53       � ��$
 Bold"�6+   �� 54       � ��$ 16(�6)   55      �� 56   
   " ��6   .�57      ��  � ��
 ��  �6+   �� 58       � ��$ Medium"�6+   �� 59       � ��$ 14(�6)   5:      �� 5;   
   " ��6   .�5<      ��  � ��
 ��  �6+   �� 5=       � ��
 �����6+   �� 5>       � ��$ Medium"�6+   �� 5?       � ��$ 16(�6)   5@      �� 5A   
   " �6   .�5B      ��  � ��
 �����6+   5C      �� 5D   
   " ��6   .�5E      ��  � ��
 �����6+   5F      5G      .��5H      �  � RowDefinitions 5I   
   �� 5J      $��* �6   6)   �� 5K      $��25 �6   6*   .+�5M   
   �� -   5        $��tabCntrl��5M   "   $��0��6    �� TabStripPlacement$	 Top=�62   $��Tahoma)�6J   $��Medium"�6^   $��12(�6s   .@�5N      �� -   5        $��navTab��5N      $��
Navigation��6$   .��5O      � $��10q�6    � ColumnDefinitions 5P      �� 5Q      $	��43* �6+   �� 5R      $
��423* �6+    5T      �� 5U      $��25 �6(   66   �� 5V      $��25 �6(   66   �� 5W      $
��283* �6(   68   �� 5X      $��40 �6(   66   .+�5Z      �� -   5        $��RepoReqt��5Z      $��
91,0,133,0q�6J   $��0��6^   $��1��6l   .��6|   Get Files From Repository6�   �� 5[      $��1��6!    �� Orientation$ 
Horizontal=�6.   $��
26,0,189,0q�6H   $��1��6\   .+�5\      �� -   5        $��	testLabel��5\   "   $��3q�63   $��0q�6?   $��98��6S   .��6^   Test Driver6k   5]      �� 5^      $��1��6!   $ 
Horizontal=�6.   $��2��6G   $��
280,0,30,0q�6[   .+�5_      �� -   5        $��	testFiles��5_   "   $��3q�63   $��0q�6?   $��92��6S   .��6^   
Test Files6j   5`      �� 5a      -   5        $��driverListBox��5a   +   $��2��6   $��5q�6@   $��	0,0,237,0q�6   $��2��6�   �� 5b      -   5        $��filesListBox��5b   +   $��2��6   $��5q�6?   	 �� SelectionMode$	 Multiple=�6~   $��	191,0,0,0q�6�   $��1��6�   �� 5c      -	   5        $��AddTest��5c      $��26,15,292,0q�6H   $��3��6]   $��1��6j   .��6z   
Add Test6�   �� 5d      -
   5        $��TestReqt��5d      $��234,15,64,0q�6O   $��3��6d   {�.  $��1��6�   .��6�   Create Test Request6�   5e      5f      �� 5g      -   5        $��TestReq��5g      $��Build Request��6%   .��5h      � $��10q�6    5i      �� 5j      $��9* �6+   �� 5k      $	��10* �6+   �� 5l      $	��17* �6+   �� 5m      $
��196* �6+   �� 5n      $	��85* �6+   �� 5o      $
��149* �6+    5q      �� 5r      $��25 �6(   66   �� 5s      $��2* �6(   66   �� 5t      $��25 �6(   66   �� 5u      $
��1.2* �6(   68   �� 5v      $��40 �6(   66   .+�5x      �� $��0��6!   $ 
Horizontal=�6.   $��Center=�6G   $��3��6d   $��107,0,143,0q�6t   $��3��6�   .+�5y      �� -   5        $��filesLabel2��5y   "   $��3q�65   $��0q�6A   $	��180��6U   .��6a   Select the Build Request6{   5z      �� 5{      -   5        $��testReqListBox��5{   +   $��1��6   $��5q�6A   $��6��6�   $��0,0,0,10q�6�   $��3��6�   6�   �� 5|      $��4��6!   $ 
Horizontal=�6.   $��108,10,142,10q�6G   $��Center=�6^   $��3��6{   $��3��6�   .+�5}      �� -   5        $
��Send��5}   !   $	��180��6-   $��20��6R   $	��Top=�6^   .��6v   Build6}   5~      5      5�      �� 5�      -   5        $
��Logs��5�      $��	Log Files��6"   .��5�      � $��10q�6    5�      �� 5�      $��9* �6+   �� 5�      $	��10* �6+   �� 5�      $	��17* �6+   �� 5�      $
��196* �6+   �� 5�      $
��137* �6+   �� 5�      $	��97* �6+    5�      �� 5�      $��25 �6(   66   �� 5�      $��2* �6(   66   �� 5�      $��25 �6(   66   �� 5�      $
��1.2* �6(   68   �� 5�      $��40 �6(   66   .+�5�      �� $��0��6!   $ 
Horizontal=�6.   $��Center=�6G   $��3��6d   $��71,0,4,0q�6t   $��2��6�   $	��258��6�   .+�5�      �� -   5        $��filesLabel3��5�   "   $��1q�65   $��0q�6A   $	��258��6U   .��6b   #!Double click to to open log files6�   5�      �� 5�      -   5        $��loglist��5�   +   $��1��6   $��5q�6:   $��6��6s   $��0,0,0,10q�6�   $��3��6�   6�   �� 5�      $��4��6!   $ 
Horizontal=�6.   $��108,10,142,10q�6G   $��Center=�6^   $��3��6{   $��3��6�   .+�5�      �� -   5        $��Refresh��5�   !   $	��180��60   $��20��6X   $	��Top=�6d   .��6|   	Refresh6�   5�      5�      5�      �� 5�      -   5        $��builder��5�      $��Mother Builder��6%   .��5�      � $��	0,54,2,39q�6    5�      �� 5�      $
��175* �6+   �� 5�      $	��13* �6+   �� 5�      $	��59* �6+   �� 5�      $	��81* �6+   �� 5�      $
��103* �6+   �� 5�      $	��54* �6+   .+�5�      �� -   5        $��instruction1��5�      $��6��60   $��0,-52,31.4,0q�6D   
�������$p��j*Kindly manually close open Mother Builder and Child Builder
consoles before clicking on the Start button��6r   �� 5�      -   5        $��child��5�      
 J� RenderTransformOrigin$
 
0.508,2.62&�6)   $��
0,30,0,317q�6L   $��6��6`   .��6t   Enter Number of child Builder6�   �� 5�      -   5        $��childNum��5�      $��3q�6.   $��1q�6:   $��0,30,31,400q�6N   
�������$��3��6w   $��3��6�   �� 5�      -   5        $��Start��5�      $��69,90,38,350q�6E   $��3��6[   .��6o   Start6v   �� 5�      -   5        $
��stop��5�      $��25,90,9,350q�6D   $��3��6Y   $��2��6i   .��6~   Stop6�   5�      5�      5�      �� 5�   
   $��1��6   $��25��6!   $��0q�6-   .@�5�      �� -   5        $��status��5�      .��5�      �� -   5        $��statusLabel��5�      	Status:68   5�      5�      5�      5�      