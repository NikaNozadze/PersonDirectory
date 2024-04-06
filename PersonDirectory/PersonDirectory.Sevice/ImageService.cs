using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;

namespace PersonDirectory.Service
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _imageUploadPath;

        public ImageService(IUnitOfWork unitOfWork, IHostEnvironment environment)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _imageUploadPath = Path.Combine(environment.ContentRootPath, "Images");
        }

        public async Task<string> GetImagePathAsync(int personId)
        {
            if (personId == 0) throw new ArgumentException("Person ID cannot be zero.");

            var person = await _unitOfWork.PersonRepository.GetAsync(personId);
            if (person == null) throw new ArgumentException($"A person with this ID was not found.");

            return person.ImagePath;
        }

        public async Task<string> InsertImageAsync(int personId, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) throw new ArgumentException("Image file is required.");
            if (personId == 0) throw new ArgumentException($"Person Id required.");

            var fileName = $"{personId}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(_imageUploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await imageFile.CopyToAsync(stream);

            var person = await _unitOfWork.PersonRepository.GetAsync(personId) ?? throw new ArgumentException($"A person with this ID was not found.");
            person.ImagePath = filePath;
            await _unitOfWork.SaveChangesAsync();

            return filePath;
        }

        public async Task<string> UpdateImageAsync(int personId, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) throw new ArgumentException("Image file is required.");
            if (personId == 0) throw new ArgumentException($"Person Id required.");

            var person = await _unitOfWork.PersonRepository.GetAsync(personId)
                         ?? throw new ArgumentException($"A person with this ID was not found.");

            if (!string.IsNullOrEmpty(person.ImagePath))
                if (File.Exists(person.ImagePath))
                    File.Delete(person.ImagePath);

            var fileName = $"{personId}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(_imageUploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await imageFile.CopyToAsync(stream);

            person.ImagePath = filePath;
            await _unitOfWork.SaveChangesAsync();

            return filePath;
        }


        public async Task DeleteImageAsync(int personId)
        {
            var person = await _unitOfWork.PersonRepository.GetAsync(personId)
                         ?? throw new ArgumentException($"A person with this ID was not found.");

            if (File.Exists(person.ImagePath))
                File.Delete(person.ImagePath);

            person.ImagePath = "";
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
