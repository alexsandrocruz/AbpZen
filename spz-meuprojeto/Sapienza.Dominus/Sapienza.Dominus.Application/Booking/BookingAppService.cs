using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Booking.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Booking;

/// <summary>
/// Application service for Booking entity
/// </summary>
[Authorize(BookingPermissions.Default)]
public class BookingAppService :
    DominusAppService,
    IBookingAppService
{
    private readonly IRepository<Dominus.Booking.Booking, Guid> _repository;
    private readonly IRepository<Dominus.SchedulerType.SchedulerType, Guid> _schedulerTypeRepository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public BookingAppService(
        IRepository<Dominus.Booking.Booking, Guid> repository,
        IRepository<Dominus.SchedulerType.SchedulerType, Guid> schedulerTypeRepository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _schedulerTypeRepository = schedulerTypeRepository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single Booking by Id
    /// </summary>
    public virtual async Task<BookingDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Booking.Booking, BookingDto>(entity);
        if (entity.SchedulerTypeId != null)
        {
            var parent = await _schedulerTypeRepository.FindAsync(entity.SchedulerTypeId.Value);
            dto.SchedulerTypeDisplayName = parent?.Name;
        }
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Bookings
    /// </summary>
    public virtual async Task<PagedResultDto<BookingDto>> GetListAsync(BookingGetListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        // Apply filters
        queryable = ApplyFilters(queryable, input);

        // Apply default sorting (by CreationTime descending)
        queryable = queryable.OrderByDescending(e => e.CreationTime);

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var entities = await AsyncExecuter.ToListAsync(queryable);
        var dtoList = ObjectMapper.Map<List<Dominus.Booking.Booking>, List<BookingDto>>(entities);
        var schedulerTypeIds = entities
            .Where(x => x.SchedulerTypeId != null)
            .Select(x => x.SchedulerTypeId.Value)
            .Distinct()
            .ToList();

        if (schedulerTypeIds.Any())
        {
            var parents = await _schedulerTypeRepository.GetListAsync(x => schedulerTypeIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.SchedulerTypeId != null))
            {
                if (parentMap.TryGetValue(dto.SchedulerTypeId.Value, out var displayName))
                {
                    dto.SchedulerTypeDisplayName = displayName;
                }
            }
        }
        var clientIds = entities
            .Where(x => x.ClientId != null)
            .Select(x => x.ClientId.Value)
            .Distinct()
            .ToList();

        if (clientIds.Any())
        {
            var parents = await _clientRepository.GetListAsync(x => clientIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ClientId != null))
            {
                if (parentMap.TryGetValue(dto.ClientId.Value, out var displayName))
                {
                    dto.ClientDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<BookingDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Booking
    /// </summary>
    [Authorize(BookingPermissions.Create)]
    public virtual async Task<BookingDto> CreateAsync(CreateUpdateBookingDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateBookingDto, Dominus.Booking.Booking>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Booking.Booking, BookingDto>(entity);
    }

    /// <summary>
    /// Updates an existing Booking
    /// </summary>
    [Authorize(BookingPermissions.Update)]
    public virtual async Task<BookingDto> UpdateAsync(Guid id, CreateUpdateBookingDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Booking.Booking), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Booking.Booking, BookingDto>(entity);
    }

    /// <summary>
    /// Deletes a Booking
    /// </summary>
    [Authorize(BookingPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBookingLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.ClientName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.Booking.Booking> ApplyFilters(IQueryable<Dominus.Booking.Booking> queryable, BookingGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SchedulerTypeId != null, x => x.SchedulerTypeId == input.SchedulerTypeId)
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
