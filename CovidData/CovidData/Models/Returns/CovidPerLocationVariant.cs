using System.Collections.Generic;

namespace CovidData.Models.Returns
{
    public class CovidPerLocationVariant
    {
        public string Location { get; set; }
        public List<CovidCase> CovidCases { get; set; }
    }
}
