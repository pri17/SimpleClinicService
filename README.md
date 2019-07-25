# Simple Clinic Booking System
Servelec healthcare new starter exercise
Use C# language, aspx webforms(applied some jQuery functions as well), fluent NHibernate.

Complete this exercise using individual sulotions.

## Checking if existing timeslots confict with each other
```c#
private Boolean checkAvailablity(String clinicId, String specialtyId, DateTime start, DateTime end)
{


    var repository = new AppointmentRepository(this.unitOfWork);
    //1. reach the appointment list with the clinic id and specialty_id which are not cancelled;
    List<Appointment> list = repository.getListWithCSD(clinicId, specialtyId, start);

    foreach (var ll in list)
    {
        DateTime llStart = ll.start_time;
        DateTime llEnd = ll.end_time;
        if (start.CompareTo(llStart) >= 0 && start.CompareTo(llEnd) < 0 
            || end.CompareTo(llStart)> 0 && end.CompareTo(llEnd) <= 0)
        {
            return false;
        }
    }
    return true;
}
```

## TimePeriodLibrary.Net.2.1.1 Package
When calculating the available timeslots, instead of implementing complicatedly, found the **TimePeriodLibrary.Net.2.1.1** [package](https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET).

There are three classes  can use,  
***"TimePriodCollection"*** -- collection of time periods;  
***"TimePeriodCombiner"***  -- consolidate time periods, with a view on overlapping or adjacent time periods;  
![timescombiner](https://github.com/pri17/SimpleClinicService/blob/master/timecombiner.png)  

***"TimeGapCalculator"***  -- calculates the gaps between time periods in a time period collection.  
![gapcalculator](https://github.com/pri17/SimpleClinicService/blob/master/gapcalculator.png)  
So after get the boooked appointment lists(not cancelled), first store the time period into a time period collection,  
```c#
TimePeriodCollection periods = new TimePeriodCollection();
 foreach (var app in appointmentList)
 {
      periods.Add(new TimeRange(app.start_time, app.end_time));
 }
```
Combine the periods  
```c#
 TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
 ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(periods);

 foreach (ITimePeriod combinedPeriod in combinedPeriods)
 {
    //Console.WriteLine("Combined Period: " + combinedPeriod);
  	System.Diagnostics.Debug.WriteLine("Combined Period: " + combinedPeriod);
}
```
Then calculate the gaps among the periods, also with the gap between the start time and end time of the day ( 'dd' is the selected date the user wants to book an appointment on ).  
```c#
TimeGapCalculator<TimeRange> gapCalculator =  new TimeGapCalculator<TimeRange>(new TimeCalendar());

ITimePeriod searchLimits = new CalendarTimeRange(
                          new DateTime(dd.Year, dd.Month, dd.Day, 9, 0, 0),
    					new DateTime(dd.Year, dd.Month, dd.Day, 16, 0, 0));
ITimePeriodCollection freeTimes = gapCalculator.GetGaps(combinedPeriods, searchLimits);
foreach (ITimePeriod free in freeTimes)
 {
     //Console.WriteLine("Combined Period: " + combinedPeriod);
      System.Diagnostics.Debug.WriteLine("Free Period: " + free);
}
```

Full code: 
```c#
public ITimePeriodCollection getAvailableTimeslots(TimePeriodCollection periods, DateTime dd, String durationId)
{
    TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
    ITimePeriodCollection combinedPeriods = periodCombiner.CombinePeriods(periods);

    //foreach (ITimePeriod combinedPeriod in combinedPeriods)
    //{
    //    Console.WriteLine("Combined Period: " + combinedPeriod);
    //}

    //3. Find te gaps among the periods, also with the gap between the start time and end time

    TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

    ITimePeriod searchLimits = new CalendarTimeRange(
                       new DateTime(dd.Year, dd.Month, dd.Day, 9, 0, 0), new DateTime(dd.Year, dd.Month, dd.Day, 16, 0, 0));
    ITimePeriodCollection freeTimes = gapCalculator.GetGaps(combinedPeriods, searchLimits);

    foreach (ITimePeriod free in freeTimes)
    {
        //Console.WriteLine("Combined Period: " + combinedPeriod);
        System.Diagnostics.Debug.WriteLine("Free Period: " + free);
    }

    ITimePeriodCollection freeTimeSlots = splitTimeRange(freeTimes, durationId);

    return freeTimeSlots;
    //System.Diagnostics.Debug.WriteLine("The int of the Relation 'After':" + (int)PeriodRelation.After);

}

private ITimePeriodCollection splitTimeRange(ITimePeriodCollection freetimes, string duration)
{
    ITimePeriodCollection freeTimeslots = new TimePeriodCollection();

    foreach (var period in freetimes)
    {
        // for each free period, first determine if the period duraion >= user selected duration
        var diff = new DateDiff(period.Start,period.End).Minutes;
        int du = Int32.Parse(duration);//duration int, minutes
        if (diff >= du)
        {
            for (int i = 0;; i=i+5) // here set the rules that difference between timeslot start time is 5 minutes
            {
                DateTime start = period.Start.AddMinutes(i); // free start = period.start + set timeslot difference
                DateTime end = start.AddMinutes(du);
                if (end.CompareTo(period.End) <= 0) // If the end of the timeslot is already later than the period end, then end.
                {
                    ITimePeriod timeslot = new TimeRange(start,end);
                    freeTimeslots.Add(timeslot);
                }

                if (start.AddMinutes(du) >= period.End)
                    // if the timeslot start+duration >= period end, then finish the loop
                {
                    break;
                }
            }

        }

    }

    return freeTimeslots;
}
```

