using AutoMapper;
using AutoMapper.QueryableExtensions;
using MigrationProject.Data;
using MigrationProject.DTOs;
using MigrationProject.Interfaces;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Services;

public class ContactUsService : IContactUsService
{
    private readonly MigrationContexts _context;
    private readonly IMapper _mapper;

    public ContactUsService(MigrationContexts context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactUsReadDto>> GetAllAsync()
    {
        return await _context.ContactUs
            .ProjectTo<ContactUsReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ContactUsReadDto?> GetByIdAsync(int id)
    {
        var contact = await _context.ContactUs.FindAsync(id);
        return contact == null ? null : _mapper.Map<ContactUsReadDto>(contact);
    }

    public async Task<ContactUsReadDto> CreateAsync(ContactUsCreateDto dto)
    {
        var contact = _mapper.Map<ContactUs>(dto);
        _context.ContactUs.Add(contact);
        await _context.SaveChangesAsync();
        return _mapper.Map<ContactUsReadDto>(contact);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _context.ContactUs.FindAsync(id);
        if (contact == null) return false;
        _context.ContactUs.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}
