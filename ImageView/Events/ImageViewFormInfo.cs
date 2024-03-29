﻿namespace ImageViewer.Events;

public class ImageViewFormInfo : ImageViewFormInfoBase
{
    public ImageViewFormInfo(Form formRef) : base(formRef)
    {
        FormHasFocus = true;
    }

    public ImageViewFormInfo(Form formRef, bool lostFocus) : base(formRef)
    {
        FormLostFocus = lostFocus;

        FormHasFocus = !FormLostFocus;
    }

    public bool FormHasFocus { get; }
    public bool FormLostFocus { get; }
}