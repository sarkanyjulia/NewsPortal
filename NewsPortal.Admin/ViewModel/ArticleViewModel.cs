using NewsPortal.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Admin.ViewModel
{
    public class ArticleViewModel : ViewModelBase
    {
        private int _id;
        private String _title;
        private DateTime _lastModified;
        private String _summary;
        private String _content;
        private Boolean _lead;
        private int _userId;
        public ObservableCollection<PictureDTO> Pictures { get; set; }

        public Int32 Id { get; set; }

        public String Title
        {
            get => _title;
            
            set
            {              
                    this._title = value;
                    OnPropertyChanged();                
            }
        }

        

        public DateTime LastModified { get; set; }

        public String Summary
        {
            get => _summary;

            set
            {
                this._summary = value;
                OnPropertyChanged();
            }
        }
        public String Content
        {
            get => _content;

            set
            {
                this._content = value;
                OnPropertyChanged();
            }
        }

        public Boolean Lead
        {
            get => _lead;

            set
            {
                this._lead = value;
                OnPropertyChanged();
            }
        }

        public int UserId { get; set; }

        public ArticleViewModel()
        {
            Pictures = new ObservableCollection<PictureDTO>();
        }
    }
}
