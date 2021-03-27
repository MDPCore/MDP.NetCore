using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Mocks
{
    public abstract class MockRepository<TEntity, TEntityId> : MockRepositoryBase<TEntity, Tuple<TEntityId>> where TEntity : class
    {
        // Constructors
        public MockRepository(Func<TEntity, Tuple<TEntityId>> getEntityIdDelegate) : base(getEntityIdDelegate) { }


        // Methods
        public void Remove(TEntityId entityId)
        {
            #region Contracts

            if (entityId == null) throw new ArgumentException();

            #endregion

            // Remove
            base.Remove(Tuple.Create(entityId));
        }

        public TEntity FindById(TEntityId entityId)
        {
            #region Contracts

            if (entityId == null) throw new ArgumentException();

            #endregion

            // FindById
            return base.FindById(Tuple.Create(entityId));
        }
    }

    public abstract class MockRepository<TEntity, TEntityId1, TEntityId2> : MockRepositoryBase<TEntity, Tuple<TEntityId1, TEntityId2>> where TEntity : class
    {
        // Constructors
        public MockRepository(Func<TEntity, Tuple<TEntityId1, TEntityId2>> getEntityIdDelegate) : base(getEntityIdDelegate) { }


        // Methods
        public void Remove(TEntityId1 entityId1, TEntityId2 entityId2)
        {
            #region Contracts

            if (entityId1 == null) throw new ArgumentException(nameof(entityId1));
            if (entityId2 == null) throw new ArgumentException(nameof(entityId2));

            #endregion

            // Remove
            base.Remove(Tuple.Create(entityId1, entityId2));
        }

        public TEntity FindById(TEntityId1 entityId1, TEntityId2 entityId2)
        {
            #region Contracts

            if (entityId1 == null) throw new ArgumentException(nameof(entityId1));
            if (entityId2 == null) throw new ArgumentException(nameof(entityId2));

            #endregion

            // FindById
            return base.FindById(Tuple.Create(entityId1, entityId2));
        }
    }

    public abstract class MockRepository<TEntity, TEntityId1, TEntityId2, TEntityId3> : MockRepositoryBase<TEntity, Tuple<TEntityId1, TEntityId2, TEntityId3>> where TEntity : class
    {
        // Constructors
        public MockRepository(Func<TEntity, Tuple<TEntityId1, TEntityId2, TEntityId3>> getEntityIdDelegate) : base(getEntityIdDelegate) { }


        // Methods
        public void Remove(TEntityId1 entityId1, TEntityId2 entityId2, TEntityId3 entityId3)
        {
            #region Contracts

            if (entityId1 == null) throw new ArgumentException(nameof(entityId1));
            if (entityId2 == null) throw new ArgumentException(nameof(entityId2));
            if (entityId3 == null) throw new ArgumentException(nameof(entityId3));

            #endregion

            // Remove
            base.Remove(Tuple.Create(entityId1, entityId2, entityId3));
        }

        public TEntity FindById(TEntityId1 entityId1, TEntityId2 entityId2, TEntityId3 entityId3)
        {
            #region Contracts

            if (entityId1 == null) throw new ArgumentException(nameof(entityId1));
            if (entityId2 == null) throw new ArgumentException(nameof(entityId2));
            if (entityId3 == null) throw new ArgumentException(nameof(entityId3));

            #endregion

            // FindById
            return base.FindById(Tuple.Create(entityId1, entityId2, entityId3));
        }
    }
}
