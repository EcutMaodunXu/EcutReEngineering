using EcutController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility
{
    public class gCodeParser
    {
        public static List<MoveInfoStruct>ParseCode(string text)
        {
            var moveInfoList = new List<MoveInfoStruct>();
            
            text = text.Replace(" ", "").ToUpper();
            text = text.Replace("N[0-9]*", "");
            
            var gCodeArray = text.Split('\n');
            //var gCodeArray = text.Split('\r');
            int traverseLength = 0;
            if(gCodeArray[gCodeArray.Length - 1] == "")
            { 
                traverseLength = gCodeArray.Length - 1;
            }
            else
            {
                traverseLength = gCodeArray.Length;
            }
            for (int i = 0; i < traverseLength ; i++)
            {
                if (!Regex.IsMatch(gCodeArray[i], "(^G0?[0-3]([XYZIJKRF][0-9]+.?[0-9]*)+\r?)|\r$"))
                {
                    return null;
                }
            }
            for (int i = 0; i < traverseLength; i++)
            {
                var newInfoItem = new MoveInfoStruct();
                newInfoItem.Gcode = gCodeArray[i];
                if (gCodeArray[i].Contains('G'))
                {
                    var remainText = Regex.Split(gCodeArray[i], "G")[1];
                    var moveInfoDataItem = Regex.Split(remainText, "[X-Z]")[0];
                    switch (int.Parse(moveInfoDataItem))
                    {
                        case 0:
                        case 1:
                            newInfoItem.Type = 1;
                            break;
                        case 2:
                            newInfoItem.Type = 2;
                            break;
                        case 3:
                            newInfoItem.Type = 3;
                            break;
                        default:
                            break;
                    }
                }
                if (gCodeArray[i].Contains('X'))
                {
                    var remainText = Regex.Split(gCodeArray[i], "X")[1];
                    var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F")[0];
                    newInfoItem.Position[0] = double.Parse(moveInfoDataItem);
                }

                if (gCodeArray[i].Contains('Y'))
                {
                    var remainText = Regex.Split(gCodeArray[i], "Y")[1];
                    var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F")[0];
                    newInfoItem.Position[1] = double.Parse(moveInfoDataItem);
                }

                if (gCodeArray[i].Contains('Z'))
                {
                    var remainText = Regex.Split(gCodeArray[i], "Z")[1];
                    var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F")[0];
                    newInfoItem.Position[2] = double.Parse(moveInfoDataItem);
                }

                if (gCodeArray[i].Contains('F'))
                {
                    var remainText = Regex.Split(gCodeArray[i], "F")[1];
                    var moveInfoDataItem = Regex.Split(remainText, "[X-Z]|[I-K]|F")[0];
                    newInfoItem.Speed = double.Parse(moveInfoDataItem);
                }
                moveInfoList.Add(newInfoItem);
            }
            
            return moveInfoList;
        }
    }
}
