using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using System.Collections.Specialized;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class MakeBarsOnTick2:AddedItemHandler<Tick>
    {
        private IDataContext tradingData;
        private BarSettings barSettings;
        private ITimeTrackable timeTracker;
        private ILogger logger;
        private bool fake = false;
        //private FakeTimeTracker2 tt;

        public MakeBarsOnTick2(BarSettings barSettings, ITimeTrackable timeTracker, IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.tradingData = tradingData;
            this.barSettings = barSettings;
            this.timeTracker = timeTracker;
            this.logger = logger;

            if (timeTracker.GetType() == typeof(FakeTimeTracker2))
            {
                fake = true;
                //tt = timeTracker as FakeTimeTracker2;
            }

        }

        public override void OnItemAdded(Tick item)
        {
            if (fake)
            {
                if (((FakeTimeTracker2)timeTracker).stop > item.DateTime)
                {
                    ((FakeTimeTracker2)timeTracker).start = item.DateTime.Date.AddHours(9);
                    ((FakeTimeTracker2)timeTracker).stop  = item.DateTime;
                }
                var d3 = item.DateTime - ((FakeTimeTracker2)timeTracker).stop;
                ((FakeTimeTracker2)timeTracker).IncrementStopDate(d3.Seconds);
            }

            if (barSettings.Symbol != item.Symbol)
                return;

            DateTime end = GetPeriodEndDate();

            if (this.tradingData.Get<IEnumerable<Tick>>().LastOrDefault(t => t.Symbol == item.Symbol).DateTime < end)
                return;

            if (BarExists(end))
                return;

            if (end > this.timeTracker.StartAt + this.timeTracker.Duration)
                return;

            DateTime begin = end.AddSeconds(-this.barSettings.Interval);

            IEnumerable<Tick> barTicks = GetTicksInRangeOf(item, begin, end);

            if (barTicks == null)
                return;

            if (barTicks.Count() == 0)
                return;

            Bar fresh = BarsFactory.MakeBar(barTicks, end);
            fresh.Interval = this.barSettings.Interval;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, добавлен новый бар {2}", DateTime.Now, this.GetType().Name, fresh.ToString()));

            this.tradingData.Get<ObservableCollection<Bar>>().Add(fresh);
        }

        private bool BarExists(DateTime date)
        {
            return this.tradingData.Get<IEnumerable<Bar>>().Any(b => b.Symbol == this.barSettings.Symbol
                && b.DateTime.Year == date.Year
                && b.DateTime.Month == date.Month
                && b.DateTime.Day == date.Day
                && b.DateTime.Hour == date.Hour
                && b.DateTime.Minute == date.Minute
                && b.DateTime.Second == date.Second
                && b.DateTime.Millisecond == date.Millisecond);
        }

        private DateTime GetPeriodEndDate()
        {
            DateTime p = this.timeTracker.StartAt.AddSeconds(this.barSettings.Interval);

            DateTime current = this.timeTracker.StartAt + this.timeTracker.Duration;

            if (current < p)
                return p.RoundDownToNearestMinutes(this.barSettings.Interval / 60);

            return current.RoundDownToNearestMinutes(this.barSettings.Interval / 60);
        }

        private DateTime GetPeriodEndDate2()
        {
            DateTime p = this.timeTracker.StartAt.AddSeconds(this.barSettings.Interval);

            DateTime current = this.timeTracker.StartAt + this.timeTracker.Duration;

            if (current < p)
                return p.RoundDownToNearestMinutes2(this.barSettings.Interval / 60);

            return current.RoundDownToNearestMinutes2(this.barSettings.Interval / 60);
        }

        private IEnumerable<Tick> GetTicksInRangeOf(Tick item, DateTime begin, DateTime end)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Tick>>().Where(t => t.Symbol == item.Symbol && t.DateTime >= begin && t.DateTime < end);
            }
            catch
            {
                return null;
            }
        }

    }
    public class FakeTimeTracker2 : ITimeTrackable
    {
        public DateTime start;
        public DateTime stop;

        private FakeTimeTracker2() { }

        public FakeTimeTracker2(DateTime start, DateTime stop)
        {
            this.start = start;
            this.stop = stop;
        }

        public DateTime StartAt
        {
            get
            {
                return this.start;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return this.stop - this.start;
            }
        }

        public void IncrementStopDate(int seconds)
        {
            this.stop = this.stop.AddSeconds(seconds);
        }
    }
}
