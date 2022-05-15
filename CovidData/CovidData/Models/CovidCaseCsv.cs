using System;

namespace CovidData.Models
{
    public class CovidCaseCsv
    {
        public string location { get; set; }
        public DateTime date { get; set; }
        public string variant { get; set; }
        public int num_sequences { get; set; }
        public double perc_sequences { get; set; }
        public int num_sequences_total { get; set; }
    }
}
