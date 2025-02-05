using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Application.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Infrastructure.Persistence.TagDailyData
{
    public class TagDailyDataManager : ITagDailyDataManager
    {
        #region Dependencies
        private readonly DataPresentationContext _context;

        private readonly ILogger<TagDailyDataManager> _logger;

        #endregion


        #region Propereties


        #endregion


        #region Ctor
        public TagDailyDataManager(DataPresentationContext context, ILogger<TagDailyDataManager> logger)
        {
            _context = context;
            _logger = logger;


        }
        #endregion

        #region Public Methods
        public async Task<bool> CreateOrUpdate(List<TagInfoDto> requests)
        {
            var tagIds = requests.Select(r => r.Id).Distinct().ToList();
            var dates = requests.Select(r => DateOnly.FromDateTime(r.Timestamp.Date)).Distinct().ToList();

            var existingData = await _context.TagDailyDatas
                .Where(c => tagIds.Contains(c.TagId) && dates.Contains(c.Date))
                .ToListAsync();

            var dataDictionary = existingData.ToDictionary(k => (k.TagId, k.Date));

            var newEntries = new List<Domain.Entities.TagDailyData>();

            foreach (var request in requests)
            {
                var date = DateOnly.FromDateTime(request.Timestamp);
                var value = (float)(request.Value);
                if (!dataDictionary.TryGetValue((request.Id, date), out var findItem))
                {
                    var newEntity = new Domain.Entities.TagDailyData
                    {
                        TagId = request.Id,
                        Date = date,
                        Key1 = value
                    };
                    newEntries.Add(newEntity);
                    dataDictionary[(request.Id, date)] = newEntity;
                }
                else
                {
                    var keys = new[]
                    {
                        findItem.Key2, findItem.Key3, findItem.Key4, findItem.Key5,
                        findItem.Key6, findItem.Key7, findItem.Key8, findItem.Key9, findItem.Key10
                    };

                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (!keys[i].HasValue)
                        {
                            switch (i)
                            {
                                case 0: findItem.Key2 = value; break;
                                case 1: findItem.Key3 = value; break;
                                case 2: findItem.Key4 = value; break;
                                case 3: findItem.Key5 = value; break;
                                case 4: findItem.Key6 = value; break;
                                case 5: findItem.Key7 = value; break;
                                case 6: findItem.Key8 = value; break;
                                case 7: findItem.Key9 = value; break;
                                case 8: findItem.Key10 = value; break;
                            }
                            break;
                        }
                    }
                }
            }

            if (newEntries.Any())
            {
                await _context.TagDailyDatas.AddRangeAsync(newEntries);
            }

            await _context.SaveChangesAsync();

            return true;
        }



        #endregion

    }
}
