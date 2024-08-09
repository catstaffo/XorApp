using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XorApp.Models
{
    public class XorIndexVM
    {
        public string? FilePath { get; set; }
        public string FileSize { get; set; }
        public string XorSize { get; set; }
        public IEnumerable<SelectListItem> XorSizeSelect { get; set; }
        public string? CurrentKey { get; set; }
        public string? CurrentValueInt { get; set; }
        public string? CurrentValueHex { get; set; }
        public string? CurrentValueBinary { get; set; }
        public IFormFile? BinaryFile { get; set; }
    }

    public class XorIndexPM
    {
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public IFormFile BinaryFile { get; set; }
        public string XorSize { get; set; }
        public string CurrentKey { get; set; }
    }
}
