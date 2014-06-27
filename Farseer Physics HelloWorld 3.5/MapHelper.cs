using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarseerPhysics.Samples
{
    public static class MapHelper
    {

        public static void Generate()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../../Maps/map01.sav");

            // Format : Width-Height-Name-ToDo
            // Ex :     20-10-DemoMap
            // Format : Type - TexID - SizeX - SizeY - CellType(TODO)

            // Create level content

            int w = 20, h = 10;


            string[,] tempMap = new string[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    tempMap[y, x] = "00-0000-00-00-00";
                }
            }

            tempMap[0, 0] = "01-0001-08-01-01";
            tempMap[0, 4] = "01-0001-08-01-01";
            tempMap[0, 8] = "01-0001-08-01-01";
            tempMap[0, 12] = "01-0001-08-01-01";
            tempMap[0, 16] = "01-0001-08-01-01";

            tempMap[4, 0] = "03-0001-08-01-01";

            // Wrtie to file
            file.WriteLine("20-10-DemoMap");
            for (int y = 0; y < h; y++)
            {
                file.WriteLine("#Column " + y);
                for (int x = 0; x < w; x++)
                {

                    file.WriteLine(tempMap[y, x]);
                }
            }

            file.Close();
        }
    }
}
