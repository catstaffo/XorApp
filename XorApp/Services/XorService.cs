using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using XorApp.Models;

namespace XorApp.Services
{
    public enum XorSize
    {
        Size8Bit = 1,
        Size16Bit = 2,
        Size32Bit = 4,
        Size64Bit = 8,
        Size128Bit = 16
    };
    public class XorService
    {
        private List<SelectListItem> GetXorSizeSelect()
        {
            List<SelectListItem> selections = new List<SelectListItem>()
            {
                new SelectListItem{Value="1", Text="8-bit"},
                new SelectListItem{Value="2", Text="16-bit"},
                new SelectListItem{Value="4", Text="32-bit"},
                new SelectListItem{Value="8", Text="64-bit"},
                new SelectListItem{Value="16", Text="128-bit"},
            };

            return selections;
        }

        public XorIndexVM BuildXorIndex(XorIndexPM? pm = null)
        {
            var vm = new XorIndexVM()
            {
                FileSize = "",
                XorSize = "1",
                XorSizeSelect = GetXorSizeSelect()
            };

            if (pm is null) return vm;
            
            vm.FilePath = pm.SourceFilePath ?? "";
            vm.XorSize = string.IsNullOrWhiteSpace(pm.XorSize) ? "2" : pm.XorSize;

            if (string.IsNullOrWhiteSpace(vm.CurrentKey)) return vm;

            vm.CurrentValueBinary = KeyAsBinary(pm.CurrentKey);
            vm.CurrentValueHex = KeyAsHex(pm.CurrentKey);
            vm.CurrentValueInt = KeyAsInt(pm.CurrentKey);
            
            return vm;
        }

        public async Task<string> DoXor(XorIndexPM pm)
        {
            if (pm.BinaryFile == null || pm.BinaryFile.Length == 0)
            {
                throw new ArgumentException("No file was uploaded.");
            }

            var fileName = pm.BinaryFile.FileName;
            var destinationPath = Path.Combine(
                Path.GetTempPath(),
                $"XOR_Size{pm.XorSize}_Key{pm.CurrentKey.Replace(" ", "")}_{fileName}"
            );

            using var memoryStream = new MemoryStream();
            await pm.BinaryFile.CopyToAsync(memoryStream);
            var buffer = memoryStream.ToArray();

            var xorSize = int.Parse(pm.XorSize);
            var xorValue = ConvertHexStringToByteArray(pm.CurrentKey);

            for (var i = 0; i < buffer.Length; i += xorSize)
            {
                for (var j = 0; j < xorSize && i + j < buffer.Length; j++)
                {
                    buffer[i + j] ^= xorValue[j % xorValue.Length];
                }
            }

            await File.WriteAllBytesAsync(destinationPath, buffer);
            return destinationPath;
        }

        private byte[] ConvertHexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        private byte[] GenerateRandomKey(int byteLength)
        {
            var randomBytes = new byte[byteLength];

            RandomNumberGenerator.Fill(randomBytes);

            return randomBytes;
        }

        public string BuildKeyView(int bitLength)
        {
            var randomKey = BitConverter.ToString(GenerateRandomKey(bitLength)).Replace("-", "");
            var obj = new
            {
                CurrentKey = KeyAsHex(randomKey),
                CurrentValueHex = KeyAsHex(randomKey),
                CurrentValueBinary = KeyAsBinary(randomKey),
                CurrentValueInt = KeyAsInt(randomKey)
            };
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        private string KeyAsBinary(string hexKey)
        {
            var key = ConvertHexStringToByteArray(hexKey);
            return string.Join("", key.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }

        private string KeyAsInt(string hexKey)
        {
            var key = ConvertHexStringToByteArray(hexKey);
            var paddedKey = key.Concat(new byte[8 - key.Length]).ToArray();
            return BitConverter.ToUInt64(paddedKey, 0).ToString();
        }

        private string KeyAsHex(string hexKey)
        {
            var key = ConvertHexStringToByteArray(hexKey);
            return BitConverter.ToString(key).Replace("-", "");
        }
    }
}
