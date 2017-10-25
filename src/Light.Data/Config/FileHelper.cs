﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Light.Data
{
    class FileHelper
    {
        public static FileInfo GetFileInfo(string path, out bool absolute)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            char c = path[0];
            if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
            {
                //绝对地址
                absolute = true;
                return new FileInfo(path);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT && path.Length >= 2 && path[1] == ':' && ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
            {
                //win 带盘符的绝对地址 C:
                absolute = true;
                return new FileInfo(path);
            }
            else
            {
                //相对地址
                absolute = false;
                string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string newpath = Path.Combine(currentDirectory, path);
                return new FileInfo(newpath);
            }
        }
    }
}
