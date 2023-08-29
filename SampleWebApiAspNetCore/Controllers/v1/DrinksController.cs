using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<DrinksController> _linkService;

        public DrinksController(
            IDrinkRepository drinkRepository,
            IMapper mapper,
            ILinkService<DrinksController> linkService)
        {
            _drinkRepository = drinkRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleDrink))]
        public ActionResult GetSingleDrink(ApiVersion version, int id)
        {
            DrinkEntity drinkItem = _drinkRepository.GetSingle(id);

            if (drinkItem == null)
            {
                return NotFound();
            }

            DrinkDto item = _mapper.Map<DrinkDto>(drinkItem);

            return Ok(_linkService.ExpandSingleItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddDrink))]
        public ActionResult<DrinkDto> AddDrink(ApiVersion version, [FromBody] DrinkCreateDto drinkCreateDto)
        {
            if (drinkCreateDto == null)
            {
                return BadRequest();
            }

            DrinkEntity toAdd = _mapper.Map<DrinkEntity>(drinkCreateDto);

            _drinkRepository.Add(toAdd);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Creating a drinkitem failed on save.");
            }

            DrinkEntity newDrinkItem = _drinkRepository.GetSingle(toAdd.Id);
            DrinkDto drinkDto = _mapper.Map<DrinkDto>(newDrinkItem);

            return CreatedAtRoute(nameof(GetSingleDrink),
                new { version = version.ToString(), id = newDrinkItem.Id },
                _linkService.ExpandSingleItem(drinkDto, drinkDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveDrink))]
        public ActionResult RemoveDrink(int id)
        {
            DrinkEntity drinkItem = _drinkRepository.GetSingle(id);

            if (drinkItem == null)
            {
                return NotFound();
            }

            _drinkRepository.Delete(id);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Deleting a drinkitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateDrink))]
        public ActionResult<DrinkDto> UpdateDrink(ApiVersion version, int id, [FromBody] DrinkUpdateDto drinkUpdateDto)
        {
            if (drinkUpdateDto == null)
            {
                return BadRequest();
            }

            var existingDrinkItem = _drinkRepository.GetSingle(id);

            if (existingDrinkItem == null)
            {
                return NotFound();
            }

            _mapper.Map(drinkUpdateDto, existingDrinkItem);

            _drinkRepository.Update(id, existingDrinkItem);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Updating a drinkitem failed on save.");
            }

            DrinkDto drinkDto = _mapper.Map<DrinkDto>(existingDrinkItem);

            return Ok(_linkService.ExpandSingleItem(drinkDto, drinkDto.Id, version));
        }
    }
}
