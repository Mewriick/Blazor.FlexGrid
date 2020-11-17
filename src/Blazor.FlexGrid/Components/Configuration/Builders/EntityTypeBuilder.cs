﻿using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class EntityTypeBuilder<TEntity> where TEntity : class
    {
        private InternalEntityTypeBuilder Builder { get; }

        public EntityTypeBuilder(InternalEntityTypeBuilder internalEntityTypeBuilder)
        {
            Builder = internalEntityTypeBuilder ?? throw new ArgumentNullException(nameof(internalEntityTypeBuilder));
        }

        public virtual PropertyBuilder<TProperty, TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
            => new PropertyBuilder<TProperty, TEntity>(
                    Builder.Property(propertyExpression.GetPropertyAccess())
                );

        public virtual MasterDetailRelationshipBuilder HasDetailRelationship<TDetailEntity>
            (Expression<Func<TEntity, object>> propertyKeyExpression, Expression<Func<TDetailEntity, object>> propertyDetailExpression)
            => new MasterDetailRelationshipBuilder(
                    Builder.HasDetailRelationship(typeof(TDetailEntity), propertyKeyExpression.GetPropertyAccess().Name, propertyDetailExpression.GetPropertyAccess().Name)
                    );

        public virtual MasterDetailRelationshipBuilder HasDetailRelationship<TDetailEntity>(Expression<Func<TEntity, IEnumerable<TDetailEntity>>> collectionProperty)
            => new MasterDetailRelationshipBuilder(
                    Builder.HasDetailRelationship(typeof(TDetailEntity))
                );

        public virtual EntityTypeBuilder<TEntity> UseCssClasses(Action<GridCssClasses> configureCssClasses)
        {
            Builder.UseCssClasses(configureCssClasses);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> AppendCssClasses(Action<GridCssClasses> configureCssClasses)
        {
            Builder.AppendCssClasses(configureCssClasses);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> IsMasterTable()
        {
            Builder.IsMasterTable();

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> OnlyShowExplicitProperties()
        {
            Builder.OnlyShowExplicitProperties();

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> EnableGrouping(Action<GlobalGroupingOptions> confGroupingOptions = null)
        {
            var groupingOptions = new GlobalGroupingOptions();
            confGroupingOptions?.Invoke(groupingOptions);
            Builder.EnableGrouping(groupingOptions);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> IsMasterTable(Action<MasterDetailOptions> configureMasterDetails)
        {
            var masterDetails = new MasterDetailOptions();
            configureMasterDetails?.Invoke(masterDetails);
            Builder.IsMasterTable(masterDetails);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> AllowInlineEdit()
        {
            Builder.AllowInlineEdit(new InlineEditOptions());

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> AllowInlineEdit(Action<InlineEditOptions> configureInlineEdit)
        {
            var inlineEditOptions = new InlineEditOptions();
            configureInlineEdit?.Invoke(inlineEditOptions);

            Builder.AllowInlineEdit(inlineEditOptions);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> AllowCreateItem(Action<CreateItemOptions> configureCreateItem = null)
        {
            var createItemOptions = new CreateItemOptions();
            configureCreateItem?.Invoke(createItemOptions);
            createItemOptions.ModelType = createItemOptions.ModelType ?? typeof(TEntity);
            createItemOptions.OutputDtoType = createItemOptions.OutputDtoType ?? createItemOptions.ModelType;

            Builder.AllowCreateItem(createItemOptions);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> AllowCreateItem<TCreateModel, TOutputDto>(Action<CreateItemOptions> configureCreateItem = null)
        {
            var createItemOptions = new CreateItemOptions();
            configureCreateItem?.Invoke(createItemOptions);
            createItemOptions.ModelType = typeof(TCreateModel);
            createItemOptions.OutputDtoType = typeof(TOutputDto);

            Builder.AllowCreateItem(createItemOptions);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> ConfigureDeleteItem(Action<DeleteItemOptions> configureDeleteItem)
        {
            if (configureDeleteItem is null)
            {
                throw new ArgumentNullException(nameof(configureDeleteItem));
            }

            var deleteItemOptions = new DeleteItemOptions();
            configureDeleteItem.Invoke(deleteItemOptions);

            Builder.DeleteItemConfiguration(deleteItemOptions);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> EnableSortingForAllProperties()
        {
            foreach (var property in typeof(TEntity).GetProperties())
            {
                var configuredProperty = Builder.Metadata.FindProperty(property.Name);
                if (configuredProperty is null)
                {
                    Builder
                        .Property(property)
                        .IsSortable(true);
                }
                else
                {
                    new InternalPropertyBuilder(configuredProperty as Property, Builder.ModelBuilder)
                        .IsSortable(true);
                }
            }

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> HasEmptyItemsMessage(string message)
            => HasEmptyItemsMessage(message, false);

        public virtual EntityTypeBuilder<TEntity> HasEmptyItemsMessage(string message, bool renderGridHeader = true)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Builder.HasEmptyItemsMessage(message);
            Builder.RenderHeaderWithEmtyItemsMessage(renderGridHeader);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> DoNotUseDeleteConfirmDialog()
        {
            ConfigureDeleteItem(cfg => cfg.UseConfirmationDialog = false);

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> EnablePreserveFiltering()
        {
            Builder.PreserveFiltering();

            return this;
        }

        public virtual EntityTypeBuilder<TEntity> EnablePreservePagination()
        {
            Builder.PreservePagination();

            return this;
        }
    }
}
