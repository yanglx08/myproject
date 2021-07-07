// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.AppMagic.Persistence.Converters;
using Microsoft.PowerPlatform.Formulas.Tools.Schemas;
using Microsoft.PowerPlatform.Formulas.Tools.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("PAModelTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("PASopa, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
namespace Microsoft.PowerPlatform.Formulas.Tools
{
    // Various utility methods.
    internal static class Utilities
    {
        public const int MaxNameLength = 50;

        // Allows using with { } initializers, which require an Add() method.
        public static void Add<T>(this Stack<T> stack, T item)
        {
            stack.Push(item);
        }

        public static IEnumerable<T> NullOk<T>(this IEnumerable<T> list)
        {
            if (list == null) return Enumerable.Empty<T>();
            return list;
        }

        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> thisDictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
            foreach (var kv in other)
            {
                thisDictionary[kv.Key] = kv.Value;
            }
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
            where TValue : new()
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            return @default;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            value = new TValue();
            dict[key] = value;
            return value;
        }

        static JsonSerializerOptions GetJsonOptions()
        {
            var opts = new JsonSerializerOptions();

            // encodes quote as \" rather than unicode.
            opts.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            opts.Converters.Add(new JsonStringEnumConverter());

            opts.Converters.Add(new JsonDateTimeConverter());
            opts.Converters.Add(new JsonVersionConverter());

            opts.WriteIndented = true;
            opts.IgnoreNullValues = true;

            return opts;
        }

        public static JsonSerializerOptions _jsonOpts = GetJsonOptions();

        // https://stackoverflow.com/questions/58138793/system-text-json-jsonelement-toobject-workaround

        public static string JsonSerialize<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj, _jsonOpts);
        }

        public static T JsonParse<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonOpts);
        }

        public static T ToObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json, _jsonOpts);
        }
        public static T ToObject<T>(this JsonDocument document)
        {
            var json = document.RootElement.GetRawText();
            return JsonSerializer.Deserialize<T>(json, _jsonOpts);
        }

        public static byte[] ToBytes(this JsonElement e)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(e, _jsonOpts);
            return bytes;
        }


        public static byte[] ToBytes(this ZipArchiveEntry e)
        {
            using (var s = e.Open())
            {
                // Non-seekable stream.
                var len = e.Length;
                var buffer = new byte[len];
                s.Read(buffer, 0, (int)len);
                return buffer;
            }
        }

        // JsonElement is loss-less, handles unknown fields without dropping them.
        // Convertering to a Poco drops fields we don't recognize.
        public static JsonElement ToJson(this ZipArchiveEntry e)
        {
            using (var s = e.Open())
            {
                var doc = JsonDocument.Parse(s);
                return doc.RootElement;
            }
        }

        /// <summary>
        /// Create a relative path from one path to another. Paths will be resolved before calculating the difference.
        /// Default path comparison for the active platform will be used (OrdinalIgnoreCase for Windows or Mac, Ordinal for Unix).
        ///   basePath:     C:\foo
        ///   full: c:\foo\bar\hi.txt
        ///   returns "bar\hi.txt"
        /// </summary>
        /// <param name="relativeTo">The source path the output should be relative to. This path is always considered to be a directory.</param>
        /// <param name="fullPathFile">The destination path. This is always a full path to a file.</param>
        /// <returns>The relative path or <paramref name="path"/> if the paths don't share the same root.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="relativeTo"/> or <paramref name="path"/> is <c>null</c> or an empty string.</exception>
        /// <remarks>
        /// Want to use Path.GetRelativePath() from Net 2.1. But since we target netstandard 2.0, we need to shim it.
        /// Convert to URIs and make the relative path. 
        /// see https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path
        /// For reference, see Core's impl at: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/IO/Path.cs#L861
        /// </remarks>
        public static string GetRelativePath(string relativeTo, string fullPathFile)
        {
            // First arg is always a path name, 2nd arg is always a directory.
            // directory is always a prefix. 
            Uri fromUri = new Uri(AppendDirectorySeparatorChar(relativeTo));
            Uri toUri = new Uri(fullPathFile);

            // path can't be made relative.
            if (fromUri.Scheme != toUri.Scheme)
                return fullPathFile;

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals(Uri.UriSchemeFile, StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static string AppendDirectorySeparatorChar(string fullDirectory)
        {
            // Append a slash only if the path does not already have a slash.
            if (!fullDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return fullDirectory + Path.DirectorySeparatorChar;
            }

            return fullDirectory;
        }

        public static void EnsureNoExtraData(Dictionary<string, JsonElement> extra)
        {
            if (extra != null && extra.Count > 0)
            {
                throw new NotSupportedException("There are fields in json we don't recognize");
            }
        }

        // Useful when we need to mutate an object (such as adding back properties)
        // but don't want to mutate the original.
        public static T JsonClone<T>(this T obj)
        {
            var str = JsonSerializer.Serialize(obj, _jsonOpts);
            var obj2 = JsonSerializer.Deserialize<T>(str, _jsonOpts);
            return obj2;
        }

        public static IList<T> Clone<T>(this IList<T> obj) where T : ICloneable<T>
        {
            if (obj == null)
                return null;
            return obj.Select(item => item.Clone()).ToList();
        }

        private const char EscapeChar = '%';

        private static bool DontEscapeChar(char ch)
        {
            // List of "safe" characters that aren't escaped.
            // Note that the EscapeChar must be escaped, so can't appear on this list!
            return
                (ch >= '0' && ch <= '9') ||
                (ch >= 'a' && ch <= 'z') ||
                (ch >= 'A' && ch <= 'Z') ||
                (ch == '[' || ch == ']') || // common in SQL connection names
                (ch == '_') ||
                (ch == '-') ||
                (ch == '~') ||
                (ch == '.') ||
                (ch == ' '); // allow spaces, very common.
        }

        // For writing out to a director.
        public static string EscapeFilename(string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ch in path)
            {
                var x = (int)ch;
                if (DontEscapeChar(ch) || x > 255)
                {
                    sb.Append(ch);
                }
                else
                {
                    if (x <= 255)
                    {
                        sb.Append(EscapeChar);
                        sb.Append(x.ToString("x2"));
                    }
                }
            }
            return sb.ToString();
        }

        private static int ToHex(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return ((int)ch) - '0';
            }
            if (ch >= 'a' && ch <= 'f')
            {
                return (((int)ch) - 'a') + 10;
            }
            if (ch >= 'A' && ch <= 'F')
            {
                return (((int)ch) - 'A') + 10;
            }
            throw new InvalidOperationException($"Unrecognized hex char {ch}");
        }

        // Unescaped is backwards compat.
        public static string UnEscapeFilename(string path)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < path.Length; i++)
            {
                var ch = path[i];
                if (ch == EscapeChar)
                {
                    // Unescape
                    int x;
                    if (path[i + 1] == EscapeChar)
                    {
                        i++;
                        x = ToHex(path[i + 1]) * 16 * 16 * 16 +
                            ToHex(path[i + 2]) * 16 * 16 +
                            ToHex(path[i + 3]) * 16 +
                            ToHex(path[i + 4]);
                        i += 4;
                    }
                    else
                    {
                        // 2 digit
                        x = ToHex(path[i + 1]) * 16 +
                            ToHex(path[i + 2]);
                        i += 2;
                    }
                    sb.Append((char)x);
                }
                else
                {
                    // Anything that is not explicitly escaped gets copied.
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }

        public static string UnEscapePAString(string text)
        {
            return text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
        }

        public static string EscapePAString(string text)
        {
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input.First().ToString().ToUpper() + input.Substring(1)
        };

        public static void EnsurePathRooted(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                throw new ArgumentException("Path must be a full path: " + path);
            }
        }

        /// <summary>
        /// Relative path of the file depends on the fileType. eg: Image files go into "Assets\Images\"
        /// </summary>
        public static string GetResourceRelativePath(ContentKind contentType)
        {
            switch (contentType)
            {
                case ContentKind.Image:
                    return @"Assets\Images";
                case ContentKind.Audio:
                    return @"Assets\Audio";
                case ContentKind.Video:
                    return @"Assets\Video";
                case ContentKind.Pdf:
                    return @"Assets\Pdf";
                default:
                    throw new NotSupportedException("Unrecognized Content Kind for local resource");
            }
        }

        /// <summary>
        /// If the name length is longer than 50, it is truncated and appended with a hash (to avoid collisions).
        /// Checks the length of the escaped name, since its possible that the length is under 60 before escaping but goes beyond 60 later.
        /// We do modulo by 1000 of the hash to limit it to 3 characters.
        /// </summary>
        /// <param name="name">The name that needs to be truncated.</param>
        /// <returns>Returns the truncated name if the escaped version of it is longer than 50 characters.</returns>
        public static string TruncateNameIfTooLong(string name)
        {
            var escapedName = EscapeFilename(name);
            if (escapedName.Length > MaxNameLength)
            {
                // limit the hash to 3 characters by doing a module by 4096 (16^3)
                var hash = (GetHash(escapedName) % 4096).ToString("x3");
                escapedName = TruncateName(escapedName, MaxNameLength - hash.Length -1) + "_" + hash;
            }
            return escapedName;
        }

        /// <summary>
        /// Truncates a string with the given length and strips off incomplete escape characters if any.
        /// Each EscapeChar (%) must be followed by two integer values if that is not the case then it is likely that the truncation left incomplete escapes.
        /// </summary>
        /// <param name="name">The string to be truncated</param>
        /// <param name="length">The max length of the truncated string.</param>
        /// <returns>Truncated string.</returns>
        private static string TruncateName(string name, int length)
        {
            int removeTrailingCharsLength = name[length - 1] == EscapeChar ? 1 : (name[length - 2] == EscapeChar ? 2 : 0);
            return name.Substring(0, length - removeTrailingCharsLength);
        }

        /// <summary>
        /// djb2 algorithm to compute the hash of a string
        /// This must be determinstic and stable since it's used in file names
        /// </summary>
        /// <param name="str">The string for which we need to compute the hash.</param>
        /// <returns></returns>
        public static ulong GetHash(string str)
        {
            ulong hash = 5381;
            foreach (char c in str)
            {
                hash = ((hash << 5) + hash) + c;
            }

            return hash;
        }


        public static void VerifyFileExists(ErrorContainer errors, string fullpath)
        {
            if (!File.Exists(fullpath))
            {
                errors.BadParameter($"File not found: {fullpath}");
            }
        }

        public static void VerifyDirectoryExists(ErrorContainer errors, string fullpath)
        {
            if (!Directory.Exists(fullpath))
            {
                errors.BadParameter($"Directory not found: {fullpath}");
            }
        }
    }
}
