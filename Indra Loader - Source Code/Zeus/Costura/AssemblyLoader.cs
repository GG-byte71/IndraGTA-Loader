using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura
{
	// Token: 0x02000008 RID: 8
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002809 File Offset: 0x00000A09
		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return "";
			}
			return culture.Name;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000281C File Offset: 0x00000A1C
		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002884 File Offset: 0x00000A84
		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028B8 File Offset: 0x00000AB8
		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
					{
						MemoryStream memoryStream = new MemoryStream();
						AssemblyLoader.CopyTo(deflateStream, memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000293C File Offset: 0x00000B3C
		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			string fullName;
			if (resourceNames.TryGetValue(name, out fullName))
			{
				return AssemblyLoader.LoadStream(fullName);
			}
			return null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000295C File Offset: 0x00000B5C
		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002984 File Offset: 0x00000B84
		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = requestedAssemblyName.CultureInfo.Name + "." + text;
			}
			byte[] rawAssembly;
			using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, text))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = AssemblyLoader.ReadStream(stream);
			}
			using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, text))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A44 File Offset: 0x00000C44
		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			object obj = AssemblyLoader.nullCacheLock;
			lock (obj)
			{
				if (AssemblyLoader.nullCache.ContainsKey(e.Name))
				{
					return null;
				}
			}
			AssemblyName assemblyName = new AssemblyName(e.Name);
			Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
			if (assembly == null)
			{
				obj = AssemblyLoader.nullCacheLock;
				lock (obj)
				{
					AssemblyLoader.nullCache[e.Name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B24 File Offset: 0x00000D24
		// Note: this type is marked as 'beforefieldinit'.
		static AssemblyLoader()
		{
			AssemblyLoader.assemblyNames.Add("costura", "costura.costura.dll.compressed");
			AssemblyLoader.assemblyNames.Add("icsharpcode.sharpziplib", "costura.icsharpcode.sharpziplib.dll.compressed");
			AssemblyLoader.symbolNames.Add("icsharpcode.sharpziplib", "costura.icsharpcode.sharpziplib.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesigncolors", "costura.materialdesigncolors.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesigncolors", "costura.materialdesigncolors.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("penet.asn1", "costura.penet.asn1.dll.compressed");
			AssemblyLoader.assemblyNames.Add("penet", "costura.penet.dll.compressed");
			AssemblyLoader.assemblyNames.Add("reloaded.assembler", "costura.reloaded.assembler.dll.compressed");
			AssemblyLoader.symbolNames.Add("reloaded.assembler", "costura.reloaded.assembler.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("reloaded.injector", "costura.reloaded.injector.dll.compressed");
			AssemblyLoader.assemblyNames.Add("reloaded.memory.buffers", "costura.reloaded.memory.buffers.dll.compressed");
			AssemblyLoader.assemblyNames.Add("reloaded.memory", "costura.reloaded.memory.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.buffers", "costura.system.buffers.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.json", "costura.system.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.memory", "costura.system.memory.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002CD5 File Offset: 0x00000ED5
		public static void Attach()
		{
			if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
			{
				return;
			}
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoader.ResolveAssembly;
		}

		// Token: 0x0400001E RID: 30
		private static object nullCacheLock = new object();

		// Token: 0x0400001F RID: 31
		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		// Token: 0x04000020 RID: 32
		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		// Token: 0x04000021 RID: 33
		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		// Token: 0x04000022 RID: 34
		private static int isAttached;
	}
}
