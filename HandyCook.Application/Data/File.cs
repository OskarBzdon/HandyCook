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

        public static async Task<File> CreateAsync(IBrowserFile file)
        {
            var instance = new File();
            using (var stream = file.OpenReadStream(file.Size))
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                instance.Bytes = memoryStream.ToArray();
            }

            instance.Name = file.Name;
            instance.Format = file.ContentType;

            return instance;
        }
    }
}
