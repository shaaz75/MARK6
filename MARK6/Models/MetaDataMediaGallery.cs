using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MARK6.Models
{
	[MetadataType(typeof(MediaGalleryMetaData))]
	public partial class MediaGallery
	{
	}

	public class MediaGalleryMetaData
	{
		public int MediaId { get; set; }
		[Required(ErrorMessage = "Title is required.")]
		[MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "File is required.")]
		[DisplayName("File")]
		public string MediaPath { get; set; }

		public string MediaType { get; set; }
		public HttpPostedFileBase MediaFile { get; set; }
	}
}