   M S B A M L   `   `   `  ����E  APresto.SDK, Version=1.1.2.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:Presto.SDK;assembly=Presto.SDK
Presto.SDK  5      O KPresto.SWCamp.Lyrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Presto.SWCamp.LyricsPresto.SWCamp.Lyrics 5      '   !Presto.SWCamp.Lyrics.LyricsWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ;ps,clr-namespace:Presto.SDK;assembly=Presto.SDK     5      4local"clr-namespace:Presto.SWCamp.Lyrics     5        � Title$  플레이어 가사 창��5	      $	��750��5
      $	��400��5      $	��750��5      $	��400��5      ���	#FF148CFF   Presto.SDK.PrestoSDK   PrestoService � DataContext# Z 5      $��Center=�5       � WindowStartupLocation$ CenterOwner=�63   .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��* �6   �� 5      $��20 �6   �� 5      $��20 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$
 tbk1��5      $��CBNU SW CAMP Project��5       $��1��5!      $��Center=�5"      $��Center=�5#      $
��Wrap=�5$      $��16(�5%      $
��Bold"�5&      
������� J� Opacity$ .6Q�5(       �� TextAlignment$ Center=�5)      �� 5*   
   -   5        $
 tbk2��5,      $'��!Presto Player 가사 프로그램��5+      $��2��5-      $��Center=�5.      $��Center=�5/      $
��Wrap=�50      $��26(�51      $
��Bold"�52      
�������$ Center=�54      	 J� Effect	 55      3  -System.Windows.Media.Effects.DropShadowEffect  56      
   ShadowDepth$
 3Q�57         	Direction$	 330Q�58         Color$ Black��59         Opacity$ .6Q�5:         
BlurRadius$ 4Q�5;      5=      �� 5>   
   -   5        $
 tbk3��5?      $�� ��5@      $��3��5A      $��Center=�5B      $��Center=�5C      $
��Wrap=�5D      $��16(�5E      $
��Bold"�5F      
�������$ .5Q�5H      $ Center=�5I      ]� 5J   
   $��5��6   $��Stretch=�6#   $��2��6A   $��Center=�6R   
�������.+�5K      �� -   5         ��Name$ 	btnSyncUp��5K      $��가사 0.1초 빠르게��6(   $	��120��6>   $��20��6J   $��Right=�6V   6m   �� 5L      -   5        $ 	btnSyncDn��5L      $��가사 0.1초 느리게��6(   $	��120��6>   $��20��6J   $��Right=�6V   6m   �� 5M      -   5        $ 
btnSyncRst��5M      $��가사 싱크 리셋��6)   $	��100��6<   $��20��6H   $��Right=�6T   6k   �� 5N      -   5        $ 
btnLoadLrc��5N      $��.lrc 불러오기��6)   $��80��6=   $��20��6H   $��Right=�6T   6k   �� 5O      -   5        $ tbkSync��5O      $��20��6)   $
��Left=�65   $��11(�6P   $��2q�6^   5P      5R      5S      