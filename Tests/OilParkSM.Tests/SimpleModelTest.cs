using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OilParkSM.Tests
{
    [TestClass]
    public class SimpleModelTest
    {
        [TestMethod]
        public void SimlpeStruct()
        {
            /*
             *  = - Pipe;  O - Tank;
             *  
             *   = -- O - = --\
             *   = --/          -- = -- O
             *   = -- O - = --/ 
             */
            //arrange
            var firstSourseQuality = new Dictionary<string, float>() { { "ОЧ", 95 }, { "Плотность", 1 } } ;
            var secondSourseQuality = new Dictionary<string, float>() { { "ОЧ", 92 }, { "Плотность", 0.7f } }; ;
            var thirdSourceQuality = new Dictionary<string, float>() { { "ОЧ", 93 }, { "Плотность", 0.86f } }; ;


            var linksMap = new (FOParam ItemParams, int[] NextObjectsIdx, float[] FlowValues, Dictionary<string , float> Quality)[9]
            {
                 (new FOParam(FOType.Pipe), new [] { 3 }, new [] { 10f, 10f, 0f }, firstSourseQuality),
                 (new FOParam(FOType.Pipe), new [] { 3 }, new [] { 10f, 10f, 0f }, secondSourseQuality),
                 (new FOParam(FOType.Pipe), new [] { 4 }, new [] { 0f, 0f, 10f }, thirdSourceQuality),
                 (new FOParam(FOType.Tank), new [] { 5 }, null, null),
                 (new FOParam(FOType.Tank), new [] { 6 }, null, null),
                 (new FOParam(FOType.Pipe), new [] { 7 }, null, null),
                 (new FOParam(FOType.Pipe), new [] { 7 }, null, null),
                 (new FOParam(FOType.Pipe), new [] { 8 }, null, null),
                 (new FOParam(FOType.Tank), new int[0] ,  null, null),
            };

            OilParkModel model = new OilParkModel(linksMap, 3);

            //act
            model.StartModel(new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f});
            float result = model.GetValue();

            //assert
            Assert.AreEqual(50, result);
        }
    }
}
