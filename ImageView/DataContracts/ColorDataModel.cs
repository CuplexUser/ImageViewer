using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "ColorDataModel")]
    public class ColorDataModel
    {
        /// <summary>
        /// Gets or sets the r.
        /// </summary>
        /// <value>
        /// The r.
        /// </value>
        [DataMember(Name = "R", Order = 1)]
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        /// <value>
        /// The g.
        /// </value>
        [DataMember(Name = "G", Order = 2)]
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>
        /// The b.
        /// </value>
        [DataMember(Name = "B", Order = 3)]
        public byte B { get; set; }

        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>
        /// a.
        /// </value>
        [DataMember(Name = "A", Order = 4)]
        public byte A { get; set; }

        /// <summary>
        /// Creates from color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static ColorDataModel CreateFromColor(Color color)
        {
            return new ColorDataModel
            {
                R=color.R,
                G= color.G,
                B= color.B,
                A = color.A
            };
        }

        /// <summary>
        /// Creates from color data model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Color CreateFromColorDataModel(ColorDataModel model)
        {
            return Color.FromArgb(model.A, model.R, model.G, model.B);
        }

        /// <summary>
        /// To the color.
        /// </summary>
        /// <returns></returns>
        public Color ToColor()
        {
            return CreateFromColorDataModel(this);
        }
    }
}