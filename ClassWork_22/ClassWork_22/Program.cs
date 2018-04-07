using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_22
{
    class File
    {
        public string Name { get; set; } // file.txt
        public string Size { get; set; }
        public string Extension
        {
            get
            {
                string[] parts = Name.Split('.');
                return parts[parts.Length-1];
            }
        }

        internal void Parse(string item)
        {
            //Text: file.txt(6B); Some string content
            var data = item.Split(':')[1];
            ParseInternal(data.Split(';'));
        }

        protected virtual void ParseInternal(string[] strs)
        {
            // [0] = file.txt(6B) file.txt 6B
            // [1] = Some string content
            var parts = strs[0].Split('(', ')');
            Name = parts[0];
            Size = parts[1];
        }

        public override string ToString()
        {
            return $@"  {Name}
        Extension: {Extension}
        Size: {Size}";
        }
    }

    class TextFile : File
    {
        public string Content { get; set; }

        protected override void ParseInternal(string[] strs)
        {
            base.ParseInternal(strs);
            Content = strs[1];
        }

        public override string ToString()
        {
            return base.ToString()+ $@"
        Content: {Content}";
        }
    }

    class ImageFile : File
    {
        public string Resolution { get; set; }

        protected override void ParseInternal(string[] strs)
        {
            base.ParseInternal(strs);
            Resolution = strs[1];
        }

        public override string ToString()
        {
            return base.ToString() + $@"
        Resolution: {Resolution}";
        }
    }

    class MovieFile : ImageFile
    {
        public string Duration { get; set; }

        protected override void ParseInternal(string[] strs)
        {
            base.ParseInternal(strs);
            Duration = strs[2];
        }

        public override string ToString()
        {
            return base.ToString() + $@"
        Duration: {Duration}";
        }
    }


    class Program
    {
        static int GetSize(string size) //16B
        {
            string resultStr = String.Empty;
            int x;

            foreach (var item in size)
            {
                if (int.TryParse(item.ToString(), out x))
                {
                    resultStr += item;
                }
            }

            return x=int.Parse(resultStr);
        }

        static void Main(string[] args)
        {
            string text = @"Text:file.txt(6B); Some string content
Image:img.bmp(19MB); 1920х1080
Text:data.txt(12B); Another string
Text:data1.txt(7B); Yet another string
Movie:logan.2017.mkv(19GB); 1920х1080; 2h12m";

            var strs = text.Split('\n');
            File[] result = new File[strs.Length];
            int index = 0;

            foreach (var item in strs)
            {
                string type = item.Split(':')[0];
                File file = null;
                switch (type)
                {
                    case "Image":
                        file = new ImageFile();
                        break;
                    case "Text":
                        file = new TextFile();
                        break;
                    case "Movie":
                        file = new MovieFile();
                        break;
                }
                file.Parse(item);
                result[index] = file;
                index++;
            }

            foreach (var item in result.OrderBy(f => GetSize(f.Size)).ThenBy(f => f.Name))
            {
                Console.WriteLine(item);
            }

        }
    }
}
