using Newtonsoft.Json;

namespace Weekl.Core.Models
{
    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_eng")]
        public string NameEng { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("kpid")]
        public int Kpid { get; set; }

        [JsonProperty("yt")]
        public string Yt { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("actors")]
        public string Actors { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("premiere")]
        public string Premiere { get; set; }

        [JsonProperty("budget")]
        public string Budget { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("rating_kp")]
        public double RatingKp { get; set; }

        [JsonProperty("rating_kp_votes")]
        public int RatingKpVotes { get; set; }

        [JsonProperty("rating_imdb")]
        public double RatingImdb { get; set; }

        [JsonProperty("rating_imdb_votes")]
        public int RatingImdbVotes { get; set; }

        [JsonProperty("isserial")]
        public int Isserial { get; set; }

        [JsonProperty("descr")]
        public string Descr { get; set; }

        [JsonProperty("hasAwards")]
        public int HasAwards { get; set; }

        [JsonProperty("ratingMPAA")]
        public string RatingMPAA { get; set; }

        [JsonProperty("ratingAgeLimits")]
        public int RatingAgeLimits { get; set; }

        [JsonProperty("trailer_hd")]
        public string TrailerHd { get; set; }

        [JsonProperty("trailer_sd")]
        public string TrailerSd { get; set; }

        [JsonProperty("trailer_low")]
        public string TrailerLow { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("quality")]
        public string Quality { get; set; }

        [JsonProperty("translator")]
        public string Translator { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }
    }

}
