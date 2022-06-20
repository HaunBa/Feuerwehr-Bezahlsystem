using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestingApp.Data;
using TestingApp.ViewModels;

namespace TestingApp.Areas.Identity.Pages.Account.Manage
{
    public class StatisticsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ChartJs Chart { get; set; }
        public ChartJs DonutChart { get; set; }

        public string DonutChartJson { get; set; }
        public string ChartJson { get; set; }

        public Dictionary<string, int> GetJson()
        {
            Dictionary<string, int> articles = new();

            var cuser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var data = _context.Payments.Include(x => x.Articles)
                                        .ThenInclude(x => x.Price)
                                        .Where(x => x.Person == cuser)
                                        .SelectMany(x => x.Articles)
                                        .ToList();

            foreach (var item in data)
            {
                if (articles.ContainsKey(item.Name))
                {
                    //true
                    articles[item.Name] += item.Amount;
                }
                else
                {
                    //false
                    articles.Add(item.Name, item.Amount);
                }
            }

            return articles;
        }

        public Dictionary<string, double> GetJsonPercentage()
        {
            Dictionary<string, int> articles = new();

            Dictionary<string, double> tmpArticles = new();

            double total = 0;

            var cuser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var data = _context.Payments.Include(x => x.Articles)
                                        .ThenInclude(x => x.Price)
                                        .Where(x => x.Person == cuser)
                                        .SelectMany(x => x.Articles)
                                        .ToList();

            foreach (var item in data)
            {
                if (articles.ContainsKey(item.Name))
                {
                    //true
                    articles[item.Name] += item.Amount;
                }
                else
                {
                    //false
                    articles.Add(item.Name, item.Amount);
                }
            }

            total = articles.Sum(x => x.Value);

            foreach (var item in articles)
            {
                tmpArticles.Add(item.Key, Math.Round(((articles[item.Key] / total) * 100), 2));
            }

            return tmpArticles;
        }

        public Dictionary<string, int> GetJsonByType(string type)
        {
            Dictionary<string, int> articles = new();

            var cuser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var data = _context.Payments.Include(x => x.Articles)
                                        .ThenInclude(x => x.Price)
                                        .Where(x => x.Person == cuser)
                                        .SelectMany(x => x.Articles)
                                        .Where(x => x.Type == type)
                                        .ToList();

            foreach (var item in data)
            {
                if (articles.ContainsKey(item.Name))
                {
                    //true
                    articles[item.Name] += item.Amount;
                }
                else
                {
                    //false
                    articles.Add(item.Name, item.Amount);
                }
            }

            return articles;
        }

        public void OnGet()
        {
            var res = GetJsonByType("Drink");  //get the data

            var articlePercentage = GetJsonPercentage();

            var articleByType = GetJson();

            var dataString = "";

            for (int i = 0; i < 1; i++)
            {
                dataString += @"{
                                label: '',
                                data: [],
                                ";


                switch (i%5)
                {
                    case 1:
                        dataString += @"backgroundColor: ['rgba(255, 99, 132, 1)'],borderColor: ['rgba(255, 99, 132, 1)']";
                        break;
                    case 2:
                        dataString += @"backgroundColor: ['rgba(54, 162, 235, 1)'],borderColor: ['rgba(54, 162, 235, 1)']";
                        break;
                    case 3:
                        dataString += @"backgroundColor: ['rgba(255, 206, 86, 1)'],borderColor: ['rgba(255, 206, 86, 1)']";
                        break;
                    case 4:
                        dataString += @"backgroundColor: ['rgba(75, 192, 192, 1)'],borderColor: ['rgba(75, 192, 192, 1)']";
                        break;
                    case 0:
                        dataString += @"backgroundColor: ['(153, 102, 255, 1)'],borderColor: ['rgba(153, 102, 255, 1)']";
                        break;
                    default:
                        break;
                }


                dataString += @",
                                borderWidth: 1
                                },";
                                
            }

            var chartData1 = "{type: 'bar',responsive: true,data:{"+
                            "labels: [],"+
                            $"datasets: [{dataString}]" +
                                "}" +
                            "}";

            // be sure replace `data: [12, 19, 3, 5, 2, 3]` to `data:[]`
            var chartData = @"{type: 'bar',responsive: true,data:{
                            labels: [],
                            datasets: [{
                                label: 'Bier',
                                data: [],
                                backgroundColor: [
                                'rgba(255, 99, 132, 0.8)',
                                'rgba(54, 162, 235, 0.8)',
                                'rgba(255, 206, 86, 0.8)',
                                'rgba(75, 192, 192, 0.8)',
                                'rgba(153, 102, 255, 0.8)',
                                'rgba(255, 159, 64, 0.8)'
                                    ],
                                borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                                    ],
                                borderWidth: 1
                            }]
                        },
                        options: { 
                                plugins: 
                                {
                                    legend: 
                                    { 
                                        display: false,
                                        
                                    }
                                }
                            }
                        }";

            var DonutChartData =  @"{type: 'doughnut',responsive: true,data:{
                                    labels: [],
                                    datasets: [{
                                        label: 'Article Percentage',
                                        data: [],
                                        backgroundColor: [
                                        'rgba(255, 99, 132, 0.2)',
                                        'rgba(54, 162, 235, 0.2)',
                                        'rgba(255, 206, 86, 0.2)',
                                        'rgba(75, 192, 192, 0.2)',
                                        'rgba(153, 102, 255, 0.2)',
                                        'rgba(255, 159, 64, 0.2)'
                                            ],
                                        borderColor: [
                                        'rgba(255, 99, 132, 1)',
                                        'rgba(54, 162, 235, 1)',
                                        'rgba(255, 206, 86, 1)',
                                        'rgba(75, 192, 192, 1)',
                                        'rgba(153, 102, 255, 1)',
                                        'rgba(255, 159, 64, 1)'
                                            ],
                                        borderWidth: 1
                                    }]
                                }
                            }";

            DonutChart = JsonConvert.DeserializeObject<ChartJs>(DonutChartData);

            var lables = new List<string>();
            var datas = new List<int>();

            foreach (var item in articlePercentage)
            {
                lables.Add(item.Key);
                datas.Add((int)item.Value);
            }

            for (int i = 0; i < articlePercentage.Count; i++)
            {
                int[] chartDatas = new int[1];
                chartDatas[0] = (int)datas[i];
                string label = lables[i];

                DonutChart.data.datasets[0].data = datas.ToArray();
                DonutChart.data.datasets[0].label = label;
                DonutChart.data.labels = DonutChart.data.labels.Append(label).ToArray(); 
            }

            DonutChartJson = JsonConvert.SerializeObject(DonutChart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            
            Chart = JsonConvert.DeserializeObject<ChartJs>(chartData);

            var names = new List<string>();
            var data = new List<int>();

            foreach (KeyValuePair<string, int> item in res)
            {
                names.Add(item.Key);
                data.Add(item.Value);
            }

            Chart.data.labels = names.ToArray();
            
            for (int i = 0; i < res.Count; i++)
            {
                int[] chartDatas = new int[1];
                chartDatas[0] = data[i];
                string label = names[i];

                Chart.data.datasets[0].data = data.ToArray();
            }

            ChartJson = JsonConvert.SerializeObject(Chart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
