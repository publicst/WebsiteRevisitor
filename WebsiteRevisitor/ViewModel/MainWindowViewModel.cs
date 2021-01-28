using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WebsiteRevisitor.Model;
using WebsiteRevisitor.ViewModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Threading;

namespace WebsiteRevisitor
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor
        public MainWindowViewModel(string jsonPath)
        {
            JsonPath = jsonPath;

            LoadJson();
            InitializeDispatcherTimer();
        }
        #endregion

        #region Destructor
        ~MainWindowViewModel()
        {
            SaveJson();
            StopDispatcherTimer();
        }
        #endregion

        #region Member
        ObservableCollection<WebsiteViewModel> _websites = new ObservableCollection<WebsiteViewModel>();
        CollectionView _websiteCollectionView = null;
        WebsiteViewModel _selected;
        #endregion 

        #region Properties
        public ObservableCollection<WebsiteViewModel> Websites 
        {
            get { return _websites; }
            set 
            {
                _websites = value;
                RaisePropertyChanged();
                RaisePropertyChanged("WebsiteCollectionView");
            }
        }
        public CollectionView WebsiteCollectionView // only used for viewing
        {
            get
            {
                _websiteCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(Websites);
                if (_websiteCollectionView != null)
                    _websiteCollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastChecked", ListSortDirection.Descending));
                return _websiteCollectionView;
            }
        }

        public WebsiteViewModel SelectedWebsite 
        { 
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }
        public VMProp<string> WindowTitle { get; set; } = new VMProp<string>();
        #endregion

        #region Private Helper
        private string JsonPath { get; set; }
        private void SaveJson()
        {
            if (!string.IsNullOrEmpty(JsonPath))
            {
                (new FileInfo(JsonPath)).Directory.Create();
                using (StreamWriter writer = File.CreateText(JsonPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, Websites);
                }
            }
        }
        private void LoadJson()
        {
            if (File.Exists(JsonPath))
            {
                using (StreamReader reader = File.OpenText(JsonPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    Websites = (ObservableCollection<WebsiteViewModel>)serializer.Deserialize(reader, typeof(ObservableCollection<WebsiteViewModel>));
                }
            }
            else
            {
                MessageBox.Show($"{Path.GetFullPath(JsonPath)} does not exist");
            }
        }
        private int GenerateNewID()
        {
            return Websites.Max(site => site.ID) + 1;
        }

        private void ResortCollectionView()
        {
            _websiteCollectionView.SortDescriptions.Clear();
            _websiteCollectionView.SortDescriptions.Add(new SortDescription("LastChecked", ListSortDirection.Descending));
        }

        private void UpdateWindowTitle() => WindowTitle.Value = $"Website Revisitor {DateTime.Now:MMM/dd/yyyy} {DateTime.Now.DayOfWeek} {DateTime.Now:HH:mm:ss}";
        #endregion

        #region Dispatcher Timer related codes
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private void InitializeDispatcherTimer()
        {
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) => UpdateWindowTitle();

        private void StopDispatcherTimer() => _dispatcherTimer.Stop();
        #endregion 

        #region Command
        void AccessAll()
        {
            foreach (var site in Websites)
                site.ForceAccess();
            ResortCollectionView();
        }

        bool CanAccessAll()
        {
            return true;
        }

        public ICommand AccessAllCommand { get { return new RelayCommand(AccessAll, CanAccessAll); } }

        void AccessExpected()
        {
            foreach (var site in Websites)
                site.AccessExpected();
            ResortCollectionView();
        }

        bool CanAccessExpected()
        {
            return true;
        }
        public ICommand AccessExpectedCommand { get { return new RelayCommand(AccessExpected, CanAccessExpected); } }
        void AccessCurrent()
        {
            SelectedWebsite.ForceAccess();
            ResortCollectionView();
        }

        bool CanAccessCurrent()
        {
            return true;
        }
        public ICommand AccessCurrentCommand { get { return new RelayCommand(AccessCurrent, CanAccessCurrent); } }

        void AddNewWebsite()
        {
            // todo : this is tightly coupled.
            var newWebsite = new WebsiteViewModel(new WeeklyWebsite(GenerateNewID(), "", "", PageStyle.Index, DateTime.MinValue));
            Websites.Add(newWebsite);
            SelectedWebsite = newWebsite;
        }

        bool CanAddNewWebsite()
        {
            // if any websites' title are null or empty, we cannot allow adding new website.
            // todo : cache so this does not have to go through the entire collection every time.
            foreach (var site in Websites)
            {
                if (string.IsNullOrEmpty(site.Title))
                    return false;
            }
            return true;
        }
        public ICommand AddNewWebsiteCommand { get { return new RelayCommand(AddNewWebsite, CanAddNewWebsite); } }

        void DeleteSelectedWebsite()
        {
            Websites.Remove(Websites.Single(site => site.ID == SelectedWebsite.ID));
            SelectedWebsite = null;
            ResortCollectionView();
        }

        /// <summary>
        /// Can be deleted if selection is not null
        /// </summary>
        /// <returns></returns>
        bool CanDeleteSelectedWebsite()
        {
            return SelectedWebsite != null;
        }
        public ICommand DeleteSelectedWebsiteCommand { get { return new RelayCommand(DeleteSelectedWebsite, CanDeleteSelectedWebsite); } }
        #endregion
    }
}
