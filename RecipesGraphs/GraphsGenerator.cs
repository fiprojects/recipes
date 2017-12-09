using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesGraphs
{
    public class GraphsGenerator
    {
        private readonly IRecipesService _recipesService;

        public GraphsGenerator(IRecipesService recipesServ)
        {
            _recipesService = recipesServ;
            if (_recipesService == null) Console.Out.WriteLine("RecipesService == NULL");
            
        }

        public void generateGraphs()
        {

            string filename = "Graph";//+filename.Substring(filename.LastIndexOf('\\')
            
            List<Recipe> recipes = _recipesService.GetAll()
                .OrderByDescending(a => a.Rating).ToList();
            
            foreach (var r in recipes)
            {
                Console.Out.WriteLine(r.Rating);
            }
            using (FileStream destinationStream = File.OpenWrite(filename))
            {
                
                //DataPoint dp = new DataPoint();
                //dp.SetValueXY(100, 200);
                //List<DataPoint> dataPoints = new List<DataPoint>();
                //dataPoints.Add(dp);
                //generatePlot(dataPoints, destinationStream);
            }


        }


        //public void generatePlot(IList<DataPoint> series, FileStream outputStream)
        //{
        //    using (var ch = new Chart())
        //    {
        //        ch.ChartAreas.Add(new ChartArea());
        //        var s = new Series();
        //        foreach (var pnt in series) s.Points.Add(pnt);
        //        ch.Series.Add(s);
        //        ch.SaveImage(outputStream, ChartImageFormat.Jpeg);
        //    }
        //}





    }
}
