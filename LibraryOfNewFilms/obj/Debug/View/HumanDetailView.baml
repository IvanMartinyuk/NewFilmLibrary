   M S B A M L   `   `   `  ????L  HLibraryOfNewFilms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:LibraryOfNewFilms.ViewLibraryOfNewFilms.View  5      K*clr-namespace:LibraryOfNewFilms.ViewModelsLibraryOfNewFilms.ViewModels  5      ,    &LibraryOfNewFilms.View.HumanDetailView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:LibraryOfNewFilms.View     5      9vm*clr-namespace:LibraryOfNewFilms.ViewModels     5        ? DataContext  5
      7   1LibraryOfNewFilms.ViewModels.HumanDetailViewModel  5   
   6$   .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $??7* ?6   6,   ?? 5      $	??18* ?6   6-   .+?5   
   ?? $??0??6   ??5      Z? .+?5      ?? $	??300??5      $	??190??5      $??10 20q?5      ??5      ?? *Human.Image+5   %   ?? 5      
???????$
??Bold"?5      $??Italic$?5      $??20(?5      ??5      ?? *
Human.Name+??5      Z? 5   (   5      ?? 5   
   $??1??6   ??5       Z? .+?5!      ?? $??20(?6   
?????????5"      Z? ??5$      Z? .??6+   Films62   ?? 5%      
???????$??5??5&      6%   ?? 5'      $??40??5*      ??5'      Z? ??5(      Z? ??5)      ?? *Human.Films+ ?? SelectedItem 5+      ?? *SelectedFilm+??5,      ?? 5-      .R?5.      ? $	??550??6$   65   5/      ??51      ?? 52      .R?53      ?? 
?????????6    ?? *
FilmName+54   5   55      57      ?? 58      $??20(?6   
?????????59      Z? ??5;      Z? .??6+   Small biography6<   ?? 5<      
???????$??5??5=      6%   ?? 5>       ?? 
IsReadOnly .  ?? TextWrapping$
 Wrap=?5?       ?? AcceptsReturn . " ?? VerticalScrollBarVisibility$
 Auto=?5A      
???????$	??300??5F      ??5B      Z? ??5C      ?? *Human.SmallInfo+??5E      Z? 5F      5G      5H      5I      