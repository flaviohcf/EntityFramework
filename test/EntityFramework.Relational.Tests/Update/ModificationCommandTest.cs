// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.ChangeTracking.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Update;
using Xunit;

namespace Microsoft.Data.Entity.Relational.Tests.Update
{
    public class ModificationCommandTest
    {
        [Fact]
        public void ModificationCommand_initialized_correctly_for_added_entities_with_temp_generated_key()
        {
            var entry = Createentry(EntityState.Added, generateKeyValues: true);
            entry.MarkAsTemporary(entry.EntityType.GetPrimaryKey().Properties[0]);

            var command = new ModificationCommand("T1", null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Added, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.True(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_added_entities_with_non_temp_generated_key()
        {
            var entry = Createentry(EntityState.Added, generateKeyValues: true);
            entry.MarkAsTemporary(entry.EntityType.GetPrimaryKey().Properties[0], isTemporary: false);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Added, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_added_entities_with_explicitly_specified_key_value()
        {
            var entry = Createentry(EntityState.Added);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Added, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_modified_entities_with_identity_key()
        {
            var entry = Createentry(EntityState.Modified, generateKeyValues: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Modified, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_modified_entities_with_client_generated_key()
        {
            var entry = Createentry(EntityState.Modified);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Modified, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.False(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.True(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_modified_entities_with_concurrency_token()
        {
            var entry = Createentry(EntityState.Modified, computeNonKeyValue: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Modified, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.True(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_deleted_entities()
        {
            var entry = Createentry(EntityState.Deleted);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Deleted, command.EntityState);
            Assert.Equal(1, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_initialized_correctly_for_deleted_entities_with_concurrency_token()
        {
            var entry = Createentry(EntityState.Deleted, computeNonKeyValue: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.Equal("T1", command.TableName);
            Assert.Null(command.SchemaName);
            Assert.Equal(EntityState.Deleted, command.EntityState);
            Assert.Equal(2, command.ColumnModifications.Count);

            var columnMod = command.ColumnModifications[0];

            Assert.Equal("Col1", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Id", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.True(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);

            columnMod = command.ColumnModifications[1];

            Assert.Equal("Col2", columnMod.ColumnName);
            Assert.Same(entry, columnMod.Entry);
            Assert.Equal("Name", columnMod.Property.Name);
            Assert.True(columnMod.IsCondition);
            Assert.False(columnMod.IsKey);
            Assert.False(columnMod.IsRead);
            Assert.False(columnMod.IsWrite);
        }

        [Fact]
        public void ModificationCommand_throws_for_unchanged_entities()
        {
            var entry = Createentry(EntityState.Unchanged);

            var command = new ModificationCommand("T1", null, new ParameterNameGenerator(), p => p.Relational());

            Assert.Equal(
                Strings.ModificationFunctionInvalidEntityState(EntityState.Unchanged),
                Assert.Throws<NotSupportedException>(() => command.AddEntry(entry)).Message);
        }

        [Fact]
        public void ModificationCommand_throws_for_unknown_entities()
        {
            var entry = Createentry(EntityState.Detached);

            var command = new ModificationCommand("T1", null, new ParameterNameGenerator(), p => p.Relational());

            Assert.Equal(
                Strings.ModificationFunctionInvalidEntityState(EntityState.Detached),
                Assert.Throws<NotSupportedException>(() => command.AddEntry(entry)).Message);
        }

        [Fact]
        public void RequiresResultPropagation_false_for_Delete_operation()
        {
            var entry = Createentry(
                EntityState.Deleted, generateKeyValues: true, computeNonKeyValue: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.False(command.RequiresResultPropagation);
        }

        [Fact]
        public void RequiresResultPropagation_true_for_Insert_operation_if_store_generated_columns_exist()
        {
            var entry = Createentry(
                EntityState.Added, generateKeyValues: true, computeNonKeyValue: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.True(command.RequiresResultPropagation);
        }

        [Fact]
        public void RequiresResultPropagation_false_for_Insert_operation_if_no_store_generated_columns_exist()
        {
            var entry = Createentry(EntityState.Added);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.False(command.RequiresResultPropagation);
        }

        [Fact]
        public void RequiresResultPropagation_true_for_Update_operation_if_non_key_store_generated_columns_exist()
        {
            var entry = Createentry(
                EntityState.Modified, generateKeyValues: true, computeNonKeyValue: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.True(command.RequiresResultPropagation);
        }

        [Fact]
        public void RequiresResultPropagation_false_for_Update_operation_if_no_non_key_store_generated_columns_exist()
        {
            var entry = Createentry(EntityState.Modified, generateKeyValues: true);

            var command = new ModificationCommand("T1",null, new ParameterNameGenerator(), p => p.Relational());
            command.AddEntry(entry);

            Assert.False(command.RequiresResultPropagation);
        }

        private class T1
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private static IModel BuildModel(bool generateKeyValues, bool computeNonKeyValue)
        {
            var model = new Entity.Metadata.Model();
            var entityType = model.AddEntityType(typeof(T1));

            var key = entityType.GetOrAddProperty("Id", typeof(int));
            key.GenerateValueOnAdd = generateKeyValues;
            key.Relational().Column = "Col1";
            entityType.GetOrSetPrimaryKey(key);

            var nonKey = entityType.GetOrAddProperty("Name", typeof(string));
            nonKey.IsConcurrencyToken = computeNonKeyValue;

            nonKey.Relational().Column = "Col2";
            nonKey.IsStoreComputed = computeNonKeyValue;

            return model;
        }

        private static InternalEntityEntry Createentry(
            EntityState entityState,
            bool generateKeyValues = false,
            bool computeNonKeyValue = false)
        {
            var model = BuildModel(generateKeyValues, computeNonKeyValue);

            return RelationalTestHelpers.Instance.CreateInternalEntry(model, entityState, new T1 { Id = 1, Name = "Test" });
        }
    }
}
