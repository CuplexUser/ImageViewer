namespace ImageViewer.Services;

public class ProgressEventArgs : EventArgs
{
    public ProgressEventArgs(ProgressStatusEnum progressStatus, int imagesLoaded, int totalNumberOfFiles)
    {
        ProgressStatus = progressStatus;
        ImagesLoaded = imagesLoaded;

        if (totalNumberOfFiles > 0)
        {
            CompletionRate = imagesLoaded / (double)totalNumberOfFiles;
        }
    }

    public ProgressStatusEnum ProgressStatus { get; }
    public int ImagesLoaded { get; }
    public double CompletionRate { get; set; }
}