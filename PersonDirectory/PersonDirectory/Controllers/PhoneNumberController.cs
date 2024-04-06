using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.DTO;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Model;

namespace PersonDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public PhoneNumberController(IPhoneNumberService phoneNumberService, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _phoneNumberService = phoneNumberService ?? throw new ArgumentNullException(nameof(phoneNumberService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpPost("create")]
        public async Task<ActionResult<PhoneNumberModel>> CreatePhoneNumber(PhoneNumberModel phoneNumberModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => _localizer[e.ErrorMessage].Value)
                                                  .ToList();

                    return BadRequest(errors);
                }
                var phoneNumberDTO = _mapper.Map<PhoneNumber>(phoneNumberModel);
                var createdPhoneNumber = await _phoneNumberService.CreatePhoneNumber(phoneNumberDTO);

                return Ok(createdPhoneNumber);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["PhoneNumberNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePhoneNumber(int id, PhoneNumberModel phoneNumberModel)
        {
            if (id != phoneNumberModel.Id)
            {
                return BadRequest();
            }
            var phoneNumberDTO = _mapper.Map<PhoneNumber>(phoneNumberModel);
            try
            {
                await _phoneNumberService.UpdatePhoneNumber(phoneNumberDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["PhoneNumberNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePhoneNumber(int id)
        {
            try
            {
                await _phoneNumberService.DeletePhoneNumber(id);
                return Ok(_localizer["PhoneNumberDeletedSuccessfully"].Value);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["PhoneNumberNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
