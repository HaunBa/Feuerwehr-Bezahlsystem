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

        public void OnGet()
        {
            var res = GetJson();  //get the data
            var dataString = "";

            for (int i = 0; i < res.Count; i++)
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

            var chartData = "{type: 'bar',responsive: true,data:{"+
                            "labels: [],"+
                            $"datasets: [{dataString}]" +
                                "}" +
                            "}";

            Chart = JsonConvert.DeserializeObject<ChartJs>(chartData);

            

            ////must remember to initialize the array....
            //Chart.data.datasets[0].data = new int[];
            //Chart.data.labels = new string[];
            //for (int i = 0; i < res.Count(); i++)
            //{
            //    Chart.data.datasets[0].data[i] = res[i];
            //}

            var names = new List<string>();
            var data = new List<int>();

            foreach (KeyValuePair<string, int> item in res)
            {
                names.Add(item.Key);
                data.Add(item.Value);
            }

            Chart.data.labels = names.ToArray();
            //Chart.data.datasets[0].data = data.ToArray();
            
            for (int i = 0; i < res.Count; i++)
            {
                int[] chartDatas = new int[1];
                chartDatas[0] = data[i];
                string label = names[i];

                Chart.data.datasets[i].data = chartDatas;
                Chart.data.datasets[i].label = label;
            }

            ChartJson = JsonConvert.SerializeObject(Chart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
