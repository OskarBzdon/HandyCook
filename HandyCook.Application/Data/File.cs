using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace HandyCook.Application.Data
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string Format { get; set; }

        public File()
        {
                
        }

        public File(byte[] bytes)
        {
            this.Bytes = bytes;
        }

        public File(IBrowserFile file)
        {
            var stream = file.OpenReadStream(file.Size);
            var memoryStream = new MemoryStream();
            memoryStream.CopyToAsync(stream).RunSynchronously();

            Bytes = memoryStream.ToArray();
            Name = file.Name;
            Format = file.ContentType;
        }
    }
}
