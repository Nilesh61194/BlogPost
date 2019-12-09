using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JsonDemo
{
    public partial class ReadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var fileStream = new FileStream(@"d:\file.txt", FileMode.Open, FileAccess.Read);

                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {

                    string line;
                    dynamic flexible = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)flexible;
                    bool ReadMore = false;
                    string content = "";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // process the line

                        if (line.Contains("---"))
                        {

                        }
                        else
                        {

                            string[] words = line.Split(':');

                            int cnt = words.Length;

                            if (cnt >= 2)
                            {
                                dictionary.Add(words[0], words[1]);
                            }
                            else
                            {
                                string check = words[0];

                                if (check != "")
                                {
                                    if (check == "READMORE")
                                    {
                                        ReadMore = true;

                                    }

                                    if (ReadMore)
                                    {
                                        if (words[0] != "READMORE")
                                        {
                                            content = content + words[0];
                                        }

                                    }
                                    else
                                    {
                                        dictionary.Add("short-content", words[0]);
                                    }
                                }
                            }

                        }


                    }

                    if (content != "")
                    {
                        dictionary.Add("content", content);
                    }

                    var serialized = JsonConvert.SerializeObject(dictionary);

                    string path = @"D:\blogfile.json";

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        using (var tw = new StreamWriter(path, true))
                        {
                            tw.WriteLine(serialized.ToString());
                            tw.Close();
                        }
                    }
                    else if (!File.Exists(path))
                    {
                        using (var tw = new StreamWriter(path, true))
                        {
                            tw.WriteLine(serialized.ToString());
                            tw.Close();
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                string errmsg = ex.Message;
            }

        }


    }
}
