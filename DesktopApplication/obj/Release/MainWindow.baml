   M S B A M L   `   `   `  ���� M  IWPFVisifire.Charts, Version=5.1.7.0, Culture=neutral, PublicKeyToken=nullM9clr-namespace:Visifire.Charts;assembly=WPFVisifire.ChartsVisifire.Charts   DesktopApplication#   DesktopApplication.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Hvc9clr-namespace:Visifire.Charts;assembly=WPFVisifire.Charts       � Title$  
MainWindow��$	��550��$	��825�� � WindowStartupLocation$ CenterScreen=� � 	Resources    BigFontButton �� Property �� Value �� Triggers P� Property P� Value��%  &	        �� ���  � ��$ Times New Roman)���  � ��$ 18(� P�  �@J�	IsFocused . . ���  � ��
 �  ��P�  �@J�IsMouseOver . . ���  � ��
 � �����  � ��$
 Bold"�P�  � �� . . ���  � ��
 ��  �.���  � RowDefinitions �� $
��Auto ��� $��* �.+��� $��0��	 �� Orientation$	 
Horizontal=�$��Center=�.+��� -   $	��new��$��0,5,0,5q�$��60��$��Center=��� -   $��Message��$��0,5,0,5q�$��Center=�$��60���� $��0��$	 
Horizontal=�$��Right=�.+��� -   $��SiginButton��$��	未登录��$	��100��$��5q�#��[  �� -   $��SettingButton��$��Setting��$��5,5,10,5q�$��50��
 � ContextMenu
 �� -   $��contextMenu��.@�m� $��帮助及反馈��m� $��退出�� � -   $��myFrame��$��1��  � Source$ PageMain.xaml?�  � NavigationUIVisibility$ Hidden=�