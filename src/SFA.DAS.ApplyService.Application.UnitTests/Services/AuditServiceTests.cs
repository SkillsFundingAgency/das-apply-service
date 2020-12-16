using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.UnitTests.Services
{
    [TestFixture]
    public class AuditServiceTests
    {
        private AuditServiceTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new AuditServiceTestsFixture();
        }

        [Test]
        public void TrackInsert_InitialState_IsNull()
        {
            _fixture.TrackInsert();
            _fixture.VerifyInsertedItemInitialStateIsNull();
        }

        [Test]
        public void TrackUpdate_InitialState_Is_Captured()
        {
            _fixture.TrackUpdate();
            _fixture.VerifyUpdateInitialState();
        }

        [Test]
        public void TrackDelete_InitialState_Is_Captured()
        {
            _fixture.TrackDelete();
            _fixture.VerifyDeletedInitialState();
        }

        [Test]
        public void Complete_TrackInsert_Saves()
        {
            _fixture
                .TrackInsert()
                .Complete();

            _fixture.VerifyInsertSaved();
        }


        [Test]
        public void Complete_TrackUpdate_Saves()
        {
            _fixture
                .TrackUpdate()
                .Complete();

            _fixture.VerifyUpdateSaved();
        }

        [Test]
        public void Complete_TrackDelete_Saves()
        {
            _fixture
                .TrackDelete()
                .Complete();

            _fixture.VerifyDeleteSaved();
        }

        private class AuditServiceTestsFixture
        {
            public AuditService AuditService { get; set; }
            public Mock<IApplyRepository> Repository { get; }
            public TestAuditEntity TestInsertAuditEntity { get; set; }
            public TestAuditEntity TestUpdateAuditEntity { get; set; }
            public TestAuditEntity TestDeleteAuditEntity { get; set; }
            public Dictionary<string, object> TestUpdateInitialState { get; set; }
            public Dictionary<string, object> TestDeleteInitialState { get; set; }
            public Dictionary<string, object> TestInsertUpdatedState { get; set; }
            public Dictionary<string, object> TestUpdateUpdatedState { get; set; }
            public Mock<IStateService> StateService { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public UserAction UserAction { get; set; }

            public AuditServiceTestsFixture()
            {
                var autoFixture = new Fixture();

                UserId = autoFixture.Create<string>();
                UserName = autoFixture.Create<string>();
                UserAction = autoFixture.Create<UserAction>();

                TestUpdateAuditEntity = new TestAuditEntity(autoFixture.Create<Guid>());
                TestInsertAuditEntity = new TestAuditEntity(autoFixture.Create<Guid>());
                TestDeleteAuditEntity = new TestAuditEntity(autoFixture.Create<Guid>());

                TestUpdateInitialState = autoFixture.Create<Dictionary<string, object>>();
                TestDeleteInitialState = autoFixture.Create<Dictionary<string, object>>();
                TestInsertUpdatedState = autoFixture.Create<Dictionary<string, object>>();
                TestUpdateUpdatedState = autoFixture.Create<Dictionary<string, object>>();

                StateService = new Mock<IStateService>();
                StateService.Setup(x => x.GetState(It.Is<object>(o => o == TestUpdateAuditEntity))).Returns(TestUpdateInitialState);
                StateService.Setup(x => x.GetState(It.Is<object>(o => o == TestDeleteAuditEntity))).Returns(TestDeleteInitialState);

                Repository = new Mock<IApplyRepository>();
                Repository.Setup(x => x.InsertAudit(It.IsAny<Audit>())).Returns(Task.CompletedTask);

                AuditService = new AuditService(StateService.Object, Mock.Of<IDiffService>(), Repository.Object);
                AuditService.StartTracking(UserAction, UserId, UserName);
            }

            public AuditServiceTestsFixture TrackInsert()
            {
                AuditService.AuditInsert(TestInsertAuditEntity);
                return this;
            }

            public AuditServiceTestsFixture TrackUpdate()
            {
                AuditService.AuditUpdate(TestUpdateAuditEntity);
                return this;
            }

            public AuditServiceTestsFixture TrackDelete()
            {
                AuditService.AuditDelete(TestDeleteAuditEntity);
                return this;
            }

            public AuditServiceTestsFixture Complete()
            {
                StateService.Setup(x => x.GetState(It.Is<object>(o => o == TestUpdateAuditEntity))).Returns(TestUpdateUpdatedState);
                StateService.Setup(x => x.GetState(It.Is<object>(o => o == TestInsertAuditEntity))).Returns(TestInsertUpdatedState);

                AuditService.Save().GetAwaiter().GetResult();

                return this;
            }

            public void VerifyInsertedItemInitialStateIsNull()
            {
                var trackedItem = AuditService.TrackedItems.Single(x => x.Operation == AuditOperation.Insert);
                Assert.IsTrue(trackedItem.InitialState == null);
            }

            public void VerifyUpdateInitialState()
            {
                var trackedItem = AuditService.TrackedItems.Single(x => x.Operation == AuditOperation.Update);
                Assert.AreSame(TestUpdateInitialState, trackedItem.InitialState);
            }

            public void VerifyDeletedInitialState()
            {
                var trackedItem = AuditService.TrackedItems.Single(x => x.Operation == AuditOperation.Delete);
                Assert.AreSame(TestDeleteInitialState, trackedItem.InitialState);
            }

            public void VerifyInsertSaved()
            {
                Repository.Verify(
                    x => x.InsertAudit(It.Is<Audit>(
                            a => a.UserId == UserId 
                                   && a.UserName == UserName
                                   && a.UserAction == UserAction.ToString()
                                   && a.EntityId == TestInsertAuditEntity.Id
                                   && a.EntityType == TestInsertAuditEntity.GetType().Name
                                   && a.InitialState == null
                                   && a.UpdatedState == JsonConvert.SerializeObject(TestInsertUpdatedState)
                                   )));
            }

            public void VerifyUpdateSaved()
            {
                Repository.Verify(
                    x => x.InsertAudit(It.Is<Audit>(
                        a => a.UserId == UserId
                             && a.UserName == UserName
                             && a.UserAction == UserAction.ToString()
                             && a.EntityId == TestUpdateAuditEntity.Id
                             && a.EntityType == TestUpdateAuditEntity.GetType().Name
                             && a.InitialState == JsonConvert.SerializeObject(TestUpdateInitialState)
                             && a.UpdatedState == JsonConvert.SerializeObject(TestUpdateUpdatedState)
                    )));
            }

            public void VerifyDeleteSaved()
            {
                Repository.Verify(
                    x => x.InsertAudit(It.Is<Audit>(
                        a => a.UserId == UserId
                             && a.UserName == UserName
                             && a.UserAction == UserAction.ToString()
                             && a.EntityId == TestDeleteAuditEntity.Id
                             && a.EntityType == TestDeleteAuditEntity.GetType().Name
                             && a.InitialState == JsonConvert.SerializeObject(TestDeleteInitialState)
                             && a.UpdatedState == null
                    )));
            }
        }

        private class TestAuditEntity : IAuditable
        {
            public TestAuditEntity(Guid id)
            {
                Id = id;
            }
            public Guid Id { get; }
        }

    }
}
