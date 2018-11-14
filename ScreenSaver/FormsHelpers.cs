﻿
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Aerial
{
    public static class FormsHelpers
    {
        public static void DataBindEnum<T>(this ComboBox combobox, T defaultValue)
        {
            var list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new
                {
                    Value = x,
                    Description =
                        (Attribute.GetCustomAttribute(x.GetType().GetField(x.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description
                        ?? x.ToString()
                })
                .OrderBy(x => x.Value)
                .ToList();
            combobox.DataSource = list;
            combobox.DisplayMember = "Description";
            combobox.ValueMember = "Value";
            combobox.SelectedValue = defaultValue;
        }

        /// <summary>
        /// Gets the bounds of all the specified screens.
        /// </summary>
        /// <param name="screens"></param>
        /// <returns></returns>
        public static Rectangle GetBounds(this Screen[] screens)
        {
            // find edges of all monitors
            var topMost = screens.Min(x => x.Bounds.Top);
            var leftMost = screens.Min(x => x.Bounds.Left);
            var bottomMost = screens.Max(x => x.Bounds.Bottom);
            var rightMost = screens.Max(x => x.Bounds.Right);

            return new Rectangle(
                leftMost, 
                topMost,
                rightMost - leftMost,
                bottomMost - topMost);
        }

        /// <summary>
        /// Sets the libvlc directory to a subdirectory of current directory based on the platform (32 vs 64-bit).
        /// </summary>
        /// <param name="e"></param>
        public static void SetRelativeLibvlcDirectory(this Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(".", "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }
    }
}
