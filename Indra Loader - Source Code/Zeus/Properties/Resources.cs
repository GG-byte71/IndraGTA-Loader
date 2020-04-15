using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Zeus.Properties
{
	// Token: 0x02000006 RID: 6
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000204F File Offset: 0x0000024F
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000278E File Offset: 0x0000098E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Zeus.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027BA File Offset: 0x000009BA
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000027C1 File Offset: 0x000009C1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027C9 File Offset: 0x000009C9
		internal static Bitmap logo_simple
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("logo_simple", Resources.resourceCulture);
			}
		}

		// Token: 0x0400001B RID: 27
		private static ResourceManager resourceMan;

		// Token: 0x0400001C RID: 28
		private static CultureInfo resourceCulture;
	}
}
