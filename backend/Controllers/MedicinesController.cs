using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_myMeds.Data;
using Backend_myMeds.DTOs;
using Backend_myMeds.Models;

namespace Backend_myMeds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MedicinesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineResponseDto>>> GetMedicines()
        {
            var medicines = await _context.Medicines
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            
            return _mapper.Map<List<MedicineResponseDto>>(medicines);
        }

        // GET: api/Medicines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineResponseDto>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return _mapper.Map<MedicineResponseDto>(medicine);
        }

        // POST: api/Medicines
        [HttpPost]
        public async Task<ActionResult<MedicineResponseDto>> PostMedicine(CreateMedicineDto createMedicineDto)
        {
            var medicine = _mapper.Map<Medicine>(createMedicineDto);
            
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<MedicineResponseDto>(medicine);
            
            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, responseDto);
        }

        // PUT: api/Medicines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicine(int id, UpdateMedicineDto updateMedicineDto)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMedicineDto, medicine);
            medicine.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Medicines/search?name=asp
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MedicineResponseDto>>> SearchMedicines([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term is required");
            }

            var medicines = await _context.Medicines
                .Where(m => m.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(m => m.Name)
                .ToListAsync();
            
            return _mapper.Map<List<MedicineResponseDto>>(medicines);
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}