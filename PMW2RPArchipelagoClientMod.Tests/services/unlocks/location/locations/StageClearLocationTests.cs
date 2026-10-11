using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location.locations;
using Moq;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.location;

namespace PMW2RPArchipelagoClientMod.Tests.services.unlocks.location.locations
{
    public class StageClearLocationTests
    {
        [Theory]
        [InlineData(GoalBossOption.Spooky, EWorldStage.Stage6_4, false)]
        [InlineData(GoalBossOption.TocMan, EWorldStage.Stage6_4, true)]
        [InlineData(GoalBossOption.Spooky, EWorldStage.Stage6_5, true)]
        [InlineData(GoalBossOption.TocMan, EWorldStage.Stage6_5, false)]
        public void OnlyClearGoalStageWhenGoalMatches(GoalBossOption goalBossOption, EWorldStage stage, bool shouldClear)
        {
            var apConnectionMock = new Mock<IAPConnectionService>();
            apConnectionMock.Setup(x => x.GoalBoss).Returns(goalBossOption);

            StageClearLocation location = new StageClearLocation(stage, apConnectionMock.Object);

            var locationDispatchMock = new Mock<ILocationsDispatcher>();
            location.Clear(locationDispatchMock.Object);

            if (shouldClear)
            {
                locationDispatchMock.Verify(x => x.ClearLocation(It.Is<StageClearLocation>(location => location.Location == stage)));
            }
            else
            {
                locationDispatchMock.VerifyNoOtherCalls();
            }
        }
    }
}
