using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BarCodeWebApi.Models;
using ZXing;

namespace BarCodeWebApi.Controllers
{
    public class BarCodeController : ApiController
    {
        public async Task<BarCodeResult> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());

            var files = provider.Files;
            var file1 = files[0];
            var fileStream = await file1.ReadAsStreamAsync();

            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            var barcodeBitmap = new Bitmap(fileStream);
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(barcodeBitmap);
            // do something with the result

            var barCodeResult = new BarCodeResult();

            if (result != null)
            {
                barCodeResult.DecoderType = result.BarcodeFormat.ToString();
                barCodeResult.DecoderContent = result.Text;
            }

            return barCodeResult;
        }
    }
}
