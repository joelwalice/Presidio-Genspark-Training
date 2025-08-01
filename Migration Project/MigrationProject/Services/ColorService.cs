using AutoMapper;
using MigrationProject.Interfaces;
using MigrationProject.DTOs;
using MigrationProject.Models;
using MigrationProject.Data;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Services;

public class ColorService : IColorService
{
    private readonly MigrationContexts _context;
    private readonly IMapper _mapper;

    public ColorService(MigrationContexts context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ColorReadDto>> GetAllAsync()
    {
        var colors = await _context.Colors.ToListAsync();
        return _mapper.Map<IEnumerable<ColorReadDto>>(colors);
    }

    public async Task<ColorReadDto?> GetByIdAsync(int id)
    {
        var color = await _context.Colors.FindAsync(id);
        return color == null ? null : _mapper.Map<ColorReadDto>(color);
    }

    public async Task<ColorReadDto> CreateAsync(ColorCreateDto dto)
    {
        var color = _mapper.Map<Color>(dto);
        _context.Colors.Add(color);
        await _context.SaveChangesAsync();
        return _mapper.Map<ColorReadDto>(color);
    }

    public async Task<bool> UpdateAsync(int id, ColorUpdateDto dto)
    {
        var color = await _context.Colors.FindAsync(id);
        if (color == null) return false;

        _mapper.Map(dto, color);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var color = await _context.Colors.FindAsync(id);
        if (color == null) return false;

        _context.Colors.Remove(color);
        await _context.SaveChangesAsync();
        return true;
    }
}
