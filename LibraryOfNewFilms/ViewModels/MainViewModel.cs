using BLL.DTO;
using BLL.Services;
using DAL.Context;
using DAL.Repository;
using LibraryOfNewFilm.Infrastructure;
using LibraryOfNewFilms.Infrastructure;
using LibraryOfNewFilms.Models;
using LibraryOfNewFilms.View;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LibraryOfNewFilms.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged, INavigate
    {
        public ICommand CloseCommand { get; set; }
        public ICommand ReturnToTopCommand { get; set; }
        public ICommand UserMenuOpen { get; set; }
        public ICommand NotificationsConmmand { get; set; }
        UserControl control;
        public UserControl Control
        {
            get => control;
            set
            {
                control = value;
                NotifyPropertyChanged();
            }
        }
        BitmapImage logoImage;
        public BitmapImage LogoImage
        {
            get => logoImage;
            set
            {
                logoImage = value;
                NotifyPropertyChanged();
            }
        }
        BitmapImage userImage;
        public BitmapImage UserImage
        {
            get => userImage;
            set
            {
                userImage = value;
                NotifyPropertyChanged();
            }
        }
        BitmapImage kolokolImage;
        public BitmapImage KolokolImage
        {
            get => kolokolImage;
            set
            {
                kolokolImage = value;
                NotifyPropertyChanged();
            }
        }
        BitmapImage exitImage;
        public BitmapImage ExitImage
        {
            get => exitImage;
            set
            {
                exitImage = value;
                NotifyPropertyChanged();
            }
        }
        Visibility notificationsVisible;
        public Visibility NotificationsVisible
        {
            get => notificationsVisible;
            set
            {
                notificationsVisible = value;
                NotifyPropertyChanged();
            }
        }
        int countOfPiblishedFilm;
        public int CountOfPiblishedFilm
        {
            get => countOfPiblishedFilm;
            set
            {
                countOfPiblishedFilm = value;
                NotifyPropertyChanged();
            }
        }
        public MainViewModel()
        {
            NotificationsVisible = Visibility.Hidden;            
            GenreRepository gr = new GenreRepository(new LibraryContext());
            if(gr.Get(0) == null)
                StartDB.Start();
            string localPath = Directory.GetCurrentDirectory() + "\\Images\\";
            LogoImage = new BitmapImage(new Uri(localPath + "Logo4.png"));
            UserImage = new BitmapImage(new Uri(localPath + "UserImage.png"));
            KolokolImage = new BitmapImage(new Uri(localPath + "Kolokolchik.png"));
            ExitImage = new BitmapImage(new Uri(localPath + "Exit.png"));
            Switcher.Content = this;
            Switcher.Switch(new MainPageView());
            LoginAndRegistrationViewModel.CompleteLogined += UserLogined;
            UserProfileViewModel.LogOutEvent += UserLogOut;
            InitCommand();
        }

        private void InitCommand()
        {
            CloseCommand = new RelayCommand(x => ((MainView)x).Close());
            ReturnToTopCommand = new RelayCommand(x => Switcher.Switch(new MainPageView()));
            UserMenuOpen = new RelayCommand(x =>
            {
                if (LoginAndRegistrationViewModel.IsLogin)
                    Switcher.Switch(new UserProfileView());
                else
                    Switcher.Switch(new LoginAndRegistrationView());
            });
            NotificationsConmmand = new RelayCommand(x =>
            {
                if (LoginAndRegistrationViewModel.IsLogin)
                {
                    MainPageView view = new MainPageView();
                    MainPageViewModel vm = (MainPageViewModel)view.DataContext;
                    vm.SetToFavoriteFilm();
                    Switcher.Switch(view);
                }
            });
        }

        public void Navigate(UserControl control)
        {
            Control = control;
        }
        void UserLogined()
        {
            foreach(Film film in UserProfileViewModel.UserDto.FavoriteFilms)
            {
                if (film.DateOfPubliced < DateTime.Now)
                    CountOfPiblishedFilm++;
            }
            if (CountOfPiblishedFilm > 0)
                NotificationsVisible = Visibility.Visible;
        }
        void UserLogOut()
        {
            NotificationsVisible = Visibility.Collapsed;
            CountOfPiblishedFilm = 0;
        }
    }
}
