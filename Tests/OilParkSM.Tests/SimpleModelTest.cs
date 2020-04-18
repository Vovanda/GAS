using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.Tests
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


            var linksMap = new (List<int> NextObjList, FactoryObjectParam Params, float[] FlowValues, Dictionary<string , float> Quality)[9]
            {
                (new List<int>() { 3 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, new [] { 10f, 10f, 0f }, firstSourseQuality),
                (new List<int>() { 3 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, new [] { 10f, 10f, 0f }, secondSourseQuality),
                (new List<int>() { 4 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, new [] { 0f, 0f, 10f }, thirdSourceQuality),
                (new List<int>() { 5 }, new FactoryObjectParam(){Type = FactoryObjectType.Tank }, null, null),
                (new List<int>() { 6 }, new FactoryObjectParam(){Type = FactoryObjectType.Tank }, null, null),
                (new List<int>() { 7 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, null, null),
                (new List<int>() { 7 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, null, null),
                (new List<int>() { 8 }, new FactoryObjectParam(){Type = FactoryObjectType.Pipe }, null, null),
                (new List<int>(), new FactoryObjectParam(){Type = FactoryObjectType.Tank }, null, null),
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
