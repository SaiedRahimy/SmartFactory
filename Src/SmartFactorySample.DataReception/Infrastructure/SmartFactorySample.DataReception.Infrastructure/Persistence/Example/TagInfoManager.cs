using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataReception.Application.Common.Exceptions;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using SmartFactorySample.DataReception.Application.Dtos;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.CreateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.DeleteTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Commands.UpdateTagInfo;
using SmartFactorySample.DataReception.Application.Entities.TagInfo.Queries.GetTagInfo;
using SmartFactorySample.DataReception.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Infrastructure.Persistence.Example
{
    public class TagInfoManager : ITagInfoManager
    {
        #region Dependencies
        private readonly DataReceptionContext _context;

        private readonly ILogger<TagInfoManager> _logger;

        #endregion


        #region Propereties
        private readonly System.Timers.Timer CacheTimer;

        private readonly ConcurrentDictionary<string, TagFullInfoDto> CachedTags = new ConcurrentDictionary<string, TagFullInfoDto>();

        #endregion


        #region Ctor
        public TagInfoManager(DataReceptionContext context, ILogger<TagInfoManager> logger)
        {
            _context = context;
            _logger = logger;

            FillTagsCache().GetAwaiter().GetResult();

            CacheTimer = new System.Timers.Timer(TimeSpan.FromMinutes(15)); 
            CacheTimer.Elapsed += async (sender, e) =>
            {
                await FillTagsCache();
            };
            CacheTimer.Enabled = true;
        }
        #endregion

        #region Public Methods
        public async Task<bool> CreateTagInfo(CreateTagInfoCommand request)
        {
            // add log
            var entity = new TagInfo
            {
                Name = request.Name,
            };
            await _context.TagInfos.AddAsync(entity);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Register Tag {request.Name}");

            return true;
        }



        public async Task<bool> DeleteTagInfo(DeleteTagInfoCommand request)
        {
            await CheckIdExist(request.Id);

            var entity = await _context.TagInfos.FirstOrDefaultAsync(c => c.Id == request.Id);

            _context.TagInfos.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Delete Tag {request.Id}");
            return true;
        }



        public async Task<TagFullInfoDto> GetTagInfo(GetTagInfoQuery request)
        {
            var result = await _context.TagInfos.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (result is not null)
            {
                return new TagFullInfoDto()
                {
                    Id = result.Id,
                    Name = result.Name
                };
            }
            else
                throw new NotFoundException($"Could not find value with id {request.Id}.");
        }



        public async Task<bool> UpdateTagInfo(UpdateTagInfoCommand request)
        {
            await CheckIdExist(request.Id);
            var entity = await _context.TagInfos.FirstOrDefaultAsync(c => c.Id == request.Id);

            //entity.Name = request.Name;// do not change Name!!!


            _context.Update(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Update Tag {request.Name}");

            return true;
        }



        private async Task CheckIdExist(int id)
        {
            if (!await _context.TagInfos.AnyAsync(c => c.Id == id))
            {
                throw new NotFoundException($"Could not find Tag with id {id}.");
            }
        }

        public async Task RegisterNewTags(List<TagInfoDto> baseSensorDataDtos)
        {
            var notRegisters = baseSensorDataDtos.Where(s => !CachedTags.ContainsKey(s.Name)).ToList();

            await Task.Run(async () =>
            {

                if (notRegisters.Count > 0)
                {
                    foreach (var tag in notRegisters)
                    {
                        var result = _context.TagInfos.Add(new TagInfo
                        {
                            Name = tag.Name,

                        });


                        if (result.State == EntityState.Added)
                        {
                            CachedTags.TryAdd(result.Entity.Name, new TagFullInfoDto { Id = result.Entity.Id, Name = result.Entity.Name });
                        }
                    }

                    await _context.SaveChangesAsync();
                }

            });
        }

        public void EnrichTagId(TagInfoDto tag)
        {
            if (CachedTags.ContainsKey(tag.Name))
            {
                var findTag = CachedTags[tag.Name];

                if (findTag != null)
                {
                    tag.Id = findTag.Id;
                }
            }

        }
        public async Task FillTagsCache()
        {
            try
            {
                var tags = await _context.TagInfos.Select(c=> new TagFullInfoDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();

                tags.ForEach(tag =>
                {

                    CachedTags.TryAdd(tag.Name, tag);

                });

                if (CachedTags.Count > 0)
                {
                    _logger.LogInformation(tags.Count + " has cached in FillTagsCache");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fill Tags Cache failed: ", ex);
            }
        }
        #endregion

    }
}
