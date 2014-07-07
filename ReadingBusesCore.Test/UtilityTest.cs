using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReadingBusesCore.Test
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void TextToNumber()
        {
            var tests = new []
            {
                new { Input = "0", Expected = 0 },
                new { Input = "1", Expected = 1 },
                new { Input = "A", Expected = 10 },
                new { Input = "a", Expected = 36 },
                new { Input = "z", Expected = 61 },
                new { Input = "10", Expected = 62 },
                new { Input = "zz", Expected = 3843 },
            };

            foreach (var test in tests)
            {
                var actual = Utility.StringToInt(test.Input, Utility.Base62);
                Assert.AreEqual(test.Expected, actual, test.Input);
            }
        }

        [TestMethod]
        public void Service_AsNumber()
        {
            var tests = new []
            {
                new { Input = "1",   Expected = 100 },
                new { Input = "1R",  Expected = 127 },
                new { Input = "1z",  Expected = 161 },
                new { Input = "1zz", Expected = 161 },
                new { Input = "N5",  Expected = 2300500 },
                new { Input = "N9",  Expected = 2300900 },
                new { Input = "N21", Expected = 2302100 },
                new { Input = "N23", Expected = 2302300 },
                new { Input = "N26", Expected = 2302600 },
                new { Input = "zz99z", Expected = 384309961 }, // max
                new { Input = "TST", Expected = int.MaxValue }, // string
                new { Input = "TVP", Expected = int.MaxValue },// string
                new { Input = "LINK", Expected = int.MaxValue },// string
            };

            foreach (var test in tests)
            {
                var actual = ServiceComparer.AsNumber(test.Input);
                Assert.AreEqual(test.Expected, actual, test.Input);
            }
        }

        [TestMethod]
        public void SortServices()
        {
            var services = new[] { "1","100","11","2","20","3" };

            var expected = new[] { "1","2","3","11","20","100" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices2()
        {
            var services = new[] { "20", "200", "20A", "21", "30" };

            var expected = new[] { "20", "20A", "21", "30", "200" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices3()
        {
            var services = new[] { "N21", "N23", "N5", "N26", "N9" };

            var expected = new[] { "N5", "N9", "N21", "N23", "N26" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices4()
        {
            var services = new[] { "ND4", "ND6", "ND75", "ND2", "ND3", "ND6A", "ND8" };

            var expected = new[] { "ND2", "ND3", "ND4", "ND6", "ND6A", "ND8", "ND75" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices5()
        {
            var services = new[] { "N21", "ND2", "ND6A","ND3", "ND4", "ND6", "N23", "N26", "N5", "N9" };

            var expected = new[] { "N5", "N9", "N21", "N23", "N26", "ND2", "ND3", "ND4", "ND6", "ND6A" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices6()
        {
            var services = new[] { "1", "2", "3", "1R" };

            var expected = new[] { "1", "1R", "2", "3" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        [TestMethod]
        public void SortServices7()
        {
            var services = new[] { "1", "TST", "TVP", "LINK" };

            var expected = new[] { "1", "LINK", "TST", "TVP" };

            var actual = services.OrderBy(s => s, new ServiceComparer());

            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        //[TestMethod]
        //public void SortServices3()
        //{
        //    var services = new[] { "1", "100","101","102","103","104","105","107","11","115",
        //        "13","14","142","144","15","155","16","17","171","172","18","19","190","191",
        //        "194","1R","2","20","200","20A","21","22","23","24","26","27","28","3","33",
        //        "40","5","50","500","51","52","53","6","701","702","82","82A","9","919","981",
        //        "983","991","LH1","LH2","LH3","LH4","LINK","N21","N23","N26","N5","N9","ND2",
        //        "ND3","ND4","ND6","ND6A","ND75","ND8","TG1","TG2","TG3","TG4","TG5","TG6",
        //        "TST","TV","TVP","V1","V10","V12","V2","V8","V9","WB5","X25","X9","X94" };

        //    var expected = 

        //    var actual = services.OrderBy(s => s, new ServiceComparer());

        //    Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        //}
    }
}
