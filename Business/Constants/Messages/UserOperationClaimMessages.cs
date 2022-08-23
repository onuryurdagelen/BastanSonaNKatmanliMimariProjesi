using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class UserOperationClaimMessages
    {
        public static string AddedUserOperationClaim = "Yetki başarıyla oluşturuldu.";
        public static string UpdatedUserOperationClaim = "Yetki başarıyla güncellendi.";
        public static string DeletedUserOperationClaim = "Yetki başarıyla silindi.";

        //public static string IsOperationSetExist = "Bu kullanıcıya bu yetki daha önce atanmış";
        public static string OperationClaimNotExist = "Seçtiğiniz yetki bilgisi yetkilerde bulunmamaktadır.";
        public static string UserNotExist = "Seçtiğiniz kullanıcı bulunamadı.";
        public static string OperationClaimSetExist = "Bu kullanıcıya bu yetki daha önce atanmış";
    }
}
