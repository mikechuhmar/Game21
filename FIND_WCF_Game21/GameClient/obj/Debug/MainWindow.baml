   M S B A M L   `   `   `  ����E  AGameClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:GameClient
GameClient  5   	       GameClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:GameClient     5   	   -   5          � Title$  
MainWindow��5   	   $��709.71��6   $	��800��6,   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��276* �6   �� 5      $
��451* �6   �� 5      $	��65* �6   �� 5      $��2* �6   .+�5   
   �� -   5        $��bConnDisconn��5   �   $��Подключиться��6   $
��Left=�6(   $��
379,10,0,0q�6C   $	��Top=�6W   $	��129��6o   $��37��6{   $��1��6�   $��2��6�   �� 5   
   -   5        $��
tbUserName��5   �   $
��Left=�6   $��37��6-   $��	10,10,0,0q�69    �� TextWrapping$
 Wrap=�6L   $%��Имя пользователя��6`   $	��Top=�6x   $	��513��6�   $��2��6�   �� 5   
   -   5        $��lbInfo��5   q   $
��Left=�6   $	��235��6-   $��	10,85,0,0q�6:   $	��Top=�6M   $	��774��6e   $��3��6   �� 5   
   -   5        $��bPrepare��5   �   $��Готов играть��6   $��1��6(   $
��Left=�68   $��276,363,0,0q�6S   $	��Top=�6h   $	��152��6�   $��32��6�   6�   �� 5   
   -   5        $��bReqCard��5   �   $3��-Запросить ещё одну карту��6   $��1��64   $
��Left=�6D   $��276,417,0,0q�6_   $	��Top=�6t   $	��152��6�   $��48��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��
dgUserCard��5   s   $
��Left=�6   $	��325��6.   $��
10,345,0,0q�6;   $	��Top=�6O   $	��173��6g   $��2��6�   5      �� 5   
   -   5        $��lPoints��5   �   $�� ��6   $
��Left=�6   $��231,345,0,0q�66   $	��Top=�6K   $��2��6c   $	��212��6w   6�   �� 5   
   -	   5        $��bStopRequest��5   �   $,��&Закончить набор карт��6   $��1��60   $
��Left=�6@   $��276,487,0,0q�6[   $	��Top=�6p   $	��152��6�   $��53��6�   6�   5      5      