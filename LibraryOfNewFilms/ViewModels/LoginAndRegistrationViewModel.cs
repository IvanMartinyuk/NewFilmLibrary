using BLL.DTO;
using BLL.Services;
using DAL.Context;
using DAL.Repository;
using GalaSoft.MvvmLight.Command;
using LibraryOfNewFilm.Infrastructure;
using LibraryOfNewFilms.Infrastructure;
using LibraryOfNewFilms.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryOfNewFilms.ViewModels
{
    public class LoginAndRegistrationViewModel : BaseNotifyPropertyChanged
    {
        public static bool IsLogin = false;
        Visibility registrationVisibly;
        UserService us;
        public delegate void Logined();
        public static event Logined CompleteLogined;
        public Visibility RegistrationVisibly
        {
            get => registrationVisibly;
            set
            {
                registrationVisibly = value;
                NotifyPropertyChanged();
            }
        }
        public LoginAndRegistrationViewModel()
        {
            us = new UserService(new UserRepository(new LibraryContext()));
            RegistrationVisibly = Visibility.Collapsed;
        }
        string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyPropertyChanged();
            }
        }
        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
        string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                NotifyPropertyChanged();
            }
        }
        string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyPropertyChanged();
            }
        }
        Brush textColor;
        public Brush TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand RegistrationCommand
        {
            get => new RelayCommand<object>(Registration);
        }
        public void Registration(object obj)
        {
            if (RegistrationVisibly == Visibility.Collapsed)
            {
                RegistrationVisibly = Visibility.Visible;
                return;
            }
            if (Login == "" || Password == "")
            {
                Text = "Enter your username or password!";
                TextColor = Brushes.Red;
                return;
            }
            Password = ((PasswordBox)obj).Password;
            ((PasswordBox)obj).Password = "";
            UserDTO user = new UserDTO()
            {
                UserName = Name,
                UserLogin = Login,
                UserPassword = Password.GetHashCode().ToString()
            };            
            foreach(UserDTO sr in us.GetAll())
            {
                if (sr.UserLogin == user.UserLogin)
                {
                    Text = "Data base have this login!";
                    TextColor = Brushes.Red;
                    return;
                }                
            }
            us.AddOrUpdate(user);
            Text = "You are registered";
            TextColor = Brushes.Green;
            ResetLogin();
        }
        void ResetLogin()
        {
            Name = "";
            Login = "";
            RegistrationVisibly = Visibility.Collapsed;
        }
        public ICommand LoginCommand
        {
            get => new RelayCommand<object>(CompleteLogin);
        }
        public void CompleteLogin(object obj)
        {
            Password = ((PasswordBox)obj).Password;
            foreach(UserDTO sr in us.GetAll())
            {
                if(sr.UserLogin == Login && sr.UserPassword == Password.GetHashCode().ToString())
                {
                    IsLogin = true;
                    UserProfileViewModel.UserDto = sr;
                    CompleteLogined?.Invoke();
                    UserProfileView view = new UserProfileView();
                    Switcher.Switch(view);
                }
            }
            if(!IsLogin)
            {
                Text = "Incorrect login or password!";
                TextColor = Brushes.Red;
            }
            
        }
    }
}
