   M S B A M L   `   `   `  ����  EstimationOfAuthorities(    "EstimationOfAuthorities.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Estymacja autorytetów��5   	   $	��555��6'   $	��800��64    � 
ResizeMode$ NoResize=�6@   .��5      �� .+�5   
   �� $	��0,5q�6    �� Orientation$ 
Horizontal=�6"   .+�5      	� -   5        $��RBEnableEmployeesPanel��5      $
��10,0q�68   $��Widok pracownika��6F   	� 5	      -   5        $��RBEnableCEOPanel��5	      $��130,4,235,0q�62   $��	Widok CEO��6G   �� 5
      -   5        $��NewEvaluation��5
      $!��Rozpocznij nową ewaluację��6*   6N   5      �� 5   
   $	��5,0q�6   � 5   
   -   5        $��MainGrid��5       � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� -   5        $��
CEOChooser��5      $ 
Horizontal=�6+   $��1��6D   .+�5      �� $��
Widok CEO
��6   $
��Left=�61   $��95,7,0,0q�6L   $	��Top=�6^   $��71��6v   5      �� 5      -   5        $��EmployeeChooser��5      $ 
Horizontal=�60   .+�5      �� $��Widok Pracownika��6   $
��Left=�63   $��0,7,0,0q�6N   $	��Top=�6_   $	��120��6w   �� 5      -   5        $��CBEmployees��5      $
��Left=�6.   $��0,10,0,0q�6I   $	��Top=�6[   $	��129��6s   5      �� 5      -	   5        $��EmployeePanel��5      $
��Left=�6.   $��1��6I   $	��Top=�6V   $	��259��6n   $��3��6z   .+�5      �� $	��5,0q�6   �� 5       $��Wybierz współpracownika��6   �� 5!      -
   5        $��ListOfEmployees1��5!      $
��Left=�62   $��93��6M   $��10,0,0,0q�6Y   $	��Top=�6k   $	��238��6�   �� 5"      $��Szczegółowe dane��6   $
��Left=�65   $	��Top=�6P   .��5#      �� .+�5$      �� $ 
Horizontal=�6%   .+�5%      �� $��Wartość dla firmy��6$   $	��191��6@   �� 5&      -   5        $��ValueForCompany��5&   &   $��50��6=   5'      �� 5(      $ 
Horizontal=�6%   .+�5)      �� $��Przepracowny czas��6$   $	��191��6@   �� 5*      -   5        $��
TimeWorked��5*   &   $��50��68   5+      �� 5,      -   5        $��Oceń Pracownika��5,   !   $��5,10,5,5q�6;   $	��Top=�6M   6e   5-      5.      5/      �� 50      -   5        $��CEOPanelAddEmployee��50      $��1��64   $
��Left=�6D   $��1��6_   $	��Top=�6l   $	��259��6�   .+�51      �� $	��5,0q�6   �� 52      $��Dodaj pracownika��6   �� 53      -   5        $��EmployeeName��53      $��23��6.    �� TextWrapping$
 Wrap=�6:   �� 54      $��Data zatrudnienia��6   &   System.Windows.Controls.Calendar  55      -   5        $��	CalendarE��55         SelectionMode$ 
SingleDate=�6,   �� 56      -   5        $��Roles��56      $��Roles��6(   $
��Left=�67   $	��Top=�6R   $	��259��6j   58      �� 5=      -   5        $��AddEditButton��5=      $��DODAJ PRACOWNIKA��6.   $��5q�6I   6T   5>      �� 5?      -   5        $��CEOPanel��5?      $��2��6)   $
��Left=�69   $��1��6T   $	��Top=�6a   $	��259��6y   .+�5@      �� $��Wybierz pracownika��6   $
��Bold"�65   $��14(�6G   �� 5B      -   5         ��Name$ ListOfEmployees2��5B      $
��Left=�64   $��93��6O   $	��Top=�6[   $	��248��6s   6   �� 5C      -   5         ��Name$ DetailsLabel��5C      $��Szczegóły pracownika��6.   $y�Visible=�6M   $
��Bold"�6b   $��14(�6t   �� 5D      $��0q�6   $��19��6'   �� 5E      -   5        	 ��Name$	 EmployeesDetails��5E      $
��Left=�66   $	��220��6Q   $	��Top=�6^   $	��248��6v   $y�Hidden=�6�   5F      5G      5H      5I      