using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GlobalConstants
    {
        public static int MENU_OBJECT_TYPE = 0;
        public static int LINE_OBJECT_TYPE = 1;
        public static int NODE_OBJECT_TYPE = 2;

        public static int CUSTOMER_ID = 0;
        public static int getCustomer()
        {

            return CUSTOMER_ID;
        }

        //Permission

        public static int ROLE_SUPPER_ADMIN = 0;
        public static int ROLE_ADMIN = 1;
        public static int ROLE_USER = 2;
        public static int ROLE_MANGER = 3;
        public static string ROLE_SESSION = "ROLE_SESSION";
        public static string PERMISSION_SESSION = "PERMISSION_SESSION";
        public static string MENU_SESSION = "MENU_SESSION";
        public static string GROUPIMAGE_SESSION = "GROUPIMAGE_SESSION";


        public static string AV_ADMIN_GROUP = "ADMIN";
        public static string AV_SUPPER_ADMIN_GROUP = "SUPPER_ADMIN";
        public static string AV_STAFF_GROUP = "STAFF";
        public static string AV_MANAGER_GROUP = "MANAGER";
        public static string AV_CONFIG_GROUP = "CONFIG";
        public static string AV_STAFF_PlASTIC_GROUP = "STAFF_PLASTIC";
        public static string AV_STAFF_COPPER_GROUP = "STAFF_COPPER";
        public static string CUSTOMER_SESSION = "CUSTOMER_SESSION";
        public static string USER_SESSION = "USER_SESSION";
        public static string GROUP_SESSION = "GROUP_SESSION";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CartSession = "CartSession";



        // ariston
        public static string ARIS_LINE_LEADER_GROUP = "LINE_LEADER";

        public static string ARIS_OPERATOR_GROUP = "OPERATOR";
        public static string ARIS_ADMIN_GROUP = "ADMIN";
        public static string ARIS_MANAGER_GROUP = "MANAGER";
        public static string ARIS_SUPPER_ADMIN_GROUP = "SUPPER_ADMIN";

        public static string ARIS_PRODUCTION_GROUP = "PRODUCTION";
        public static string ARIS_QUALITY_GROUP = "QUALITY";
        public static string ARIS_MAINTENANCE_GROUP = "MAINTENANCE";
        public static string ARIS_LOGISTICS_GROUP = "LOGISTICS";

        public static string LANG_SESSION = "LANG_SESSION";
        public static string LANG_VI = "vi";
        public static string LANG_VI_Text = "Việt Nam";
        public static string LANG_EN = "en";
        public static string LANG_EN_Text = "English";

    }

    public class UserFunction
    {
        public static bool USER_LINE_STAFF(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_STAFF_GROUP || strRole == Common.GlobalConstants.ARIS_LINE_LEADER_GROUP)
            {
                return true;
            }
            return false;
        }
        public static bool USER_AVADMIN_STAFF(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_ADMIN_GROUP
                || strRole == Common.GlobalConstants.AV_MANAGER_GROUP
                )
            {
                return true;
            }
            return false;
        }
        public static bool USER_REPORT(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_ADMIN_GROUP
                || strRole == Common.GlobalConstants.AV_STAFF_GROUP
                || strRole == Common.GlobalConstants.ARIS_LINE_LEADER_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                )
            {
                return true;
            }
            return false;
        }
        public static bool USER_MENU_PRODUCTION(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_ADMIN_GROUP
                || strRole == Common.GlobalConstants.ARIS_LINE_LEADER_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                )
            {
                return true;
            }
            return false;
        }
        public static bool USER_DICTIONARY(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_ADMIN_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP

                )
            {
                return true;
            }
            return false;
        }

        public static bool USER_LINE_LEADER(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_LINE_LEADER_GROUP)
            {
                return true;
            }
            return false;
        }
        public static bool USER_GROUP_ALL(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_LOGISTICS_GROUP
                || strRole == Common.GlobalConstants.ARIS_MAINTENANCE_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                || strRole == Common.GlobalConstants.ARIS_PRODUCTION_GROUP
                || strRole == Common.GlobalConstants.ARIS_QUALITY_GROUP
                || strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP
                || strRole == Common.GlobalConstants.ARIS_LINE_LEADER_GROUP
                )
            {
                return true;
            }
            return false;
        }
        public static bool USER_GROUP_ALL_NOTLEADER(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_LOGISTICS_GROUP
                || strRole == Common.GlobalConstants.ARIS_MAINTENANCE_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                || strRole == Common.GlobalConstants.ARIS_PRODUCTION_GROUP
                || strRole == Common.GlobalConstants.ARIS_QUALITY_GROUP
                || strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP
                )
            {
                return true;
            }
            return false;
        }
        public static bool USER_GROUP_MANAGER(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                || strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP
                )
            {
                return true;
            }
            return false;
        }

        public static bool USER_PRODUCTION(string strRole)
        {
            if (
                strRole == Common.GlobalConstants.ARIS_PRODUCTION_GROUP
               )
            {
                return true;
            }
            return false;
        }
        public static bool USER_QUALITY(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_QUALITY_GROUP
               )
            {
                return true;
            }
            return false;
        }
        public static bool USER_ADMIN(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP
              )
            {
                return true;
            }
            return false;
        }
        public static bool USER_CONFIG(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP
                || strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP
                || strRole == Common.GlobalConstants.ARIS_SUPPER_ADMIN_GROUP
              )
            {
                return true;
            }
            return false;
        }


        public static bool DELETE_PERMISSION(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_ADMIN_GROUP)
                return true;
            if (strRole == Common.GlobalConstants.AV_MANAGER_GROUP)
                return true;
            if (strRole == Common.GlobalConstants.AV_CONFIG_GROUP)
                return true;

            return false;
        }
        public static bool USER_MANAGE_PERMISSION(string strRole)
        {
            if (strRole == Common.GlobalConstants.ARIS_MANAGER_GROUP)
                return true;
            if (strRole == Common.GlobalConstants.ARIS_ADMIN_GROUP)
                return true;


            return false;
        }
        public static bool USER_CONFIG_PERMISSION(string strRole)
        {
            if (strRole == Common.GlobalConstants.AV_CONFIG_GROUP
                || strRole == Common.GlobalConstants.AV_SUPPER_ADMIN_GROUP
                )
                return true;
            return false;
        }

        public static bool VIEW_COST_PERMISSION(string strRole)
        {
            //if (strRole == Common.GlobalConstants.AV_ACCOUNTANT_GROUP)
            //    return true;
            //if (strRole == Common.GlobalConstants.AV_MANAGER_GROUP)
            //    return true;

            return false;
        }



    }
}
