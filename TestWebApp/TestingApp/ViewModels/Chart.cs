namespace TestingApp.ViewModels
{
    public class ChartJs
    {
        public string type { get; set; }
        public bool responsive { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
    }

    public class Data
    {
        public string[] labels { get; set; }
        public Dataset[] datasets { get; set; }
    }

    public class Dataset
    {
        public string label { get; set; }
        public int[] data { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public int borderWidth { get; set; }
    }

    public class Options
    {
        public Scales scales { get; set; }
    }

    public class Scales
    {
        public Yax[] yAxes { get; set; }
    }

    public class Yax
    {
        public Ticks ticks { get; set; }
    }

    public class Ticks
    {
        public bool beginAtZero { get; set; }
    }
}
