   M S B A M L   `   `   `  ����   DesktopApplication!    DesktopApplication.PageMain   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$  PageMain��.��� $��5q� � ShowGridLines .   � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�� -   $��	SearchBar��$��0�� � ColumnDefinitions �� $
��Auto ��� $��2* ��� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ Vertical=�$
��Left=�$��5q�.+��� -   $��	AddButton��$	��Add�� � ContextMenu �� -   $��contextMenu��.@�m� -   $��选择文件��m� -   $��选择文件夹���� $��5q�$��1��
�������
�������$��1q� �� CornerRadius$ 1��._��� -   $��textblock_filepath��$��Stretch=��� $��2��$��5q��� $��搜索��$��2��$��5q��� $��
NEW FOLDER��$��3��$��5q�� -   $��Mainlist��$��1��$��5q� �� $��* ��� $
��Auto ��� $��2* �.+�R� -   $��leftList��$��0��.@�Q� $��全部文件��.@�Q� $��文档��Q� $��图片��Q� $��视频��Q� $��音乐��Q� $��应用��Q� $��其他���� $��3��$��1��$��Stretch=�$��Center=� �� ShowsPreview .  �� -	   $��
mylistview��$��2�� �� View �� -
   gridView��.P��� $��	文件名��	 �� Width$	 90��
 �� DisplayMemberBinding
 ��  �� Path$ 	File_Path� �� Mode$ OneWay=��� $��2��$��Stretch=�