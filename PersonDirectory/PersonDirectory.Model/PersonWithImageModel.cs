using Microsoft.AspNetCore.Mvc;

namespace PersonDirectory.Model
{
    public class PersonWithImageModel
    {
        public PersonModel Person { get; set; }
        public PhysicalFileResult? ImageUrl { get; set; }
    }
}
