using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;

public class FirebaseStorageService
{
    private readonly string bucket = "programacionavanzada2026.firebasestorage.app"; // ⚠️ IMPORTANTE
    private readonly string credPath = "wwwroot/firebase/firebase-key.json";

    public async Task<string> SubirImagenAsync(IFormFile archivo)
    {
        var credential = GoogleCredential.FromFile(credPath);
        var storage = await StorageClient.CreateAsync(credential);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);

        using (var stream = archivo.OpenReadStream())
        {
            await storage.UploadObjectAsync(
                bucket,
                fileName,
                null,
                stream,
                new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead // 🔥 AQUÍ ES LA CLAVE
                }
            );
        }

        var url = $"https://storage.googleapis.com/{bucket}/{fileName}";

        return url;
    }
}