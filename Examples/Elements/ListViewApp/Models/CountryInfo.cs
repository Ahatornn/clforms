namespace ListViewApp.Models
{
    public class CountryInfo
    {
        public string Country { get; set; }
        public string Alpha3 { get; set; }
        public int Gdp { get; set; }
        public decimal Population { get; set; }
        public decimal Percentage { get; set; }

        public override string ToString() => $"Country:{Country}, gdp:{Gdp}";
    }
}
