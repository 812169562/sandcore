
using Autofac;
using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sand.DI;

namespace Sand.Cache
{
    public partial class RedisHelper : CSRedis.QuickHelperBase
    {
        public static IConfiguration Configuration { get; internal set; }
        public static void InitializeConfiguration(IConfiguration cfg)
        {
            Configuration = cfg;
            int port, poolsize, database;
            string ip, pass;
            //if (!int.TryParse(cfg["ConnectionStrings:redis:port"], out port)) port = 6379;
            //if (!int.TryParse(cfg["ConnectionStrings:redis:poolsize"], out poolsize)) poolsize = 50;
            //if (!int.TryParse(cfg["ConnectionStrings:redis:database"], out database)) database = 0;
            ip = "192.168.184.128";
            //pass = cfg["ConnectionStrings:redis:pass"];
            //Name = cfg["ConnectionStrings:redis:name"];
            Instance = new CSRedis.ConnectionPool(ip, 6379, 50);
            Instance.Connected += (s, o) => {
                CSRedis.RedisClient rc = s as CSRedis.RedisClient;
                if (!string.IsNullOrEmpty("")) rc.Auth("");
                if (0 > 0) rc.Select(1);
            };
        }
    }
}
