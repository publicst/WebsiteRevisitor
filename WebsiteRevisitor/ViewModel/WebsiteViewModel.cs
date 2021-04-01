using System;
using System.Net.Http;
using WebsiteRevisitor.Model;

namespace WebsiteRevisitor.ViewModel
{
    public class WebsiteViewModel : ViewModelBase
    {
        #region Constructor
        public WebsiteViewModel(IWebsite website)
        {
            _website = website;
        }
        #endregion Constructor

        #region Member
        IWebsite _website;
        #endregion Member

        #region Properties
        public IWebsite Website
        {
            get { return _website; }
            set 
            { 
                _website = value;
                RaisePropertyChanged();
            }
        }

        public int ID 
        { 
            get { return _website.ID; }
            set { _website.ID = value; }
        }
        public string Title
        {
            get { return _website.Title; }
            set
            {
                if (_website.Title != value)
                {
                    _website.Title = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Url
        {
            get { return _website.Url; }
            set
            {
                if (_website.Url != value)
                {
                    _website.Url = value;
                    RaisePropertyChanged();
                }
            }
        }
        public PageStyle PageStyle
        {
            get { return _website.PageStyle; }
            set
            {
                if (_website.PageStyle != value)
                {
                    _website.PageStyle = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime LastChecked
        {
            get { return _website.LastChecked; }
            set
            {
                if (_website.LastChecked != value)
                {
                    _website.LastChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public UpdatePeriod UpdatePeriod
        {
            get { return _website.UpdatePeriod; }
            set
            {
                if (_website.UpdatePeriod != value)
                {
                    // todo : this is tightly coupled
                    // todo : if leak were to occur, I think it would be here
                    switch(value)
                    {
                        case UpdatePeriod.Weekly:
                            _website = new WeeklyWebsite(_website.ID, _website.Title, _website.Url, _website.PageStyle, _website.LastChecked, UpdatePeriod.Weekly, _website.ExpectedDay, _website.ExpectedDayOfWeek);
                            break;
                        case UpdatePeriod.Monthly:
                            _website = new MonthlyWebsite(_website.ID, _website.Title, _website.Url, _website.PageStyle, _website.LastChecked, UpdatePeriod.Monthly, _website.ExpectedDay, _website.ExpectedDayOfWeek);
                            break;
                        default:
                            _website = new DailyWebsite(_website.ID, _website.Title, _website.Url, _website.PageStyle, _website.LastChecked, UpdatePeriod.Daily, _website.ExpectedDay, _website.ExpectedDayOfWeek);
                            break;
                    }
                    // following unlike other Propertiers is not necessary because its different now
                    // _website.UpdatePeriod = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ExpectedDay
        {
            get { return _website.ExpectedDay; }
            set
            {
                if (_website.ExpectedDay != value)
                {
                    if (value <= 0)
                        _website.ExpectedDay = 1;
                    else if (value > 29)
                        _website.ExpectedDay = 29;
                    else
                        _website.ExpectedDay = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DayOfWeek ExpectedDayOfWeek
        {
            get { return _website.ExpectedDayOfWeek; }
            set
            {
                if (_website.ExpectedDayOfWeek != value)
                {
                    _website.ExpectedDayOfWeek = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion Properties

        #region Methods
        public void ForceAccess()
        {
            _website.ForceAccess();
            LastChecked = DateTime.Now;
        }

        public void AccessExpected()
        {
            if (_website.AccessExpected())
                LastChecked = DateTime.Now;            
        }

        public void AccessDiagnose(HttpClient client)
        {
            _website.AccessDiagnose(client);
        }

        public bool IsUpdateExpected()
        {
            return _website.IsUpdateExpected();
        }
        #endregion

        #region Command
        #endregion
    }
}
