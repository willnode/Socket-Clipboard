using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SocketClipboard
{
    public class InviterUtility
    {

        //declare the Netapi32 : NetServerEnum method import
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true),
        SuppressUnmanagedCodeSecurity]

        public static extern int NetServerEnum(
            string ServerNane, // must be null
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain, // null for login domain
            out int dwResumeHandle
            );

        //declare the Netapi32 : NetApiBufferFree method import
        [DllImport("Netapi32", SetLastError = true),
        SuppressUnmanagedCodeSecurity]

        public static extern int NetApiBufferFree(
            IntPtr pBuf);

        //create a _SERVER_INFO_100 STRUCTURE
        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal int sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }


        /// Uses the DllImport : NetServerEnum with all its required parameters
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
        public static void GetLocalComputers(List<string> list)
        {
            //local fields
            list.Clear();
            const int SV_TYPE_SERVER = 2;
            const int SV_TYPE_WORKSTATION = 1;
            const int MAX_PREFERRED_LENGTH = -1;
            IntPtr buffer = IntPtr.Zero, tmp = IntPtr.Zero;
            int entriesRead = 0, totalEntries = 0, resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));

            try
            {
                //call the DllImport : NetServerEnum with all its required parameters
                if (NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH, out entriesRead,
                    out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out resHandle) == 0)
                {
                    //loop through all SV_TYPE_WORKSTATION and SV_TYPE_SERVER PC's
                    for (int i = 0; i < totalEntries; i++)
                    {
                        tmp = new IntPtr((int)buffer + (i * sizeofINFO));

                        //add the PC names to the ArrayList
                        list.Add(Marshal.PtrToStructure<_SERVER_INFO_100>(tmp).sv100_name);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                // Free buffer
                NetApiBufferFree(buffer);
            }
        }

    }
}