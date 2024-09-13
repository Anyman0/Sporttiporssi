using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class HockeyGame
    {
        [JsonProperty("Events")]
        public List<Event> Events { get; set; }
    }

    public class Event : INotifyPropertyChanged
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("StageId")]
        public Guid StageId { get; set; }

        [JsonProperty("Stage")]
        public Stage Stage { get; set; }

        [JsonProperty("HomeTeamId")]
        public Guid HomeTeamId { get; set; }

        [JsonProperty("HomeTeam")]
        public Team HomeTeam { get; set; }

        [JsonProperty("AwayTeamId")]
        public Guid AwayTeamId { get; set; }

        [JsonProperty("AwayTeam")]
        public Team AwayTeam { get; set; }

        [JsonProperty("EventStatus")]
        public string EventStatus { get; set; }

        [JsonProperty("EventSeriesId")]
        public int EventSeriesId { get; set; }

        [JsonProperty("EventPriority")]
        public int EventPriority { get; set; }

        [JsonProperty("EventCoverage")]
        public int EventCoverage { get; set; }

        [JsonProperty("EarnInfo")]
        public string EarnInfo { get; set; }

        [JsonProperty("EventType")]
        public int EventType { get; set; }

        [JsonProperty("EventStartDate")]
        public long EventStartDate { get; set; }

        [JsonProperty("ExternalId")]
        public int ExternalId { get; set; }

        [JsonProperty("ExternalIdX")]
        public int ExternalIdX { get; set; }

        [JsonProperty("EventId")]
        public int EventId { get; set; }

        private bool _isRosterExpanded;
        private bool _isStatsExpanded;
        private int _homeTeamRank;
        private int _awayTeamRank;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsRosterExpanded
        {
            get => _isRosterExpanded;
            set
            {
                if (_isRosterExpanded != value)
                {
                    _isRosterExpanded = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsStatsExpanded
        {
            get => _isStatsExpanded;
            set
            {
                if (_isStatsExpanded != value)
                {
                    _isStatsExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public int HomeTeamRank
        {
            get => _homeTeamRank;
            set
            {
                _homeTeamRank = value;
                OnPropertyChanged(nameof(HomeTeamRank));
            }
        }

        public int AwayTeamRank
        {
            get => _awayTeamRank;
            set
            {
                _awayTeamRank = value;
                OnPropertyChanged(nameof(AwayTeamRank));
            }
        }

        public string EventStartLocalTime
        {
            get
            {
                try
                {
                    // Convert long to string and parse it to a DateTime
                    string eventStartDateString = EventStartDate.ToString();
                    DateTime eventDateTimeUtc = DateTime.ParseExact(eventStartDateString, "yyyyMMddHHmmss", null);

                    // Print raw UTC time for debugging
                    Console.WriteLine($"Raw UTC time: {eventDateTimeUtc.ToUniversalTime()}");

                    // Find the Finnish time zone
                    TimeZoneInfo finlandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Helsinki");

                    // Convert the UTC time to Finnish local time
                    DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(eventDateTimeUtc, finlandTimeZone);

                    // Print local time for debugging
                    Console.WriteLine($"Local time: {localDateTime}");

                    // Remove DST +1 effect
                    localDateTime = localDateTime.AddHours(-1);

                    // Return time in "HH:mm" format
                    return localDateTime.ToString("HH:mm");
                }
                catch (TimeZoneNotFoundException ex)
                {
                    // Handle specific time zone not found exception
                    return $"Time zone not found: {ex.Message}";
                }
                catch (InvalidTimeZoneException ex)
                {
                    // Handle invalid time zone data
                    return $"Invalid time zone data: {ex.Message}";
                }
                catch (Exception ex)
                {
                    // Handle general exceptions
                    return $"Error: {ex.Message}";
                }
            }
        }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
    }

    public class Team : INotifyPropertyChanged
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("Abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("CountryId")]
        public string CountryId { get; set; }

        [JsonProperty("CountryName")]
        public string CountryName { get; set; }

        [JsonProperty("HasVideo")]
        public bool HasVideo { get; set; }

        [JsonProperty("HomeEvents")]
        public List<Event> HomeEvents { get; set; }

        [JsonProperty("AwayEvents")]
        public List<Event> AwayEvents { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _rank;
        public int Rank
        {
            get => _rank;
            set
            {
                _rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }
    }

    public class Stage
    {
        public Guid Id { get; set; } // Sid

        [Required]
        public string Name { get; set; } // Snm

        public string Code { get; set; } // Scd

        public string CountryId { get; set; } // Cid

        public string CountryName { get; set; } // Cnm

        public string CountryNameT { get; set; } // CnmT

        public string CountryShortName { get; set; } // Csnm

        public string CountryCode { get; set; } // Ccd

        public int Scu { get; set; } // Scu

        public string Sds { get; set; } // Sds

        public int Chi { get; set; } // Chi

        public int Shi { get; set; } // Shi

        public ICollection<Event> Events { get; set; } // Navigation property
    }

}
