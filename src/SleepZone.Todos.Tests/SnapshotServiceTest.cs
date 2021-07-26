using SleepZone.Todos.Mocks;
using System;
using System.Linq;
using Xunit;

namespace SleepZone.Todos.Tests
{
    public class SnapshotServiceTest
    {
        [Fact]
        public void Snapshot_X個Todo資料包含Y個完成_快照數量XY()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();
            todoRepository.Add(new Todo() { TodoId = "TodoId-001", Name = "Name-001", IsComplete = true });
            todoRepository.Add(new Todo() { TodoId = "TodoId-002", Name = "Name-002", IsComplete = true });
            todoRepository.Add(new Todo() { TodoId = "TodoId-003", Name = "Name-003", IsComplete = false });

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(3, snapshot.TotalCount);
            Assert.Equal(2, snapshot.CompleteCount);
        }

        [Fact]
        public void Snapshot_X個Todo資料包含0個完成_快照數量X0()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();
            todoRepository.Add(new Todo() { TodoId = "TodoId-001", Name = "Name-001", IsComplete = false });
            todoRepository.Add(new Todo() { TodoId = "TodoId-002", Name = "Name-002", IsComplete = false });
            todoRepository.Add(new Todo() { TodoId = "TodoId-003", Name = "Name-003", IsComplete = false });

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(3, snapshot.TotalCount);
            Assert.Equal(0, snapshot.CompleteCount);
        }

        [Fact]
        public void Snapshot_0個Todo資料包含0個完成_快照數量00()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(0, snapshot.TotalCount);
            Assert.Equal(0, snapshot.CompleteCount);
        }
    }
}
