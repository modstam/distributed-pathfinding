   M S B A M L   `   `   `  ����R  Ndistributed pathfinding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:distributed_pathfindingdistributed_pathfinding  5   	   (    "distributed_pathfinding.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:distributed_pathfinding     5   	   -   5        $��
mainWindow��5	   	     � Title$  
Simulation��5   	   $	��800��6   $	��800��6*   .��5      � .+�5   
   � $	��0,0q�6   $��Center=�6   $	��Top=�69   $��55��6Q   .+�5      �� -   5         ��Name$ 	numAgents��5      $
��Left=�6)   $��23��6D   $��10,25,140,0q�6P    �� TextWrapping$
 Wrap=�6e   $��	numAgents��6y   $	��Top=�6�   $	��120��6�   6�   �� 5      -   5         ��Name$ startButton��5      $��
Start/Stop��6*   $
��Left=�6?   $��140,25,10,0q�6Z   $	��Top=�6o   $��74��6�   6�   5      � 5   
   $��0,0,0,0q�6   $��Center=�6    $��Center=�6=   .+�5      �� -   5        $��	mapCanvas��5      
�������$��center=�6>   $	��649��6[   $��0,0,0,10q�6h   $��center=�6z   $
��1059��6�   6�   5      � 5   
   $��Center=�6   $��Bottom=�6,   $��55��6G   $��0,10,0,0q�6S   .+�5      �� -   5        $ 	ipTextBox��5      $
��Left=�6)   $��22��6D   $��	24,0,0,21q�6P   $
 Wrap=�6c   $��
Ip Address��6w   $��Bottom=�6�   $	��120��6�   �� 5      -   5        $ networkButton��5      $��Listen��6-   $
��Left=�6>   $��
276,0,0,24q�6Y   $��Bottom=�6m   $��74��6�   6�   �� 5      -   5         ��Name$ modeCheckBox��5      $��
WorkerMode��6-   $
��Left=�6B   $��
163,0,0,28q�6]   $��Bottom=�6q   6�   5      5      5      