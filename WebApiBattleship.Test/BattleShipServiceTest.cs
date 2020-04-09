using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Models.RequestModels;
using WebApiBattleShip.Services;
using WebApiBattleShip.Services.Interfaces;

namespace WebApiBattleship.Test
{
    [TestFixture]
    public class BattleShipServiceTest
    {
        private ILogger<BattleshipService> _loggerBattleShipService;
        private ILogger<ServiceHelper> _loggerServiceHelper;

        private static BattleshipBoardGame _battleshipBoardGame;
        private static Response _response;
        private BattleshipService _battleshipService;
        private ServiceHelper _serviceHelper;

        [SetUp]
        public void Setup()
        {
            _response = new Response();
            var mockLoggerBattleShip = new Mock<ILogger<BattleshipService>>();
            _loggerBattleShipService = mockLoggerBattleShip.Object;

            var mockLoggerServiceHelper = new Mock<ILogger<ServiceHelper>>();
            _loggerServiceHelper = mockLoggerServiceHelper.Object;
            _battleshipBoardGame = new BattleshipBoardGame();

            _serviceHelper = new ServiceHelper(_loggerServiceHelper, _battleshipBoardGame, _response);

            var mockIServiceHelper = new Mock<IServiceHelper>();
            mockIServiceHelper.Setup(x => x.AddShipHorizontally(It.IsAny<ShipAddRequest>(), It.IsAny<Board>())).Returns(Task.FromResult(It.IsAny<bool>()));

            mockIServiceHelper.Setup(x => x.AddShipVertically(It.IsAny<ShipAddRequest>(), It.IsAny<Board>())).Returns(Task.FromResult(It.IsAny<bool>()));

            mockIServiceHelper.Setup(x => x.IsShipPlacementHorizontallyPossible(It.IsAny<ShipAddRequest>(), It.IsAny<List<List<Cell>>>())).Returns(Task.FromResult(It.IsAny<bool>()));

            mockIServiceHelper.Setup(x => x.IsShipPlacementVerticallyPossible(It.IsAny<ShipAddRequest>(), It.IsAny<List<List<Cell>>>())).Returns(Task.FromResult(It.IsAny<bool>()));


            _battleshipService = new BattleshipService(_loggerBattleShipService, _battleshipBoardGame, _serviceHelper, _response);

        }

        [Test]
        public async Task CreateBoard_ShouldReturn_10by10Board()
        {
            //Act.
            var result = await _battleshipService.CreateBoard();


            //Assert.
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.That(_battleshipBoardGame.Board.BoardGrid.Grid.Count == 10);
            Assert.That(_battleshipBoardGame.Board.BoardGrid.Grid[0].Count == 10);
            Assert.That(_battleshipBoardGame.Board.BoardGrid.Grid[9].Count == 10);
        }



        [TestCase("1", "2", "vertical", "4")]
        [TestCase("0", "0", "vertical", "10")]
        [TestCase("0", "0", "horizontal", "10")]
        [TestCase("1", "2", "vertical", "2")]
        [TestCase("1", "2", "vertical", "1")]
        [TestCase("1", "2", "vertical", "4")]
        [TestCase("1", "2", "vertical", "4")]
        [TestCase("9", "9", "vertical", "1")]
        public async Task AddShip_ShouldReturn_TrueForAllValidRequest(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();

            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = orientation,
                Length = length
            };

            //Act
            var result = await _battleshipService.AddShip(request);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }






        [TestCase(null, null, null, null)]
        [TestCase("0", "0", null, "10")]
        [TestCase("0", "0", null, null)]
        [TestCase("0", "0", "horizontal", null)]
        [TestCase("", "", "", "")]
        [TestCase("", "", null, "10")]
        [TestCase("0", "0", "", "")]
        [TestCase("0", "", "", "")]
        [TestCase("-1", "-2", "vertical", "-2")]
        [TestCase("-1", "-2", "vertical", "1")]
        [TestCase("1", "2", "vertical", "-4")]
        [TestCase("11", "10", "vertical", "11")]
        [TestCase("10", "10", "horizontal", "10")]
        [TestCase("asdasd", "asdasd", "vertical", "asdasd")]
        [TestCase("-1", "sddasd", "vertical", "asdasd")]
        [TestCase("asdasd", "2", "vertical", "cadasd")]
        [TestCase("asdasd", "2", "aaSswqwda", "cadasd")]
        [TestCase("!@#!@!@$", "#@$#@", "@#$#@$", "@#$#%^")]
        public async Task AddShip_ShouldReturn_FalseForAllInvalidRequest(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();

            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = orientation,
                Length = length
            };

            //Act
            var result = await _battleshipService.AddShip(request);
            Assert.IsFalse(result);

        }


        [TestCase("1", "2", "vertical", "2")]
        [TestCase("3", "4", "vertical", "5")]
        public async Task AttackShip_ShouldReturn_SuccessfulAttack(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships.
            var resultAddShip = await _battleshipService.AddShip(request);


            var attackRequest = new AttackRequest
            {
                HorizontalPosition = hpos,
                VerticalPosition = vpos,

            };

            //Act
            var result = await _battleshipService.AttackShip(attackRequest);

            var remainingLength = _battleshipBoardGame.Ships[0].ShipPosition.Where(x => x.Hit == false).Count();
            Assert.IsTrue(result);

            Assert.That(remainingLength == (Convert.ToInt32(length) - 1));

        }



        [TestCase("1", "2", "vertical", "2")]
        [TestCase("3", "4", "horizontal", "5")]
        public async Task AttackShip_ShouldReturn_MissedAttack(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships
            var resultAddShip = await _battleshipService.AddShip(request);
            AttackRequest attackRequest;
            if (orientation == "vertical")
            {
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos + 1,
                    VerticalPosition = vpos,

                };
            }
            else
            {
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = vpos + 1,

                };

            }

            //Act
            var result = await _battleshipService.AttackShip(attackRequest);

            var remainingLength = _battleshipBoardGame.Ships[0].ShipPosition.Where(x => x.Hit == false).Count();

            //Assert
            Assert.IsFalse(result);
            Assert.That(remainingLength == (Convert.ToInt32(length)));

        }



        [TestCase("1", "2", "vertical", "2")]
        public async Task AttackShip_ShouldReturn_SinkShip(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships
            var resultAddShip = await _battleshipService.AddShip(request);


            AttackRequest attackRequest = new AttackRequest();
            bool attackResult = false;
            if (orientation == "vertical")
            {
                //Attack Ship : Attempt#1
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = vpos,

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);


                //Attack Ship : Attempt#2
                var newVPos = Convert.ToInt32(vpos);
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = (++newVPos).ToString(),

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);
            }

            //Assert
            Assert.IsTrue(attackResult);
            Assert.That(_response.Message == ResponseMessages.ATTACK_SUNK_SHIP);

        }


        [TestCase("1", "2", "vertical", "2")]
        public async Task AttackShip_AfterSinkShip_ShouldReturn_MissedShip(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships
            var resultAddShip = await _battleshipService.AddShip(request);


            AttackRequest attackRequest = new AttackRequest();
            bool attackResult = false;
            if (orientation == "vertical")
            {
                //Attack Ship : Attempt#1
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = vpos,

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);


                //Attack Ship : Attempt#2
                var newVPos = Convert.ToInt32(vpos);
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = (++newVPos).ToString(),

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);
            }

            //Assert
            Assert.IsTrue(attackResult);
            Assert.That(_response.Message == ResponseMessages.ATTACK_SUNK_SHIP);



            attackRequest = new AttackRequest
            {
                HorizontalPosition = hpos,
                VerticalPosition = vpos,

            };

            //Act
            attackResult = await _battleshipService.AttackShip(attackRequest);

            Assert.IsFalse(attackResult);
            Assert.That(_response.Message == ResponseMessages.ATTACK_MISS);
        }





        [TestCase("1", "2", "vertical", "2")]
        public async Task AddShip_AfterSinkShip_ShouldReturn_ShipAdded(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships
            var resultAddShip = await _battleshipService.AddShip(request);


            AttackRequest attackRequest = new AttackRequest();
            bool attackResult = false;
            if (orientation == "vertical")
            {
                //Attack Ship : Attempt#1
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = vpos,

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);


                //Attack Ship : Attempt#2
                var newVPos = Convert.ToInt32(vpos);
                attackRequest = new AttackRequest
                {
                    HorizontalPosition = hpos,
                    VerticalPosition = (++newVPos).ToString(),

                };

                //Act
                attackResult = await _battleshipService.AttackShip(attackRequest);
            }

            //Assert
            Assert.IsTrue(attackResult);
            Assert.That(_response.Message == ResponseMessages.ATTACK_SUNK_SHIP);


            //Act
            attackResult = await _battleshipService.AttackShip(attackRequest);

            //Add ship
            request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };
            resultAddShip = await _battleshipService.AddShip(request);



            //Assert if the ship is added.
            Assert.IsTrue(resultAddShip);
            Assert.That(_response.Message == ResponseMessages.SHIP_ADDED);
        }



        [TestCase("1", "2", "vertical", "2")]
        [TestCase("3", "4", "horizontal", "5")]

        public async Task AddShip_OnSuccessfulAttack_ShouldReturn_NotAdded(string hpos, string vpos, string orientation, string length)
        {
            //Arrange
            await _battleshipService.CreateBoard();


            var request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships.
            var resultAddShip = await _battleshipService.AddShip(request);


            var attackRequest = new AttackRequest
            {
                HorizontalPosition = hpos,
                VerticalPosition = vpos,

            };

            //Act
            var result = await _battleshipService.AttackShip(attackRequest);

            var remainingLength = _battleshipBoardGame.Ships[0].ShipPosition.Where(x => x.Hit == false).Count();

            //Assert if the attack was successful
            Assert.IsTrue(result);
            Assert.That(remainingLength == (Convert.ToInt32(length) - 1));


            //Add ship

            request = new ShipAddRequest
            {
                HorizontalStartingPoint = hpos,
                VerticalStartingPoint = vpos,
                Orientation = "vertical",
                Length = length
            };

            //Add Ships.
            resultAddShip = await _battleshipService.AddShip(request);
            Assert.IsFalse(resultAddShip);
            Assert.That(_response.Message == ResponseMessages.SHIP_OVERLAPPING);

        }

    }
}