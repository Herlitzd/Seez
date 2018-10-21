using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Seez.Models;
using System.Linq;

namespace Seez
{
  public class ReceiptEngine
  {
    public static Receipt Create(OcrResult ocrResult)
    {
      var groups = GroupIntoRows(ocrResult);
      // var receipt = new Receipt { Items = }

      return null;
    }

    private static int roundToNearest(int value, int nearest)
    {
      int offset = value % nearest;
      return value - offset;
    }
    public static IDictionary<int, IList<String>> GroupIntoRows(OcrResult ocrResult)
    {
      Dictionary<int, IList<String>> lines = new Dictionary<int, IList<string>>();


      foreach (var region in ocrResult.Regions)
      {
        foreach (var line in region.Lines)
        {
          // Bounding boxes are "x,y,w,h"
          var y = int.Parse(line.BoundingBox.Split(',')[1]);
          var roundedY = roundToNearest(y, 5);
          var lineText = line.Words.Aggregate(String.Empty, (acc, next) => acc += next.Text);
          if (lines.ContainsKey(roundedY))
          {
            lines[roundedY].Add(lineText);
          }
          else
          {
            lines.Add(roundedY, new List<String>() { lineText });
          }
        }
      }

      return lines;
    }
  }
}