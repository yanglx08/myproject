// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Microsoft.PowerPlatform.Formulas.Tools
{
    internal class MsAppTest
    {
        // Given an msapp (original source of truth), stress test the conversions
        public static bool StressTest(string pathToMsApp)
        {
            try
            {
                using (var temp1 = new TempFile())
                {
                    string outFile = temp1.FullPath;

                    var log = TextWriter.Null;

                    // MsApp --> Model
                    CanvasDocument msapp;
                    ErrorContainer errors = new ErrorContainer();
                    try
                    {
                        using (var stream = new FileStream(pathToMsApp, FileMode.Open))
                        {
                            msapp = MsAppSerializer.Load(stream, errors);
                        }
                        errors.ThrowOnErrors();
                    }
                    catch (NotSupportedException)
                    {
                        errors.FormatNotSupported($"Too old: {pathToMsApp}");
                        return false;
                    }

                    // Model --> MsApp
                    errors = msapp.SaveToMsApp(outFile);
                    errors.ThrowOnErrors();
                    var ok = MsAppTest.Compare(pathToMsApp, outFile, log);
                    if (!ok) { return false; }


                    // Model --> Source
                    using (var tempDir = new TempDir())
                    {
                        string outSrcDir = tempDir.Dir;
                        errors = msapp.SaveToSources(outSrcDir, verifyOriginalPath : pathToMsApp);
                        errors.ThrowOnErrors();
                        return true;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool Compare(string pathToZip1, string pathToZip2, TextWriter log)
        {
            var c1 = ChecksumMaker.GetChecksum(pathToZip1);
            var c2 = ChecksumMaker.GetChecksum(pathToZip2);
            if (c1.wholeChecksum == c2.wholeChecksum)
            {
                return true;
            }

            // If there's a checksum mismatch, do a more intensive comparison to find the difference.
#if DEBUG
            // Provide a comparison that can be very specific about what the difference is.
            var comp = new Dictionary<string, byte[]>();
            DebugChecksum(pathToZip1, log, comp, true);
            DebugChecksum(pathToZip2, log, comp, false);

            foreach (var kv in comp) // Remaining entries are errors.
            {
                Console.WriteLine("FAIL: 2nd is missing " + kv.Key);
            }
#endif
            return false;
        }

        // Compare the debug checksums. 
        // Get a hash for the MsApp file.
        // First pass adds file/hash to comp.
        // Second pass checks hash equality and removes files from comp.
        // AFter second pass, comp should be 0. any files in comp were missing from 2nd pass.
        public static void DebugChecksum(string pathToZip, TextWriter log, Dictionary<string,byte[]> comp, bool first)
        {
            using (var z = ZipFile.OpenRead(pathToZip))
            {
                foreach (ZipArchiveEntry e in z.Entries.OrderBy(x => x.FullName))
                {
                    var key = ChecksumMaker.ChecksumFile<DebugTextHashMaker>(e.FullName, e.ToBytes());
                    if (key ==null)
                    {
                        continue;
                    }
                    
                    // Do easy diffs
                    {
                        if (first)
                        {
                            comp.Add(e.FullName, key);
                        }
                        else
                        {
                            byte[] otherContents;
                            if (comp.TryGetValue(e.FullName, out otherContents))
                            {

                                bool same = key.SequenceEqual(otherContents);

                                if (!same)
                                {
                                    // Fail! Mismatch
                                    Console.WriteLine("FAIL: hash mismatch: " + e.FullName);

                                    // Write out normalized form. Easier to spot the diff.
                                    File.WriteAllBytes(@"c:\temp\a1.json", otherContents);
                                    File.WriteAllBytes(@"c:\temp\b1.json", key);

                                    // For debugging. Help find exactly where the difference is. 
                                    for (int i = 0; i < otherContents.Length; i++)
                                    {
                                        if (i >= key.Length)
                                        {
                                            break;
                                        }
                                        if (otherContents[i] != key[i])
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    // success
                                }
                                comp.Remove(e.FullName);
                            }
                            else
                            {
                                // Missing file!
                                Console.WriteLine("FAIL: 2nd has added file: " + e.FullName);
                            }
                        }
                    }
                }
            }
        }
    }
}
