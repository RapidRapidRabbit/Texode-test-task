using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnimalsApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace AnimalsApi.Validators
{
    public class AnimalRequestModelValidator : AbstractValidator<AnimalRequestModel>
    {
        public AnimalRequestModelValidator()
        {
            RuleFor(model => model.File).Must(ValidImageExtension).WithMessage("Wrong image extension");
            RuleFor(model => model.File).Must(ValidImageSize).WithMessage("Wrong image size");
        }

        private bool ValidImageExtension(object value)
        {
            if (value == null)
                return true;

            Dictionary<string, List<byte[]>> fileSignatures = new Dictionary<string, List<byte[]>> {

                {".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                } },
                {".bmp", new List<byte[]>
                {
                    new byte[] { 0x42, 0x4D },
                } },
                {".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                } },
                {".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                } },
            };

            var file = value as IFormFile;
            var ext = Path.GetExtension(file.FileName).ToLower();


            if (!fileSignatures.ContainsKey(ext))
            {
                return false;
            }

            using var reader = new BinaryReader(file.OpenReadStream());
            var signatures = fileSignatures[ext];

            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
        }

        private bool ValidImageSize(object value)
        {
            const double maxImageSize = 5;
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > 1048576 * maxImageSize)  // 1MB * maxImageSize        
                    return false;
            }

            return true;
        }

    }
}
