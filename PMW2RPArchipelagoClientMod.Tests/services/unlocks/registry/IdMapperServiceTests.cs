using MelonLoader;
using Moq;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.item;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.location;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;
using PMW2RPArchipelagoClientMod.services.util;

namespace PMW2RPArchipelagoClientMod.Tests.services.unlocks.registry
{
    public class IdMapperServiceTests
    {
        private Mock<MelonMod> _mockMelonMod;
        private Mock<IReflectionService> _mockReflectionService;
        private Mock<IAPConnectionService> _mockConnectionService;

        private void _setup()
        {
            _mockMelonMod = new Mock<MelonMod>();
            _mockConnectionService = new Mock<IAPConnectionService>();
            _mockReflectionService = new Mock<IReflectionService>();
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Invalid_NoValidConstructor
        {
            public Invalid_NoValidConstructor(bool _)
            {

            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Invalid_ValidConstructor
        {
            public Invalid_ValidConstructor()
            {

            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Invalid_ValidConstructor2
        {
            public Invalid_ValidConstructor2(long l)
            {

            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Invalid_ValidConstructor3
        {
            public Invalid_ValidConstructor3(long l, IAPConnectionService aPConnection)
            {

            }
        }

        private class Invalid_NoAttribute : IUnlockableItem
        {
            public Invalid_NoAttribute(long l, IAPConnectionService aPConnection)
            {

            }

            public long Id => throw new NotImplementedException();

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData(typeof(Invalid_NoValidConstructor))]
        [InlineData(typeof(Invalid_ValidConstructor))]
        [InlineData(typeof(Invalid_ValidConstructor2))]
        [InlineData(typeof(Invalid_ValidConstructor3))]
        [InlineData(typeof(Invalid_NoAttribute))]
        public void NoRegistrationAttemptsIfNoValidClasses(Type type)
        {
            _setup();
            _mockReflectionService.Setup(x => x.GetLoadedTypes())
                .Returns([type]);

            var idMapperService = new IdMapperService(_mockMelonMod.Object,
                _mockConnectionService.Object,
                _mockReflectionService.Object);
            idMapperService.Init();
            Assert.False(idMapperService.TryGetItemFromId(5, out var item));
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_NoValidConstructor : IUnlockableItem
        {
            public Valid_NoValidConstructor(bool _)
            {

            }

            public long Id => throw new NotImplementedException();

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void NoValidConstructorThrowsError()
        {
            _setup();
            _mockReflectionService.Setup(x => x.GetLoadedTypes())
                .Returns([typeof(Valid_NoValidConstructor)]);

            var idMapperService = new IdMapperService(_mockMelonMod.Object,
                _mockConnectionService.Object,
                _mockReflectionService.Object);
            Assert.Throws<InvalidDataException>(() => idMapperService.Init());
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_DefaultConstructor : IUnlockableItem
        {
            public Valid_DefaultConstructor()
            {

            }

            public long Id => 60;

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_ServiceConstructor : IUnlockableItem
        {
            public Valid_ServiceConstructor(IAPConnectionService connectionService)
            {

            }

            public long Id => 60;

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_LongConstructor : IUnlockableItem
        {
            private long _id;

            public Valid_LongConstructor(long l)
            {
                _id = l;
            }

            public long Id => _id;

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_LargerConstructor : IUnlockableItem
        {
            private long _id;

            public Valid_LargerConstructor(long l, IAPConnectionService connectionService)
            {
                _id = l;
            }

            public long Id => _id;

            public void Unlock(IItemsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_DefaultConstructorLoc : IUnlockableLocation
        {
            public Valid_DefaultConstructorLoc()
            {

            }

            public long Id => 60;

            public void Clear(ILocationsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_ServiceConstructorLoc : IUnlockableLocation
        {
            public Valid_ServiceConstructorLoc(IAPConnectionService connectionService)
            {

            }

            public long Id => 60;

            public void Clear(ILocationsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_LongConstructorLoc : IUnlockableLocation
        {
            private long _id;

            public Valid_LongConstructorLoc(long l)
            {
                _id = l;
            }

            public long Id => _id;

            public void Clear(ILocationsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [MapFromIdRange(
            inclusiveMin: 0,
            exclusiveMax: 10)]
        private class Valid_LargerConstructorLoc : IUnlockableLocation
        {
            private long _id;

            public Valid_LargerConstructorLoc(long l, IAPConnectionService connectionService)
            {
                _id = l;
            }

            public long Id => _id;

            public void Clear(ILocationsDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData(typeof(Valid_DefaultConstructor), 5, 60, typeof(IUnlockableItem))]
        [InlineData(typeof(Valid_ServiceConstructor), 5, 60, typeof(IUnlockableItem))]
        [InlineData(typeof(Valid_LongConstructor), 5, 5, typeof(IUnlockableItem))]
        [InlineData(typeof(Valid_LargerConstructor), 5, 5, typeof(IUnlockableItem))]
        [InlineData(typeof(Valid_DefaultConstructorLoc), 5, 60, typeof(IUnlockableLocation))]
        [InlineData(typeof(Valid_ServiceConstructorLoc), 5, 60, typeof(IUnlockableLocation))]
        [InlineData(typeof(Valid_LongConstructorLoc), 5, 5, typeof(IUnlockableLocation))]
        [InlineData(typeof(Valid_LargerConstructorLoc), 5, 5, typeof(IUnlockableLocation))]
        public void ValidClassesRegisteredProperlyAsItems(Type type, long inId, long outId, Type dataType)
        {
            _setup();
            _mockReflectionService.Setup(x => x.GetLoadedTypes())
                .Returns([type]);

            var idMapperService = new IdMapperService(_mockMelonMod.Object,
                _mockConnectionService.Object,
                _mockReflectionService.Object);
            idMapperService.Init();
            if (dataType == typeof(IUnlockableItem))
            {
                Assert.True(idMapperService.TryGetItemFromId(inId, out var item));
                Assert.Equal(outId, item.Id);
            }
            else if (dataType == typeof(IUnlockableLocation))
            {
                Assert.True(idMapperService.TryGetLocationFromId(inId, out var location));
                Assert.Equal(outId, location.Id);
            }
        }
    }
}
