using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000002 RID: 2
public class BasicInject
{
	// Token: 0x06000003 RID: 3
	[DllImport("kernel32.dll")]
	private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

	// Token: 0x06000004 RID: 4
	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	// Token: 0x06000005 RID: 5
	[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	// Token: 0x06000006 RID: 6 RVA: 0x00002058 File Offset: 0x00000258
	public static int Main(string dllName)
	{
		Process process = Process.GetProcessesByName("GTA5")[0];
		IntPtr hProcess = BasicInject.OpenProcess(1082, false, process.Id);
		IntPtr procAddress = BasicInject.GetProcAddress(BasicInject.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
		IntPtr intPtr = BasicInject.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), 12288U, 4U);
		UIntPtr uintPtr;
		BasicInject.WriteProcessMemory(hProcess, intPtr, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out uintPtr);
		BasicInject.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, IntPtr.Zero);
		return 0;
	}

	// Token: 0x06000007 RID: 7
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	// Token: 0x06000008 RID: 8
	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	// Token: 0x06000009 RID: 9
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

	// Token: 0x04000001 RID: 1
	private const int PROCESS_CREATE_THREAD = 2;

	// Token: 0x04000002 RID: 2
	private const int PROCESS_QUERY_INFORMATION = 1024;

	// Token: 0x04000003 RID: 3
	private const int PROCESS_VM_OPERATION = 8;

	// Token: 0x04000004 RID: 4
	private const int PROCESS_VM_WRITE = 32;

	// Token: 0x04000005 RID: 5
	private const int PROCESS_VM_READ = 16;

	// Token: 0x04000006 RID: 6
	private const uint MEM_COMMIT = 4096U;

	// Token: 0x04000007 RID: 7
	private const uint MEM_RESERVE = 8192U;

	// Token: 0x04000008 RID: 8
	private const uint PAGE_READWRITE = 4U;
}
