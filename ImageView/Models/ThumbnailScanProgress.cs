namespace ImageViewer.Models;

public class ThumbnailScanProgress
{
    public int TotalAmountOfFiles { get; set; }

    public int ScannedFiles { get; set; }

    public int PercentComplete
    {
        get
        {
            var rate = 0;
            if (TotalAmountOfFiles > 0) rate = Convert.ToInt32((double)ScannedFiles / TotalAmountOfFiles * 100);

            return rate;
        }
    }

    public bool IsComplete { get; set; }
}