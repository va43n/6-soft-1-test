using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using _6_soft_1_test;
using System.Collections.Generic;

namespace _6_soft_1_unit_tests
{
	[TestClass]
	public class Controller_Test
	{
        [TestMethod]
        public void EditSomethingTest()
        {
            Controller controller = new Controller(2, 16);

            Assert.AreEqual("1", controller.ButtonClicked(1));
            Assert.AreEqual("11", controller.ButtonClicked(1));
            Assert.AreEqual("111", controller.ButtonClicked(1));
            Assert.AreEqual("1111", controller.ButtonClicked(1));

            Assert.AreEqual("11111", controller.ButtonClicked(1));
            Assert.AreEqual("111110", controller.ButtonClicked(0));
            Assert.AreEqual("1111100", controller.ButtonClicked(0));
            Assert.AreEqual("11111000", controller.ButtonClicked(0));

            Assert.AreEqual("11111000.", controller.ButtonClicked(16));

            Assert.AreEqual("11111000.0", controller.ButtonClicked(0));
            Assert.AreEqual("11111000.00", controller.ButtonClicked(0));
            Assert.AreEqual("11111000.001", controller.ButtonClicked(1));
            Assert.AreEqual("11111000.0011", controller.ButtonClicked(1));
        }

        [TestMethod]
		public void CheckNormalInput19Test()
		{
			Controller controller = new Controller(2, 16);

			controller.ButtonClicked(1);
            controller.ButtonClicked(1);
            controller.ButtonClicked(1);
            controller.ButtonClicked(1);

            controller.ButtonClicked(1);
            controller.ButtonClicked(0);
            controller.ButtonClicked(0);
            controller.ButtonClicked(0);

            controller.ButtonClicked(16);

            controller.ButtonClicked(0);
            controller.ButtonClicked(0);
            controller.ButtonClicked(1);
            controller.ButtonClicked(1);

			Assert.AreEqual("F8.3", controller.ButtonClicked(19));
        }

        [TestMethod]
        public void CheckBadInput19Test()
        {
            Controller controller = new Controller(16, 2);

            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);

            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);

            controller.ButtonClicked(16);

            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);
            controller.ButtonClicked(15);

            try
            {
                controller.ButtonClicked(19);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Too big number", ex.Message);
            }
        }
    }

	[TestClass]
	public class History_Test
	{
        [TestMethod]
        public void AddRecordTest()
		{
			History history = new History();

			history.AddRecord(2, 10, "101", "5");
            history.AddRecord(10, 16, "15", "F");
            history.AddRecord(3, 10, "2", "2");

            Assert.AreEqual("101 (2) = 5 (10)", history.records[0].ToString());
            Assert.AreEqual("15 (10) = F (16)", history.records[1].ToString());
            Assert.AreEqual("2 (3) = 2 (10)", history.records[2].ToString());
        }

        [TestMethod]
		public void ClearAllRecordsTest()
		{
			History history = new History();

            history.AddRecord(2, 10, "101", "5");
            history.AddRecord(10, 16, "15", "F");
            history.AddRecord(3, 10, "2", "2");

			history.Clear();

			Assert.AreEqual(0, history.records.Count);
        }
    }

    [TestClass]
	public class Editor_Test
	{
        [TestMethod]
		public void EnterNumberTest()
		{
            Editor editor = new Editor();

            Assert.AreEqual("0", editor.EditSomething(0));

			Assert.AreEqual("1", editor.EditSomething(1));
            Assert.AreEqual("1F", editor.EditSomething(15));
            Assert.AreEqual("1F4", editor.EditSomething(4));
            Assert.AreEqual("1F4A", editor.EditSomething(10));

            Assert.AreEqual("1F4A0", editor.EditSomething(0));
            Assert.AreEqual("1F4A00", editor.EditSomething(0));
            Assert.AreEqual("1F4A000", editor.EditSomething(0));
        }

        [TestMethod]
        public void EnterDelimeterTest()
		{
			Editor editor1 = new Editor();

            Assert.AreEqual("0.", editor1.EditSomething(16));
			
			editor1.EditSomething(2);
            Assert.AreEqual("0.2", editor1.EditSomething(16));
            Assert.AreEqual("0.2", editor1.EditSomething(16));
            Assert.AreEqual("0.2", editor1.EditSomething(16));

            Editor editor2 = new Editor();

            editor2.EditSomething(15);
            editor2.EditSomething(0);
            editor2.EditSomething(1);
            editor2.EditSomething(7);
            Assert.AreEqual("F017.", editor2.EditSomething(16));
            Assert.AreEqual("F017.", editor2.EditSomething(16));
            Assert.AreEqual("F017.", editor2.EditSomething(16));

            editor2.EditSomething(3);
            Assert.AreEqual("F017.3", editor2.EditSomething(16));
            Assert.AreEqual("F017.3", editor2.EditSomething(16));
            Assert.AreEqual("F017.3", editor2.EditSomething(16));
        }

        [TestMethod]
        public void DeleteRightNumberTest()
        {
            Editor editor = new Editor();

            Assert.AreEqual("0", editor.EditSomething(17));
            Assert.AreEqual("0", editor.EditSomething(17));
            Assert.AreEqual("0", editor.EditSomething(17));

			editor.EditSomething(1);
            editor.EditSomething(2);
            editor.EditSomething(3);
            editor.EditSomething(4);

            Assert.AreEqual("123", editor.EditSomething(17));
            Assert.AreEqual("12", editor.EditSomething(17));
            Assert.AreEqual("1", editor.EditSomething(17));
            Assert.AreEqual("0", editor.EditSomething(17));
            Assert.AreEqual("0", editor.EditSomething(17));
            Assert.AreEqual("0", editor.EditSomething(17));
        }

        [TestMethod]
        public void DeleteNumberTest()
        {
            Editor editor = new Editor();

            Assert.AreEqual("0", editor.EditSomething(18));
            Assert.AreEqual("0", editor.EditSomething(18));
            Assert.AreEqual("0", editor.EditSomething(18));

            editor.EditSomething(1);
            editor.EditSomething(2);
            editor.EditSomething(3);
            editor.EditSomething(4);

            Assert.AreEqual("0", editor.EditSomething(18));
            Assert.AreEqual("0", editor.EditSomething(18));
            Assert.AreEqual("0", editor.EditSomething(18));
        }

        [TestMethod]
        public void DeleteUnnecessarySymbolsTest()
        {
            Editor editor = new Editor();

            editor.EditSomething(1);
            editor.EditSomething(2);
            editor.EditSomething(3);
            editor.EditSomething(4);

			editor.EditSomething(16);

            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());

            editor.EditSomething(16);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);

            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234", editor.DeleteUnnecessarySymbols());

            editor.EditSomething(16);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(1);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);

            Assert.AreEqual("1234.00001", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234.00001", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("1234.00001", editor.DeleteUnnecessarySymbols());

			editor.EditSomething(18);
            editor.EditSomething(1);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);
            editor.EditSomething(0);

            Assert.AreEqual("10000", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("10000", editor.DeleteUnnecessarySymbols());
            Assert.AreEqual("10000", editor.DeleteUnnecessarySymbols());
        }
    }

    [TestClass]
	public class Converter_p1_10_Test
	{
		[TestMethod]
		public void ZeroTest()
		{
			List<string> numbers = new List<string>()
			{
				"0", "0.0", "0.", "0.00", "0.000"
			};
			for (int i = 2; i <= 16; i++)
				for (int j = 0; j < numbers.Count; j++)
					Assert.AreEqual(0, Converter_p1_10.ConvertValue(numbers[j], i));
		}

		[TestMethod]
		public void IntTest()
		{
			string number = "1011011";

			List<int> expected = new List<int>()
			{
				91, 841, 4421, 16381, 48175, 120401, 
				266761, 538741, 1011011, 1787545, 3008461, 
				4857581, 7570711, 11444641, 16846865
			};
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(expected[i - 2], Converter_p1_10.ConvertValue(number, i));
			}
		}

		[TestMethod]
		public void DoubleTest()
		{
			string number = "0.11001";
			List<double> expected = new List<double>()
			{
                0.78125, 0.44855967078, 0.3134765625,
                0.24032, 0.19457304526, 0.16332480514,
                0.14065551757, 0.12347372521, 0.11001,
                0.09917976293, 0.09028179655, 0.08284292997,
                0.07653247158, 0.07111242798, 0.06640720367
            };
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(Math.Round(expected[i - 2], 10), Math.Round(Converter_p1_10.ConvertValue(number, i)), 10);
			}
		}

		[TestMethod]
		public void IntDoubleTest()
		{
			List<string> numbers = new List<string>()
			{
                "1010110101.11011101", "1111.0011", "321123123.111122", 
				"412312.414141", "51234123.12345", "65432123.55555", 
				"7774727.1234567", "1213521.888", "990953223.123", 
				"2343AAAA.A", "ABBBBB.234232", "CBBBBBCC.CCC", 
				"DDDDDD.DDD", "34EDEDE.67899", "43FFFFF.FF233"
            };
			List<double> expected = new List<double>()
			{
                693.86328125, 40.04938271604, 235227.33447266,
                13457.874944, 1466691.2398405, 5604686.8332838,
                2095575.1632648, 658711.99862826, 990953223.123,
                44991792.9090909, 2737151.189924, 810501691.99954,
                7529535.9996356, 37965359.43367, 71303167.99663
            };
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(Math.Round(expected[i - 2], 10), Math.Round(Converter_p1_10.ConvertValue(numbers[i - 2], i)), 10);
			}
		}
	}

	[TestClass]
	public class Converter_10_p2_Test
	{
		[TestMethod]
		public void ZeroTest()
		{
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual("0", Converter_10_p2.ConvertValue(0, i, 0));
			}
		}

		[TestMethod]
		public void IntTest()
		{
			int number = 100983;
			List<string> expected = new List<string>()
			{
				"11000101001110111", "12010112010", "120221313", "11212413",
				"2055303", "600261", "305167", "163463", "100983", "69963",
				"4A533", "36C6C", "28B31", "1EDC3", "18A77"
			};
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(expected[i - 2], Converter_10_p2.ConvertValue(number, i, 0));
			}
		}

		[TestMethod]
		public void DoubleTest()
		{
			double number = 0.1220703125;
			List<string> expected = new List<string>()
			{
				"0.0001111101", "0.0100212222010111", "0.01331", 
				"0.0301121332421432", "0.0422111513", "0.0566043102630124", 
				"0.0764", "0.1078811461404165", "0.1220703125", 
				"0.1385260065542225", "0.156B3", "0.17825B1104A2B197", 
				"0.19CD64B537", "0.1C6EC22476C92B7D", "0.1F4"
			};
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(expected[i - 2], Converter_10_p2.ConvertValue(number, i, 16));
			}
		}

		[TestMethod]
		public void IntDoubleTest()
		{
			double number = 356.1953125;
			List<string> expected = new List<string>()
			{
				"101100100.0011001", "111012.0120211011000012", "11210.0302", 
				"2411.0442013343304022", "1352.1101043", "1016.1236642146033055",
				"544.144", "435.1673400562278451", "356.1953125",
				"2A4.216A6300A4423583", "258.2416", "215.270142195A34754C",
				"1B6.2A3D1A7", "18B.2DE2A66A2DE2A66A", "164.32"
			};
			for (int i = 2; i <= 16; i++)
			{
				Assert.AreEqual(expected[i - 2], Converter_10_p2.ConvertValue(number, i, 16));
			}
		}
	}
}
