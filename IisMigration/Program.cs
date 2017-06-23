using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace IisMigration
{
    class Program
    {
        static void Main(string [] args)
        {
            if (args.Length != 2) return;

            try
            {
                var file = args[1];

                var appCmd = new AppCmd(ConfigurationManager.AppSettings["AppCmd"]);

                var serializer = new XmlSerializer(typeof(Iis));

                switch (args[0].ToLower())
                {
                    case "/import":
                        using (var reader = new StreamReader(file))
                        {
                            var iis = (Iis)serializer.Deserialize(reader);
                        }
                        break;

                    case "/export":
                        using (var writer = new StreamWriter(file, false))
                        {
                            serializer.Serialize(writer, Iis.Export(appCmd));
                        }
                        break;
                }
            }

            catch (Exception e)
            {
                var message = e.Message;

                if (e.InnerException != null)
                {
                    message += Environment.NewLine;
                    message += e.InnerException.Message;
                }

                Console.WriteLine("Exception: {0}", message);
            }
        }
    }
}
