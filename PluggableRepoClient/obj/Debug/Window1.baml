   M S B A M L   `   `   `  ����N  JPluggableRepoClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:PluggableRepoClientPluggableRepoClient  5   	   !    PluggableRepoClient.Window1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:PluggableRepoClient     5   	     � Title$  Code Window��5   	   $	��500��6   $	��400��6*    � 	Resources 5       �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� (    '� �    '��     �� 5%   
   " ��6   .�5&      ��  � ��
 ��  �6+   �� 5'       � ��$
 Bold"�6+   �� 5(       � ��$ 14(�6)   5)      �� 5*   
   " ��6   .�5+      ��  � ��
 ��  �6+   �� 5,       � ��$
 Bold"�6+   �� 5-       � ��$ 14(�6)   5.      �� 5/   
   " ��6   .�50      ��  � ��
 ��  �6+   �� 51       � ��$ Medium"�6+   �� 52       � ��$ 14(�6)   53      �� 54   
   " �6   .�55      ��  � ��
 �����6+   56      �� 57   
   " ��6   .�58      ��  � ��
 ��  �6+   �� 59       � ��$ Medium"�6+   �� 5:       � ��$ 14(�6)   5;      5<      .��5=      � $��10q�6    � RowDefinitions 5>      �� 5?      $��25 �6   6$   �� 5@      $��2* �6   6$   �� 5A      $��25 �6   6$   �� 5B      $
��1.2* �6   6&   �� 5C      $��40 �6   6$   .+�5E      �� -   5        $��	codeLabel��5E      $��0��6    �� TextAlignment$ Center=�6,   $��3q�6C   $��0q�6O   .��6c   	Code View6n   ,� 5F      $��1��6   $��1q�6,   
���   � ,� IsToolBarVisible .  " ,� VerticalScrollBarVisibility$
 Auto=�6m   $��0,0,0,10q�6�   $��3��6�   .��5G      /� -   5        	 �Name$	 codeView��5G      
 /� 
FontFamily$
 Consolas)�6%    /� FontSize$ 12(�6;    /� 
Foreground
 ��  �.T�5H   
   J� -   5        $
	 main��5H      .*�6    .*�5J      this is test�� with second line5L      5M   	   5N      �� 5Q      $��4��6    �� Orientation$ 
Horizontal=�6   $��10q�67   $��Center=�6C   .+�5R      �� -   5        $��
exitButton��5R      $��60��6!   .��6E   Exit6K   5S      5T      5U      