using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Admin.Model;
using NewsPortal.Admin.Persistence;
using NewsPortal.Data;

namespace NewsPortal.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INewsPortalModel _model;
        private ObservableCollection<ArticleListElement> _articles;
        

        public ObservableCollection<ArticleListElement> Articles
        {
            get { return _articles; }
            private set
            {
                if (_articles != value)
                {
                    _articles = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand NewArticleCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }

        public event EventHandler ExitApplication;
        public event EventHandler ArticleEditingStarted;
        public event EventHandler ArticleEditingFinished;

        public MainViewModel(INewsPortalModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            
            LoadCommand = new DelegateCommand(param => LoadAsync());
        }

        

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Articles = new ObservableCollection<ArticleListElement>(_model.ArticleList); // az adatokat egy követett gyűjteménybe helyezzük
               
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }
    }
}
