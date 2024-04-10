using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConnectFour.Model;
using ConnectFour.Persistence;
using Moq;

namespace ConnectFourTest
{
    [TestClass]
    public class ConnectFourTest
    {
        private ConnectFourGameModel _model = null!;
        private Player[] _mockedTable = null!;
        private Mock<IPersistence> _mock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new Player[100];
            _mockedTable[90] = Player.PlayerX;
            _mockedTable[98] = Player.PlayerO;
            _mockedTable[99] = Player.PlayerX;

            _mock = new Mock<IPersistence>();
            _mock.Setup(mock => mock.Load(It.IsAny<String>()))
                .Returns(() => (_mockedTable, 0, 0));

            _model = new ConnectFourGameModel(_mock.Object, 10);
        }


        [TestMethod]
        public void TestNewGame()
        {
            _model.NewGame();
            Assert.AreEqual(Player.NoPlayer, _model[0, 0]);
            Assert.AreEqual(Player.NoPlayer, _model[9, 0]);
            Assert.AreEqual(Player.NoPlayer, _model[0, 9]);
            Assert.ThrowsException<ArgumentException>(() => _model[15, 0]);
            Assert.ThrowsException<ArgumentException>(() => _model[0, 12]);
        }

        [TestMethod]
        public void TestStepGame()
        {
            _model.NewGame();
            _model.StepGame(9, 0);
            Assert.AreEqual(Player.PlayerX, _model[9, 0]);
            _model.StepGame(9, 9);
            Assert.AreEqual(Player.PlayerO, _model[9, 9]);
            _model.StepGame(8, 0);
            Assert.AreEqual(Player.PlayerX, _model[8, 0]);
            Assert.AreEqual(3, _model.StepNumber);


            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _model.StepGame(10, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _model.StepGame(0, 11));

            Assert.ThrowsException<InvalidOperationException>(() => _model.StepGame(9, 0));
            Assert.ThrowsException<InvalidOperationException>(() => _model.StepGame(5, 5));
        }

        [TestMethod]
        public void TestAdvanceTime()
        {
            _model.NewGame();

            Int32 time = 0;
            while (_model.GameTimeX != 10)
            {
                _model.AdvanceTime();

                time++;

                Assert.AreEqual(time, _model.GameTimeX);
                Assert.AreEqual(0, _model.StepNumber);
            }

            Assert.AreEqual(10, _model.GameTimeX);
            Assert.AreEqual(0, _model.GameTimeO);
        }


        [TestMethod]
        public void LoadTest()
        {
            _model.NewGame();
            _model.LoadGame(String.Empty);

            for (Int32 i = 0; i < 10; i++)
            {
                for (Int32 j = 0; j < 10; j++)
                {
                    Assert.AreEqual(_mockedTable[i*10+j], _model[i, j]);
                }
            }
            Assert.AreEqual(3, _model.StepNumber);
            Assert.AreEqual(0, _model.GameTimeO);
            Assert.AreEqual(0, _model.GameTimeX);
            _mock.Verify(dataAccess => dataAccess.Load(String.Empty), Times.Once());

        }
    }
}