using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Mocks
{
    public abstract class MockRepository<TEntity, TEntityId> where TEntity : class
    {
        // Fields
        private readonly List<TEntity> _entityList = new List<TEntity>();

        private readonly Func<TEntity, TEntityId> _getEntityIdDelegate = null;       


        // Constructors
        public MockRepository(Func<TEntity, TEntityId> getEntityIdDelegate)
        {
            #region Contracts

            if (getEntityIdDelegate == null) throw new ArgumentException(nameof(getEntityIdDelegate));

            #endregion

            // Default
            _getEntityIdDelegate = getEntityIdDelegate;
        }


        // Properties
        protected List<TEntity> EntityList { get { return _entityList; } }


        // Methods
        public void Add(TEntity entity)
        {
            #region Contracts

            if (entity == null) throw new ArgumentException();

            #endregion

            // Find
            var storeEntity = this.FindById(_getEntityIdDelegate(entity));
            if (storeEntity != null) throw new DuplicateKeyException($"Id is exist.: id={_getEntityIdDelegate(entity)}");

            // Add
            _entityList.Add(entity);
        }

        public void Update(TEntity entity)
        {
            #region Contracts

            if (entity == null) throw new ArgumentException();

            #endregion

            // Find
            var storeEntity = this.FindById(_getEntityIdDelegate(entity));
            if (storeEntity == null) throw new InvalidOperationException($"Id not exist.: id={_getEntityIdDelegate(entity)}");

            // Remove
            _entityList.Remove(storeEntity);

            // Add
            _entityList.Add(entity);
        }

        public void Remove(TEntityId entityId)
        {
            #region Contracts

            if (entityId == null) throw new ArgumentException();

            #endregion

            // Find
            var storeEntity = this.FindById(entityId);
            if (storeEntity == null) return;

            // Remove
            _entityList.Remove(storeEntity);
        }


        public TEntity FindById(TEntityId entityId)
        {
            #region Contracts

            if (entityId == null) throw new ArgumentException();

            #endregion

            // Find
            var storeEntity = _entityList.Find((entity) => EqualityComparer<TEntityId>.Default.Equals(entityId, _getEntityIdDelegate(entity)));
            if (storeEntity == null) return null;

            // Return
            return storeEntity;
        }

        public List<TEntity> FindAll()
        {
            // FindAll
            return _entityList.ToList();
        }
    }
}
