using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Admin.Model;
using NewsPortal.Admin.Persistence;
using NewsPortal.Data;
using System.Windows;

namespace NewsPortal.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INewsPortalModel _model;
        private ObservableCollection<ArticleListElement> _articles;
        private ArticleListElement _selectedArticle;
        private PictureDTO _selectedPicture;

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

        public ArticleViewModel ArticleUnderEdit { get; set; }

        public ArticleListElement SelectedArticle
        {
            get { return _selectedArticle; }
            set
            {
                if (_selectedArticle != value)
                {
                    _selectedArticle = value;
                    OnPropertyChanged();
                }
            }
        }

        public PictureDTO SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                if (_selectedPicture != value)
                {
                    _selectedPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand NewArticleCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand DeletePictureCommand { get; private set; }
        public DelegateCommand NewPictureCommand { get; private set; }
        public DelegateCommand CloseEditorCommand { get; private set; }
        public DelegateCommand SaveArticleCommand { get; private set; }

        public event EventHandler ExitApplication;
        public event EventHandler ArticleEditingStarted;
        public event EventHandler ArticleEditingFinished;
        public event EventHandler<ArticleEventArgs> ImageEditingStarted;

        public MainViewModel(INewsPortalModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            
            LoadAsync();
            LoadCommand = new DelegateCommand(param => LoadAsync());
            NewArticleCommand = new DelegateCommand(param => CreateArticle());
            DeleteCommand = new DelegateCommand(param => DeleteArticle(param as ArticleListElement));
            ExitCommand = new DelegateCommand(param => OnExitApplication());
            EditCommand = new DelegateCommand(param => EditArticle(param as ArticleListElement));
            CloseEditorCommand = new DelegateCommand(param => OnArticleEditingFinished());
            SaveArticleCommand = new DelegateCommand(param => SaveArticle());
            NewPictureCommand = new DelegateCommand(param => OnImageEditingStarted());
            DeletePictureCommand = new DelegateCommand(param => DeleteSelectedPicture());
        }

        private void DeleteSelectedPicture()
        {
            if (_selectedPicture != null)
            {               
                ArticleUnderEdit.Pictures.Remove(_selectedPicture);
            }
        }

        private async void SaveArticle()
        {
            if (ArticleUnderEdit == null)
            {
                MessageBox.Show("Nincs menthető cikk.");
            }
            else
            {
                ArticleDTO articleToSave = new ArticleDTO
                {
                    Id = ArticleUnderEdit.Id,
                    Title = ArticleUnderEdit.Title,
                    Summary = ArticleUnderEdit.Summary,
                    Content = ArticleUnderEdit.Content,
                    Lead = ArticleUnderEdit.Lead,
                    UserId = ArticleUnderEdit.UserId,
                    Pictures = ArticleUnderEdit.Pictures,
                    LastModified = DateTime.Now
                };
                try
                {                    
                    if (articleToSave.Id == 0)
                    {
                        await _model.CreateArticleAsync(articleToSave);                       
                    }
                    else
                    {
                        await _model.UpdateArticle(articleToSave);                
                    }
                    OnArticleEditingFinished();
                }
                catch (InvalidFormException ex)
                {
                    MessageBox.Show("Az űrlap helytelenül van kitöltve, így nem lehet menteni.\nHelyes:\n- az összes szöveges mező ki van töltve\n- az összefoglaló nem hosszabb 1000 karakternél\n- vezető cikkhez kötelező legalább egy képet feltölteni", "Hiba");
                }
                catch
                {
                    MessageBox.Show("Sikertelen mentés.", "Hiba");
                }
            }
        }

        
        private async void EditArticle(ArticleListElement article)
        {
            if (article != null && ArticleUnderEdit==null)
                try
                {
                    await _model.LoadArticleAsync(article.Id);
                    ArticleDTO articleDTO = _model.ArticleToEdit;
                    ArticleUnderEdit = new ArticleViewModel
                    {
                        Id = articleDTO.Id,
                        Title = articleDTO.Title,
                        LastModified = articleDTO.LastModified,
                        Summary = articleDTO.Summary,
                        Content = articleDTO.Content,
                        Lead = articleDTO.Lead,
                        UserId = articleDTO.UserId,
                        Pictures = new ObservableCollection<PictureDTO>(articleDTO.Pictures)

                    };
                    OnArticleEditingStarted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem sikerült a cikk betöltése.", "Hiba");
                }
        }

        internal void ResetArticleUnderEdit()
        {
            ArticleUnderEdit = null;
        }

        private void CreateArticle()
        {             
            if (ArticleUnderEdit == null)
            {
                ArticleUnderEdit = new ArticleViewModel();
                OnArticleEditingStarted();
            }
        }      

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Articles = new ObservableCollection<ArticleListElement>(_model.ArticleList);
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }


        private void DeleteArticle(ArticleListElement article)
        {
            if (article != null)
            {
                MessageBoxResult result = MessageBox.Show("Valóban törölni szeretné a cikket?", "Törlés", MessageBoxButton.OKCancel);
                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.OK:
                        try
                        {
                            _model.DeleteArticle(article);
                            SelectedArticle = null;
                            _articles.Remove(article);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Sikertelen mentés");
                        }
                        break;
                }
            }
        }

        private void OnArticleEditingStarted()
        {
            if (ArticleEditingStarted != null)
                ArticleEditingStarted(this, EventArgs.Empty);
        }

        private void OnArticleEditingFinished()
        {          
            if (ArticleEditingFinished != null)
                ArticleEditingFinished(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        public void ViewModel_ArticleChanged(object sender, ArticleListEventArgs e)
        {
            Int32 index = Articles.IndexOf(Articles.FirstOrDefault(building => building.Id == e.Article.Id));
            Articles.RemoveAt(index); // módosítjuk a kollekciót
            Articles.Insert(index, e.Article);
        }

        public void ViewModel_ArticleCreated(object sender, ArticleListEventArgs e)
        {
            Articles.Add(e.Article);
        }

        public void ViewModel_PictureCreated(object sender, PictureEventArgs e)
        {
            ArticleUnderEdit.Pictures.Add(e.Picture);
        }

        private void OnImageEditingStarted()
        {
            if (ImageEditingStarted != null)
                ImageEditingStarted(this, new ArticleEventArgs { ArticleId = ArticleUnderEdit.Id });
        }

        private void ImageListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (lbTodoList.SelectedItem != null)
            //    this.Title = (lbTodoList.SelectedItem as TodoItem).Title;
        }
    }
}
