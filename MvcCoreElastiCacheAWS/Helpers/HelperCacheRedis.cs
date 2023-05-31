using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                //AQUI ES DONDE IRA LA CADENA DE CONEXION
                return ConnectionMultiplexer.Connect("CacheRedis");
            });

        public static ConnectionMultiplexer Connection
        {
            get { return CreateConnection.Value; }
        }
    }
}
