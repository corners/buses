using ReadingBusesCore.Persistence;
using ReadingBusesCore.Persistence.Entities;
using ReadingBusesCore.Routes.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingBusesCore.Routes
{
    public static class RouteApi
    {
        const int MaxDataAge_Days = 100;

        //public static async Task<IReadOnlyList<ServicePattern>> GetServicePatterns()
        //{
        //    var webQuery = new WebQuery();
        //    return await DownloadAsync<ServicePattern>(
        //        "ServicePatterns", 
        //        webQuery.GetServicePatterns, 
        //        context => context.ServicePatterns);
        //}

        public static async Task<ServicePattern> GetSerivcePattern(string service)
        {
            var webQuery = new WebQuery();
            return await webQuery.GetServicePattern(service);
        }
            
        public static async Task<IReadOnlyList<Location>> GetLocations()
        {
            var webQuery = new WebQuery();
            return await GetDataAsync2<IReadOnlyList<Location>>(
                "Locations",
                webQuery.GetLocations,
                (context, locations) => context.Locations.AddRange(locations),
                context => context.Locations.ToList().AsReadOnly());
        }

        public static IReadOnlyList<string> Services(IReadOnlyList<Location> locations)
        {
            var services = locations.SelectMany(l => l.ServiceList)
                                    .Distinct(StringComparer.OrdinalIgnoreCase)
                                    .ToList()
                                    .AsReadOnly();
            return services;
        }

        public static IReadOnlyList<Location> LocationsForService(string service, IReadOnlyList<Location> locations)
        {
            var result = locations.Where(l => ForService(service, l))
                                  .ToList()
                                  .AsReadOnly();
            // need to order by direction --> extra web query required
            return result;
        }



        static async Task<T> GetDataAsync2<T>(
            string table,
            Func<Task<T>> webQueryAsync,
            Action<Context, T> dbWrite,
            Func<Context, T> dbsetSelector)
            where T : class
        {
            T records;
            using (var context = new Context())
            {
                var update = context.LastUpdates.Find(table);
                var reload = update == null || (update.Updated - DateTime.UtcNow).TotalDays > MaxDataAge_Days;
                if (reload)
                {
                    records = await webQueryAsync();

                    context.Database.ExecuteSqlCommand("delete from " + table);
                    dbWrite(context, records);

                    if (update == null)
                    {
                        update = new LastUpdate { TableName = table };
                        context.LastUpdates.Add(update);
                    }
                    update.Updated = DateTime.UtcNow;

                    context.SaveChanges();
                }
                else
                    records = dbsetSelector(context);
            }
            return records;
        }
        
        //static async Task<IReadOnlyList<T>> GetDataAsync<T>(
        //    string table, 
        //    Func<Task<IReadOnlyList<T>>> webQueryAsync, 
        //    Func<Context, DbSet<T>> dbsetSelector)
        //    where T : class
        //{
        //    IReadOnlyList<T> records;
        //    using (var context = new Context())
        //    {
        //        var update = context.LastUpdates.Find(table);
        //        var reload = update == null || (update.Updated - DateTime.UtcNow).TotalDays > MaxDataAge_Days;
        //        if (reload)
        //        {
        //            records = await webQueryAsync();

        //            context.Database.ExecuteSqlCommand("delete from " + table);
        //            dbsetSelector(context).AddRange(records);

        //            if (update == null)
        //            {
        //                update = new LastUpdate { TableName = table };
        //                context.LastUpdates.Add(update);
        //            }
        //            update.Updated = DateTime.UtcNow;

        //            context.SaveChanges();
        //        }
        //        else
        //            records = dbsetSelector(context).ToList().AsReadOnly();
        //    }
        //    return records;
        //}

        static bool ForService(string service, Location location)
        {
            return location.ServiceList.Any(s => service.Equals(s, StringComparison.OrdinalIgnoreCase));
        }

    }
}
