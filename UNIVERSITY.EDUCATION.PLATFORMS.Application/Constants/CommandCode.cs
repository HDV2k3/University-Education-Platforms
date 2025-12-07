using System.ComponentModel;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Constants
{
    public enum CommandCode

    {

        #region User

        [Description("Xem user")]
        VIEW_USER,

        [Description("Tạo user")]
        CREATE_USER,

        [Description("Xóa user")]
        DELETE_USER,

        [Description("Chỉnh sửa user")]

        UPDATE_USER,

        #endregion
        #region Role

        [Description("Xem vai trò")]
        VIEW_ROLE,

        [Description("Tạo vai trò")]
        CREATE_ROLE,

        [Description("Xóa vai trò")]
        DELETE_ROLE,

        [Description("Chỉnh vai vài trò")]

        UPDATE_ROLE,

        #endregion

        #region System

        [Description("Full quyền hệ thống")]
        FULL_CONTROLL,

        #endregion

    }
}
