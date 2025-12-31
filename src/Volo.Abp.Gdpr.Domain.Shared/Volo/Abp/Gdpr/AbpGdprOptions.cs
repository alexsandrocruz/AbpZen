using System;

namespace Volo.Abp.Gdpr;

public class AbpGdprOptions
{
    /// <summary>
    /// Used to indicate allowed request time interval.
    /// Default is 1 day.
    /// </summary>
    public TimeSpan RequestTimeInterval { get; set; } = TimeSpan.FromDays(1);
    
    public int MinutesForDataPreparation { get; set; } = 60;
}