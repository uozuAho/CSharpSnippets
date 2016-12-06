using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Runtime.Serialization;
using System.Text;

namespace CSharpSnippets.Serialization
{
    /// <summary>
    /// Import/export arbitrary class data to/from file
    /// Note: This requires inclusion of WindowsBase.dll
    /// </summary>
    class ClassDataPacker : IDisposable
    {
        private Stream _outputStream;
        private Package _package;

        public ClassDataPacker()
        {
        }

        public ClassDataPacker(Stream stream)
        {
            _outputStream = stream;
            _package = Package.Open(_outputStream, FileMode.Create);
        }

        public static ClassDataPacker OpenPack(string path)
        {
            var pack = new ClassDataPacker();
            pack._package = Package.Open(path);
            return pack;
        }

        public void WriteEntity<T>(T entity, Func<T, object> primaryKeySelector) where T : class
        {
            WriteEntity(entity, obj => new[] { primaryKeySelector(obj) });
        }

        public void WriteEntity<T>(T entity, Func<T, object[]> primaryKeySelector) where T : class
        {
            SerialiseEntityToPackagePart(BuildPackagePartUri<T>(primaryKeySelector(entity)), entity);
        }

        public void WriteEntities<T>(IEnumerable<T> entities, Func<T, object> primaryKeySelector) where T : class
        {
            WriteEntities(entities, arg => new[] { primaryKeySelector(arg) });
        }

        public void WriteEntities<T>(IEnumerable<T> entities, Func<T, object[]> primaryKeySelector) where T : class
        {
            foreach (var entity in entities)
            {
                WriteEntity(entity, primaryKeySelector);
            }
        }

        public PackagePartCollection ListParts()
        {
            return _package.GetParts();
        }

        public T GetEntity<T>(object primaryKey) where T : class
        {
            var part = _package.GetPart(BuildPackagePartUri<T>(primaryKey));
            if (part == null)
                return null;
            return Deserialize<T>(part);
        }

        private void SerialiseEntityToPackagePart<T>(Uri partUri, T entity) where T : class
        {
            PackagePart packagePart;
            if (_package.PartExists(partUri))
                packagePart = _package.GetPart(partUri);
            else
                packagePart = _package.CreatePart(partUri, System.Net.Mime.MediaTypeNames.Text.Xml);

            if (packagePart == null)
                return;

            using (MemoryStream memStream = new MemoryStream())
            {
                SerializeToStream(memStream, entity);
                memStream.Seek(0, SeekOrigin.Begin);
                memStream.CopyTo(packagePart.GetStream());
            }
            _package.CreateRelationship(partUri, TargetMode.Internal, "blah blah relationship type");
            _package.Flush();
        }

        private static void SerializeToStream<T>(Stream stream, T entity) where T : class
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, entity);
        }

        private static T Deserialize<T>(PackagePart part) where T : class
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(part.GetStream());
        }

        public static Uri BuildPackagePartUri<T>(params object[] primaryKeys) where T : class
        {
            var entityType = typeof(T);
            var typeName = entityType.Name;

            StringBuilder builder = new StringBuilder();
            foreach (object key in primaryKeys)
            {
                builder.Append("_");
                builder.Append(key.ToString());
            }

            return new Uri(
                string.Format(CultureInfo.CurrentCulture, "/{0}/{1}{2}.xml", entityType.Name, typeName, builder),
                UriKind.Relative);
        }

        public void Dispose()
        {
            if (_package != null)
            {
                _package.Flush();
                _package.Close();
            }
        }
    }
}
