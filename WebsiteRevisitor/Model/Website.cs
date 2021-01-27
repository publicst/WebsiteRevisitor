using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteRevisitor.Model
{
    public class DailyWebsite : Website
    {
        public override bool IsUpdateExpected()
        {
            return true;
        }

        public DailyWebsite(int id, string title, string url, PageStyle pageStyle, DateTime lastChecked, UpdatePeriod updatePeriod = UpdatePeriod.Daily, int expectedDay = 1, DayOfWeek expectedDayOfWeek = DayOfWeek.Monday):
            base(id, title, url, pageStyle, lastChecked, updatePeriod, expectedDay, expectedDayOfWeek)
        {

        }
    }

    public class WeeklyWebsite : Website
    {
        public override bool IsUpdateExpected()
        {
            // if LastChecked date is before most recent expectedDayOfWeek (including today), then return true.
            var lastExpectedDayDate = DateTime.Now;
            while (lastExpectedDayDate.DayOfWeek != ExpectedDayOfWeek)
                lastExpectedDayDate = lastExpectedDayDate.AddDays(-1);
            return LastChecked <= lastExpectedDayDate;
        }

        public WeeklyWebsite(int id, string title, string url, PageStyle pageStyle, DateTime lastChecked, UpdatePeriod updatePeriod = UpdatePeriod.Weekly, int expectedDay = 1, DayOfWeek expectedDayOfWeek = DayOfWeek.Monday) :
            base(id, title, url, pageStyle, lastChecked, updatePeriod, expectedDay, expectedDayOfWeek)
        {

        }
    }

    public class MonthlyWebsite : Website
    {
        public override bool IsUpdateExpected()
        {
            // if LastChecked date's month is before latest ExpectedDay, then return true
            var now = DateTime.Now;
            // if today's Day is after ExpectedDay, check for this month if not last month.
            // not storing int Month because it could be last year (if today's month is January)
            var lastExpectedDate = (now.Day >= ExpectedDay) ? now : now.AddMonths(-1);
            lastExpectedDate = new DateTime(lastExpectedDate.Year, lastExpectedDate.Month, ExpectedDay); // now may not be the ExpectedDay
            return LastChecked <= lastExpectedDate;
        }

        public MonthlyWebsite(int id, string title, string url, PageStyle pageStyle, DateTime lastChecked, UpdatePeriod updatePeriod = UpdatePeriod.Weekly, int expectedDay = 1, DayOfWeek expectedDayOfWeek = DayOfWeek.Monday) :
            base(id, title, url, pageStyle, lastChecked, updatePeriod, expectedDay, expectedDayOfWeek)
        {

        }
    }

    public abstract class Website : IWebsite
    {
        #region IWebsite Properties
        public int ID { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public PageStyle PageStyle { get; set; } = PageStyle.Index;
        public DateTime LastChecked { get; set; } = DateTime.MinValue;
        public UpdatePeriod UpdatePeriod { get; set; }
        public int ExpectedDay { get; set; }
        public DayOfWeek ExpectedDayOfWeek { get; set; }
        #endregion

        #region IWebsite Methods
        public void ForceAccess()
        {
            AccessAndUpdateLastChecked();
        }

        public bool AccessExpected()
        {
            if (IsUpdateExpected())
            {
                return AccessAndUpdateLastChecked();
            }
            return false;
        }

        private bool AccessAndUpdateLastChecked()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                Process.Start(Url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Individual implementation of the Website class needs to implement this.
        /// </summary>
        public abstract bool IsUpdateExpected();
        #endregion

        #region Constructor
        public Website(int id, string title, string url, PageStyle pageStyle, DateTime lastChecked, UpdatePeriod updatePeriod, int expectedDay = 1, DayOfWeek expectedDayOfWeek = DayOfWeek.Monday)
        {
            ID = id;
            Title = title;
            Url = url;
            PageStyle = pageStyle;
            LastChecked = lastChecked;
            UpdatePeriod = updatePeriod;
            ExpectedDay = expectedDay;
            ExpectedDayOfWeek = expectedDayOfWeek;
        }
        #endregion Constructor
    }
}
