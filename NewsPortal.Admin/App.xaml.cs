using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NewsPortal.Admin.Model;
using NewsPortal.Admin.Persistence;
using NewsPortal.Admin.View;
using NewsPortal.Admin.ViewModel;

namespace NewsPortal.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private INewsPortalModel _model;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private EditorWindow _editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new NewsPortalModel(new NewsPortalServicePersistence("http://localhost:44348/")); // megadjuk a szolgáltatás címét
            /*
            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();
            */
            _mainViewModel = new MainViewModel(_model);
            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            /*
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.BuildingEditingStarted += new EventHandler(MainViewModel_BuildingEditingStarted);
            _mainViewModel.BuildingEditingFinished += new EventHandler(MainViewModel_BuildingEditingFinished);
            _mainViewModel.ImageEditingStarted += new EventHandler<BuildingEventArgs>(MainViewModel_ImageEditingStarted);*/
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();

            _loginView.Close();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn) // amennyiben be vagyunk jelentkezve, kijelentkezünk
            {
                await _model.LogoutAsync();
            }
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            Shutdown();
        }
    }

 
}
