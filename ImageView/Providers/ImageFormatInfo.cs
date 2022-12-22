namespace ImageViewer.Providers;

public class ImageFormatInfo
{
    private static IEnumerable<ImageFormatInfo> _formats;

    private static readonly string[] ImgFormatGuids =
    {
        "ca62b55c-ec91-4ff0-9488-cfa8811758ed",
        "3fee38d4-d399-4590-8ba3-9d670d11753c",
        "b5a200ad-a406-4b55-8d05-620d8d83c557",
        "825b221f-0975-4390-a1d5-4092efcd89ba",
        "3c1124bf-7faa-4e4f-bffc-df471b991e4b",
        "efbdab8a-5407-4078-a375-a61bfbee0dbd",
        "a04b8c1d-b44d-4bda-928e-a59d9b9761bd",
        "cc67a56d-ab9d-42bd-99af-4b54c363f43a",
        "f7e024c5-9085-4b86-a340-60e27ae71ea8",
        "bc8c6862-03ae-47c4-827a-ca189b47b580",
        "9083ac7a-0e2a-4d90-87b4-b9954c199a5b",
        "50fbb940-8617-4abb-b0d6-92450d87bbd1",
        "f3d45fca-42c7-4293-a443-dffbaed10038",
        "439cbb66-ec38-44c9-b7c6-619248bd7e04",
        "e25ed1b9-30cb-4acd-89a6-4aedd12cb303",
        "4f11bacf-ee7f-451c-92ec-a144dfaba791",
        "98f9cc32-a2e8-4514-a5cb-ec14130728aa",
        "850af48a-68e5-4be0-ae0e-15fead8bd36a",
        "2373bb58-23d9-4543-a22f-c23bd81285d5",
        "92334fa8-89f3-4838-b965-9c4e2d314a6a",
        "8b7fd61e-d251-4d24-bbe7-e6dfe200f4ae",
        "0fc617aa-b2f6-4345-b3ea-cb4010518d52",
        "0eab349c-d8ac-4bc9-a5ce-1cdca3afb50d"
    };

    private ImageFormatInfo(string name, string fileExtention, ImageType imgType)
    {
        Name = name;
        FileExtention = fileExtention;
        ImageFormatId = Guid.ParseExact(ImgFormatGuids[(int)imgType], "D");
    }

    public string Name { get; set; }
    public string FileExtention { get; set; }

    public Guid ImageFormatId { get; }

    public static IEnumerable<ImageFormatInfo> GetImageFormatTypes()
    {
        return _formats ?? (_formats = InitList());
    }

    private static IEnumerable<ImageFormatInfo> InitList()
    {
        var imgFormatList = new List<ImageFormatInfo>
        {
            new("Jpeg", "jpg;jpeg", ImageType.Jpeg),
            new("Png", "png", ImageType.Png),
            new("Bitmap", "bmp", ImageType.Bitmap),
            new("Gif", "gif", ImageType.Gif),
            new("Webp", "webp", ImageType.Webp)
        };

        return imgFormatList;
    }
}