using Microsoft.Extensions.Caching.Distributed;
using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services
{
    public class ServiceAWSCache
    {
        //private IDatabase cache;
        private readonly IDistributedCache cache;

        public ServiceAWSCache()
        {
            this.cache = cache;
        }

        public async Task<List<Coche>> getCochesFavoritosAsync()
        {
            string jsonCoches = await this.cache.GetStringAsync("cochesfavoritos");
            if(jsonCoches == null)
            {
                return null;
            }
            else
            {
                List<Coche> cars= JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return cars;
            }
        }

        public async Task AddCocheAsync(Coche car)
        {
            List<Coche> cars= await this.getCochesFavoritosAsync();
            if(cars == null)
            {
                cars = new List<Coche>();
            }
            cars.Add(car);
            string jsonCoches=JsonConvert.SerializeObject(cars);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30),
            };
            await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);
        }

        public async Task DeleteCocheFavoritosAsync(int idcoche)
        {
            List<Coche> coches= await this.getCochesFavoritosAsync();
            if (coches != null)
            {
                Coche carEliminar =
                    coches.FirstOrDefault(x => x.idCoche == idcoche);
                coches.Remove(carEliminar);
                if (coches.Count == 0)
                {
                    await this.cache.RemoveAsync("cochesfavoritos");
                }
                else
                {
                    string jsonCoches = JsonConvert.SerializeObject(coches);
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration= TimeSpan.FromMinutes(30),
                    };
                    await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);
                    //await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
                }

            }
        }
    }
}
