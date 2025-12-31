using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Volo.CmsKit.Public.Polls;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Poll;

public class PollViewModel
{
    public Guid? Id { get; set; }
    
    [Required]
    [Display(Name = "Question")]
    public string Question { get; set; }
    
    public bool AllowMultipleVote { get; set; }
    
    public int VoteCount { get; set; }
    
    public bool ShowVoteCount { get; set; }
    
    public bool ShowResultWithoutGivingVote { get; set; }
    
    public bool ShowHoursLeft { get; set; }

    public TimeSpan? TimeLeft => GetTimeLeft();

    public bool IsOutdated => CalculateIsOutdated();
    
    public DateTime? EndDate { get; set; }
    
    public DateTime? ResultShowingEndDate { get; set; }
    
    public DateTime CreationTime { get; set; }

    public List<string> Texts { get; set; } = new();
    
    public List<Guid> OptionIds { get; set; } = new();

    //Result
    public int PollVoteCount { get; set; }
    
    public List<PollResultDto> PollResultDetails { get; set; }

    public bool IsVoted { get; set; }
    
    public string WidgetName { get; set; }
    
    public string LoginUrl { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string GetTimeLeftAsText(IHtmlLocalizer l)
    {
        if (TimeLeft == null || !ShowHoursLeft)
        {
            return string.Empty;
        }

        if (TimeLeft.Value.TotalDays > 365)
        {
            return string.Empty;
        }

        if (TimeLeft.Value.TotalDays > 30)
        {
            var months = (int) (TimeLeft.Value.TotalDays / 30);
            var localizationKey = months == 1 ? "MonthLeft" : "MonthsLeft";
            return l.GetString(localizationKey, months);
        }

        if (TimeLeft.Value.TotalDays > 1)
        {
            var days = (int) TimeLeft.Value.TotalDays;
            var localizationKey = days == 1 ? "DayLeft" : "DaysLeft";
            return l.GetString(localizationKey, days);
        }

        if (TimeLeft.Value.TotalHours > 1)
        {
            var hours = (int) TimeLeft.Value.TotalHours;
            var localizationKey = hours == 1 ? "HourLeft" : "HoursLeft";
            return l.GetString(localizationKey, hours);
        }

        if (TimeLeft.Value.TotalMinutes > 1)
        {
            var minutes = (int) TimeLeft.Value.TotalMinutes;
            var localizationKey = minutes == 1 ? "MinuteLeft" : "MinutesLeft";
            return l.GetString(localizationKey, minutes);
        }
        else
        {
            var seconds = (int) TimeLeft.Value.TotalSeconds;
            var localizationKey = seconds == 1 ? "SecondLeft" : "SecondsLeft";
            return l.GetString(localizationKey, seconds);
        }
    }
    public string GetVoteCountText(IHtmlLocalizer l)
    {
        if (!ShowVoteCount)
        {
            return string.Empty;
        }
        
        if (VoteCount == 0)
        {
            return l["NoVotes"].Value;
        }
        
        if (VoteCount == 1)
        {
            return l.GetString("SingleVoteCount");
        }
        
        return l.GetString("VoteCount{0}", VoteCount);
    }

    private bool CalculateIsOutdated()
    {
        if (TimeLeft == null)
        {
            return false;
        }
        
        return TimeLeft.Value.TotalSeconds < 1;
    }

    public TimeSpan? GetTimeLeft()
    {
        var now = DateTime.Now;
        
        if (!EndDate.HasValue)
        {
            return null;
        }

        if (EndDate.Value <= now)
        {
            return null;
        }

        return EndDate - now;
    }

}