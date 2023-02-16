using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CBUnZip
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Get directory zip file
            DirectoryInfo ZipDirectory = new DirectoryInfo(ConfigurationManager.AppSettings.Get("PathCBOutput"));
            FileInfo[] Zipfiles = ZipDirectory.GetFiles("*.zip");
            //
            try
            {
                foreach (FileInfo file in Zipfiles)
                {
                    // Create folder and Extract from zip
                    string ZipPath = file.FullName;
                    string Zipfile = file.Name;
                    string IndexWordRemove = Zipfile;
                    IndexWordRemove = IndexWordRemove.Remove(IndexWordRemove.IndexOf("."));
                    string ExtractPath = ConfigurationManager.AppSettings.Get("PathWaferMapping") + IndexWordRemove;
                    if (!Directory.Exists(ExtractPath)) // Check duplicate folder
                    {
                        //Create folder and unzip
                        Directory.CreateDirectory(ExtractPath);
                        ZipFile.ExtractToDirectory(ZipPath, ExtractPath);
                        // 
                        //Create Log File
                        string LogFilePath = ConfigurationManager.AppSettings.Get("Logfile");
                        using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                        using (StreamWriter WriteFile = new StreamWriter(FileLog))
                        {
                            WriteFile.WriteLine(DateTime.Now);
                            WriteFile.WriteLine(IndexWordRemove + " Unzip Success");
                            WriteFile.WriteLine("-----------------------------------------------------------------");
                        }
                        //Rename to .bak
                        string Sourcezip = ZipPath;
                        string RenameZip = ZipPath + ".bak";
                        try
                        {
                            File.Move(Sourcezip, RenameZip);
                        }
                        catch (IOException e)
                        {
                            //Create Log File
                            using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                            using (StreamWriter WriteFile = new StreamWriter(FileLog))
                            {
                                WriteFile.WriteLine(DateTime.Now);
                                WriteFile.WriteLine("Rename Error");
                                WriteFile.WriteLine(e.Message);
                                WriteFile.WriteLine("{0} Exception caught.", e);
                                WriteFile.WriteLine("-----------------------------------------------------------------");
                            }
                        }
                    }
                    else
                    {
                        // Unzip zip Exist folder
                        using (ZipArchive source = ZipFile.Open(ZipPath, ZipArchiveMode.Read, null))
                        {
                            foreach (ZipArchiveEntry entry in source.Entries)
                            {
                                string fullPath = Path.GetFullPath(Path.Combine(ExtractPath, entry.FullName));

                                if (Path.GetFileName(fullPath).Length != 0)
                                {
                                    entry.ExtractToFile(fullPath, true);
                                }
                            }
                        }
                        // Exist folder
                        DirectoryInfo FileDatDirectory = new DirectoryInfo(ConfigurationManager.AppSettings.Get("PathWaferMapping") + IndexWordRemove);

                        FileInfo[] DatFile = FileDatDirectory.GetFiles("W-NO-*.DAT");

                        if (DatFile != null)
                        {
                            foreach (FileInfo GetNumberDat in DatFile)
                            {
                                FileInfo[] GetNumberFile = DatFile;
                                int CountDatFile = GetNumberFile.Count();
                                string Lotdotdatpath = ConfigurationManager.AppSettings.Get("PathWaferMapping") + IndexWordRemove + @"\LOT.DAT";
                                using (BinaryWriter writer = new BinaryWriter(File.Open(Lotdotdatpath, FileMode.Open, FileAccess.ReadWrite)))
                                {
                                    if (CountDatFile == 25)
                                    {
                                        int offset = 38; //position you want to start editing
                                        byte[] set_data = new byte[] { 0x01,0x00,0x02,0x00,0x03,0x00,0x04,0x00,0x05,0x00,0x06,0x00,0x07,0x00,
                                                                   0x08,0x00,0x09,0x00,0x10,0x00,0x11,0x00,0x12,0x00,0x13,0x00,0x14,0x00,
                                                                   0x15,0x00,0x16,0x00,0x17,0x00,0x18,0x00,0x19,0x00,0x20,0x00,0x21,0x00,
                                                                   0x22,0x00,0x23,0x00,0x24,0x00,0x25,0x00 }; //new data
                                        writer.Seek(offset, SeekOrigin.Begin); //move your cursor to the position
                                        writer.Write(set_data); //write it     
                                    }
                                    else
                                    {
                                        int CheckNumberofWafer = 1;
                                        foreach (FileInfo GetDatnumber in DatFile)
                                        {
                                            string GetNumberWafer = GetDatnumber.Name.Substring(5, 2);
                                            int GetIntNumberDat = Convert.ToInt32(GetNumberWafer);
                                            byte GetByteNumberDat = Convert.ToByte(GetIntNumberDat);
                                            byte ZeroData = Convert.ToByte(0);
                                        JumpForLoopSetData: //lopp for set data to correct position
                                            if (GetByteNumberDat == CheckNumberofWafer)
                                            {
                                                if (GetByteNumberDat == 1)
                                                {
                                                    writer.Seek(38, SeekOrigin.Begin);
                                                    writer.Write(1);
                                                }
                                                else if (GetByteNumberDat == 2)
                                                {
                                                    writer.Seek(40, SeekOrigin.Begin);
                                                    writer.Write(2);
                                                }
                                                else if (GetByteNumberDat == 3)
                                                {
                                                    writer.Seek(42, SeekOrigin.Begin);
                                                    writer.Write(3);
                                                }
                                                else if (GetByteNumberDat == 4)
                                                {
                                                    writer.Seek(44, SeekOrigin.Begin);
                                                    writer.Write(4);
                                                }
                                                else if (GetByteNumberDat == 5)
                                                {
                                                    writer.Seek(46, SeekOrigin.Begin);
                                                    writer.Write(5);
                                                }
                                                else if (GetByteNumberDat == 6)
                                                {
                                                    writer.Seek(48, SeekOrigin.Begin);
                                                    writer.Write(6);
                                                }
                                                else if (GetByteNumberDat == 7)
                                                {
                                                    writer.Seek(50, SeekOrigin.Begin);
                                                    writer.Write(7);
                                                }
                                                else if (GetByteNumberDat == 8)
                                                {
                                                    writer.Seek(52, SeekOrigin.Begin);
                                                    writer.Write(8);
                                                }
                                                else if (GetByteNumberDat == 9)
                                                {
                                                    writer.Seek(54, SeekOrigin.Begin);
                                                    writer.Write(9);
                                                }
                                                else if (GetByteNumberDat == 10)
                                                {
                                                    writer.Seek(56, SeekOrigin.Begin);
                                                    writer.Write(16);
                                                }
                                                else if (GetByteNumberDat == 11)
                                                {
                                                    writer.Seek(58, SeekOrigin.Begin);
                                                    writer.Write(17);
                                                }
                                                else if (GetByteNumberDat == 12)
                                                {
                                                    writer.Seek(60, SeekOrigin.Begin);
                                                    writer.Write(18);
                                                }
                                                else if (GetByteNumberDat == 13)
                                                {
                                                    writer.Seek(62, SeekOrigin.Begin);
                                                    writer.Write(19);
                                                }
                                                else if (GetByteNumberDat == 14)
                                                {
                                                    writer.Seek(64, SeekOrigin.Begin);
                                                    writer.Write(20);
                                                }
                                                else if (GetByteNumberDat == 15)
                                                {
                                                    writer.Seek(66, SeekOrigin.Begin);
                                                    writer.Write(21);
                                                }
                                                else if (GetByteNumberDat == 16)
                                                {
                                                    writer.Seek(68, SeekOrigin.Begin);
                                                    writer.Write(22);
                                                }
                                                else if (GetByteNumberDat == 17)
                                                {
                                                    writer.Seek(70, SeekOrigin.Begin);
                                                    writer.Write(23);
                                                }
                                                else if (GetByteNumberDat == 18)
                                                {
                                                    writer.Seek(72, SeekOrigin.Begin);
                                                    writer.Write(24);
                                                }
                                                else if (GetByteNumberDat == 19)
                                                {
                                                    writer.Seek(74, SeekOrigin.Begin);
                                                    writer.Write(25);
                                                }
                                                else if (GetByteNumberDat == 20)
                                                {
                                                    writer.Seek(76, SeekOrigin.Begin);
                                                    writer.Write(32);
                                                }
                                                else if (GetByteNumberDat == 21)
                                                {
                                                    writer.Seek(78, SeekOrigin.Begin);
                                                    writer.Write(33);
                                                }
                                                else if (GetByteNumberDat == 22)
                                                {
                                                    writer.Seek(80, SeekOrigin.Begin);
                                                    writer.Write(34);
                                                }
                                                else if (GetByteNumberDat == 23)
                                                {
                                                    writer.Seek(82, SeekOrigin.Begin);
                                                    writer.Write(35);
                                                }
                                                else if (GetByteNumberDat == 24)
                                                {
                                                    writer.Seek(84, SeekOrigin.Begin);
                                                    writer.Write(36);
                                                }
                                                else if (GetByteNumberDat == 25)
                                                {
                                                    writer.Seek(86, SeekOrigin.Begin);
                                                    writer.Write(37);
                                                }
                                                else
                                                {
                                                    CheckNumberofWafer++;
                                                }
                                            }
                                            else
                                            {
                                                writer.Seek(0, SeekOrigin.Current); //move your cursor to the position
                                                writer.Write(ZeroData); //write data  
                                                CheckNumberofWafer++;
                                                goto JumpForLoopSetData;
                                            }
                                        }
                                    }
                                    try
                                    {
                                        //Convert lot name Ascii to Hex Data 
                                        string AsciiString = IndexWordRemove;
                                        byte[] bytes = Encoding.Default.GetBytes(AsciiString);
                                        int CharCount = AsciiString.Length;
                                        for (int CharPosition = 1; CharPosition <= CharCount; CharPosition++)
                                        {
                                            writer.Seek(0, SeekOrigin.Begin); //move your cursor to the position
                                            writer.Write(bytes); //write it     
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //Create Log File
                                        string LogFileconvertPath = ConfigurationManager.AppSettings.Get("Logfile");
                                        using (FileStream FileLog = new FileStream(LogFileconvertPath, FileMode.Append, FileAccess.Write))
                                        using (StreamWriter WriteFile = new StreamWriter(FileLog))
                                        {
                                            WriteFile.WriteLine(DateTime.Now);
                                            WriteFile.WriteLine("Convert Name Error");
                                            WriteFile.WriteLine(e.Message);
                                            WriteFile.WriteLine("{0} Exception caught.", e);
                                            WriteFile.WriteLine("-----------------------------------------------------------------");
                                        }
                                    }
                                }
                            }
                        }
                        //Create Log File
                        string LogFilePath = ConfigurationManager.AppSettings.Get("Logfile");
                        using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                        using (StreamWriter WriteFile = new StreamWriter(FileLog))
                        {
                            WriteFile.WriteLine(DateTime.Now);
                            WriteFile.WriteLine(IndexWordRemove + " Unzip Success");
                            WriteFile.WriteLine("-----------------------------------------------------------------");
                        }
                        //Rename to .bak
                        string Sourcezip = ZipPath;
                        string RenameZip = ZipPath + ".bak";
                        try
                        {
                            if (File.Exists(RenameZip))
                            {
                                string NewName = RenameZip.Remove(RenameZip.IndexOf(".2"));
                                string SetName = NewName + "." + (DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip.bak");
                                File.Move(Sourcezip, SetName);
                            }
                            else
                            {
                                File.Move(Sourcezip, RenameZip);
                            }
                        }
                        catch (IOException e)
                        {
                            //Create Log File
                            using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                            using (StreamWriter WriteFile = new StreamWriter(FileLog))
                            {
                                WriteFile.WriteLine(DateTime.Now);
                                WriteFile.WriteLine("Rename Error");
                                WriteFile.WriteLine(e.Message);
                                WriteFile.WriteLine("{0} Exception caught.", e);
                                WriteFile.WriteLine("-----------------------------------------------------------------");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Create Log File
                string LogFilePath = ConfigurationManager.AppSettings.Get("Logfile");
                using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter WriteFile = new StreamWriter(FileLog))
                {
                    WriteFile.WriteLine(DateTime.Now);
                    WriteFile.WriteLine("Unzip Error");
                    WriteFile.WriteLine(e.Message);
                    WriteFile.WriteLine("{0} Exception caught.", e);
                    WriteFile.WriteLine("-----------------------------------------------------------------");
                }
            }
        }
    }
}