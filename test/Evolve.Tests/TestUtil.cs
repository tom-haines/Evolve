﻿using System.IO;
using System.Text;
using EvolveDb.Connection;
using EvolveDb.Metadata;
using EvolveDb.Migration;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace EvolveDb.Tests
{
    internal static class TestUtil
    {
        public static void CreateSqlServerDatabase(string dbName, string cnxStr)
        {
            var cnn = new SqlConnection(cnxStr);
            cnn.Open();

            using (var cmd = cnn.CreateCommand())
            {
                cmd.CommandText = $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}') " +
                                  $"BEGIN " +
                                  $"CREATE DATABASE {dbName} " +
                                  $"END";

                cmd.ExecuteNonQuery();
            }

            cnn.Close();
        }

        public static WrappedConnection CreateSQLiteWrappedCnx() => new WrappedConnection(new SqliteConnection("Data Source=:memory:"));

        public static FileMigrationScript BuildFileMigrationScript(string path = null, string version = null, string description = null) =>
            new FileMigrationScript(
                path: path ?? TestContext.CrLfScriptPath,
                version: version ?? "1",
                description: description ?? "desc",
                type: MetadataType.Migration);

        public static FileMigrationScript BuildRepeatableFileMigrationScript(string path = null, string description = null, string version = null) =>
            new FileMigrationScript(
                path: path ?? TestContext.CrLfScriptPath,
                version: version ?? null,
                description: description ?? "desc",
                type: MetadataType.RepeatableMigration);

        public static EmbeddedResourceMigrationScript BuildEmbeddedResourceMigrationScript(string version = null, string description = null, string name = null, Stream content = null) =>
            new EmbeddedResourceMigrationScript(
                version: version ?? "1",
                description: description ?? "desc",
                name: name ?? "name",
                content: content ?? new MemoryStream(Encoding.UTF8.GetBytes("content")),
                type: MetadataType.Migration);

        public static EmbeddedResourceMigrationScript BuildRepeatableEmbeddedResourceMigrationScript(string name = null, string description = null, Stream content = null) =>
            new EmbeddedResourceMigrationScript(
                version: null,
                description: description ?? "desc",
                name: name ?? "name",
                content: content ?? new MemoryStream(Encoding.UTF8.GetBytes("content")),
                type: MetadataType.RepeatableMigration);

        public static MigrationMetadata BuildMigrationMetadata(string version = null, string description = null, string name = null) =>
            new MigrationMetadata(
                version: version ?? "1",
                description: description ?? "desc",
                name: name ?? "name",
                type: MetadataType.Migration);
    }
}
