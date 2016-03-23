// -----------------------------------------------------------------------
// <copyright file="InMemoryDbSet.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// In-memory implementation of database set
    /// </summary>
    /// <typeparam name="TEntity">The type that defines the set.</typeparam>
    public class InMemoryDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity> where TEntity : class
    {
        /// <summary>
        /// Set where entities stored.
        /// </summary>
        private readonly HashSet<TEntity> set;

        /// <summary>
        /// Queryable sequence of entities.
        /// </summary>
        private readonly IQueryable<TEntity> queryableSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbSet{TEntity}"/> class.
        /// </summary>
        public InMemoryDbSet()
            : this(Enumerable.Empty<TEntity>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbSet{TEntity}"/> class using given sequence.
        /// </summary>
        /// <param name="entities">Sequence of entities which should be stored in the set.</param>
        public InMemoryDbSet(IEnumerable<TEntity> entities)
        {
            this.set = new HashSet<TEntity>();
            foreach (var entity in entities)
            {
                this.set.Add(entity);
            }

            this.queryableSet = this.set.AsQueryable();
        }

        /// <summary>
        /// Gets an <see cref="ObservableCollection{T}"/> that represents
        /// a local view of all Added, Unchanged, and Modified entities in this set.
        /// This local view will stay in sync as entities are added or removed from the
        /// context. Likewise, entities added to or removed from the local view will
        /// automatically be added to or removed from the context.
        /// </summary>
        /// <remarks>
        /// This property can be used for data binding by populating the set with data,
        /// for example by using the Load extension method, and then binding to the local
        /// data through this property. For WPF bind to this property directly. For Windows
        /// Forms bind to the result of calling ToBindingList on this property
        /// </remarks>
        public override ObservableCollection<TEntity> Local
        {
            get { return new ObservableCollection<TEntity>(this.queryableSet); }
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree
        /// associated with this instance of <see cref="IQueryable"/> is executed.
        /// </summary>
        /// <value>
        /// A <see cref="Type"/> that represents the type of the element(s) that are returned
        /// when the expression tree associated with this object is executed.
        /// </value>
        public Type ElementType
        {
            get { return this.queryableSet.ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="IQueryable"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Expression"/> that is associated with this instance
        /// of <see cref="IQueryable"/>.
        /// </value>
        public Expression Expression
        {
            get { return this.queryableSet.Expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <value>The <see cref="IQueryProvider"/> that is associated with this data source.</value>
        public IQueryProvider Provider
        {
            get { return this.queryableSet.Provider; }
        }

        /// <summary>
        /// Adds the given entity to the context underlying the set in the Added state
        /// such that it will be inserted into the database when SaveChanges is called.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The entity.</returns>
        /// <remarks>
        /// Note that entities that are already in the context in some other state will
        /// have their state set to Added. Add is a no-op if the entity is already in
        /// the context in the Added state.
        /// </remarks>
        public override TEntity Add(TEntity entity)
        {
            this.set.Add(entity);
            return entity;
        }

        /// <summary>
        /// Attaches the given entity to the context underlying the set. That is, the
        /// entity is placed into the context in the Unchanged state, just as if it had
        /// been read from the database.
        /// </summary>
        /// <param name="entity">The entity to attach.</param>
        /// <returns>The entity.</returns>
        /// <remarks>
        /// Attach is used to repopulate a context with an entity that is known to already
        /// exist in the database.  SaveChanges will therefore not attempt to insert
        /// an attached entity into the database because it is assumed to already be
        /// there.  Note that entities that are already in the context in some other
        /// state will have their state set to Unchanged. Attach is a no-op if the entity
        /// is already in the context in the Unchanged state.
        /// </remarks>
        public override TEntity Attach(TEntity entity)
        {
            this.set.Add(entity);
            return entity;
        }

        /// <summary>
        /// Marks the given entity as Deleted such that it will be deleted from the database
        /// when SaveChanges is called. Note that the entity must exist in the context
        /// in some other state before this method is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>The entity.</returns>
        /// <remarks>
        /// Note that if the entity exists in the context in the Added state, then this
        /// method will cause it to be detached from the context. This is because an
        /// Added entity is assumed not to exist in the database such that trying to
        /// delete it does not make sense.
        /// </remarks>
        public override TEntity Remove(TEntity entity)
        {
            this.set.Remove(entity);
            return entity;
        }

        /// <summary>
        /// Creates a new instance of an entity for the type of this set.  Note that
        /// this instance is NOT added or attached to the set.  The instance returned
        /// will be a proxy if the underlying context is configured to create proxies
        /// and the entity type meets the requirements for creating a proxy.
        /// </summary>
        /// <returns>The entity instance, which may be a proxy.</returns>
        public override TEntity Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds an entity with the given primary key values.  If an entity with the
        /// given primary key values exists in the context, then it is returned immediately
        /// without making a request to the store. Otherwise, a request is made to the
        /// store for an entity with the given primary key values and this entity, if
        /// found, is attached to the context and returned. If no entity is found in
        /// the context or the store, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        /// <remarks>
        /// The ordering of composite key values is as defined in the EDM, which is in
        /// turn as defined in the designer, by the Code First fluent API, or by the
        /// DataMember attribute.
        /// </remarks>
        public override TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A System.Collections.Generic.IEnumerator{T} that can be used to iterate through
        /// the collection.
        /// </returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.queryableSet.GetEnumerator();
        }
    }
}
