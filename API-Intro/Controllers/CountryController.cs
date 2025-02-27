﻿using API_Intro.Data;
using API_Intro.DTOs.Countries;
using API_Intro.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Intro.Controllers
{

    public class CountryController : BaseController
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CountryController(AppDbContext context,
                                 IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Countries.AsNoTracking().ToListAsync();

            var mappedDatas = _mapper.Map<List<CountryDto>>(result);

            return Ok(mappedDatas);


        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Countries.AddAsync(_mapper.Map<Country>(request));  //new Country { Name = request.Name, Population = request.Population }
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(Create), request);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromBody] CountryEditDto request)
        {
            var entity = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null) return NotFound();

            _mapper.Map(request, entity);

            _context.Countries.Update(entity);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(m=> m.Id == id);
            if (entity == null) return NotFound();

            return Ok(_mapper.Map<CountryDto>(entity));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id == null) return BadRequest();

            var entity = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();

            _context.Countries.Remove(entity);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
    }
}
