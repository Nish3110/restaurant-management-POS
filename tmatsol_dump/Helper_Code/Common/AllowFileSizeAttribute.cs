namespace tmatsol_dump.Helper_Code.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    //Allow File Size Attribute to DataAnnotationLib

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple =false,Inherited =true)]
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        #region Public / Protected Properties

        //Gets or Sets File Size Propert. Default is 1 GB / Values are always in Bytes.
        public int FileSize { get; set; } = 1 * 1024 * 1024;

        #endregion

        #region Is valid method
        //Is Valid Method / Returns True Is Specific Extension Matches.

        public override bool IsValid(object value)
        {
            //Initialization
            HttpPostedFileBase file = value as HttpPostedFileBase;
            bool isValid = true;

            //Settings
            int allowedFileSize = this.FileSize;

            //Verification
            if (file != null)
            {
                //Initialization
                var fileSize = file.ContentLength;

                //Settings
                isValid = fileSize <= allowedFileSize;
            }
            //Info
            return isValid;
        }

        #endregion
    }
}