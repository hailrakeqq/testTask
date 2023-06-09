using TestTask.Models;

namespace TestTask.Services;

public class FileServices
{
    public async Task<bool> CreateJsonFromString(Catalog catalog,string json)
    {
        string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "ExportedCatalogs", $"{catalog.Name.Replace(" ", "")}.json");
        if (!System.IO.File.Exists(pathToFile))
        {
            using (StreamWriter writer = new StreamWriter(pathToFile))
            {
                writer.Write(json);
            }
            return true;
        }

        return false;
    }

    public async Task<string> GetStringFromUploadedJson(IFormFile file)
    {
        if (file != null && file.Length > 0 && file.ContentType == "application/json")
        { 
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ImportedCatalogs", fileName);
        
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        
            string json = System.IO.File.ReadAllText(filePath);

            return json;
        }

        return null;
    }
    public async Task<byte[]> ReadStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}