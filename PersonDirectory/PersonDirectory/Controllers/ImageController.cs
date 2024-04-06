using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PersonDirectory.API.Resources;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ImageController(IImageService imageService, IStringLocalizer<SharedResources> localizer)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [HttpGet("get/{personId}")]
        public async Task<IActionResult> GetImage(int personId)
        {
            try
            {
                var imagePath = await _imageService.GetImagePathAsync(personId);
                return PhysicalFile(imagePath, "image/jpeg");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["ImageNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpPost("upload/{personId}")]
        public async Task<IActionResult> InsertImage(int personId, IFormFile imageFile)
        {
            try
            {
                var filePath = await _imageService.InsertImageAsync(personId, imageFile);
                return Ok(filePath);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["ImageNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpPut("update/{personId}")]
        public async Task<IActionResult> UpdateImage(int personId, IFormFile imageFile)
        {
            try
            {
                var filePath = await _imageService.UpdateImageAsync(personId, imageFile);
                return Ok(filePath);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(_localizer["ImageNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }

        [HttpDelete("delete/{personId}")]
        public async Task<IActionResult> DeleteImage(int personId)
        {
            try
            {
                await _imageService.DeleteImageAsync(personId);
                return Ok(_localizer["ImageDeletedSuccessfully"].Value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(_localizer["ImageNotFoundMessage"].Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _localizer[ex.Message].Value);
            }
        }
    }
}
