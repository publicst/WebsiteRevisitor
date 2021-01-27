using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteRevisitor.Model
{
    public interface IWebsite
    {
        #region Properties
        /// <summary>
        /// ID of this entry
        /// </summary>
        int ID { get; set; }
        /// <summary>
        /// Title of the website to visit
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Last Time User Checked the website
        /// </summary>
        DateTime LastChecked { get; set; }

        /// <summary>
        /// Expected Day of Month (only for UpdatePeriod.Monthly)
        /// </summary>
        int ExpectedDay { get; set; }

        /// <summary>
        /// Expected Day of Week
        /// </summary>
        DayOfWeek ExpectedDayOfWeek { get; set; }

        /// <summary>
        /// Expected Update Period
        /// </summary>
        UpdatePeriod UpdatePeriod { get; set; }

        /// <summary>
        /// PageStyle (next episode hint or index page)
        /// </summary>
        PageStyle PageStyle { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Access website without condition.
        /// </summary>
        void ForceAccess();

        /// <summary>
        /// Access if change is expected
        /// </summary>
        bool AccessExpected();

        /// <summary>
        /// Given Expected Day / DayOfWeek and UpdatePeriod return true if update is expected.
        /// </summary>
        /// <returns>If update is expected, return true</returns>
        bool IsUpdateExpected();
        #endregion
    }
}
