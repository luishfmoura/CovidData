using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidData.Models
{
    [Table("CovidCases")]
    public class CovidCase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Variant { get; set; }
        [Required]
        public int NumSequences { get; set; }
        [Required]
        public double PercSequences { get; set; }
        [Required]
        public int NumSequencesTotal { get; set; }
    }
}
