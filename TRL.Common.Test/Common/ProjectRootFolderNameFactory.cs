using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Test
{
    public static class ProjectRootFolderNameFactory
    {
        public static string Make()
        {
            string result = string.Empty; ; 

            string[] pathParts = AppDomain.CurrentDomain.BaseDirectory.Split('\\');

            for (int i = 0; i < pathParts.Length - 3; i++)
            {
                if (i == 0)
                    result += pathParts[i];
                else
                    result += string.Concat("\\", pathParts[i]);

            }

            return String.Concat(result, "\\TRL.Common.Test");

        }
    }
}
