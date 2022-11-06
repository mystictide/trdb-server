using Newtonsoft.Json;
using RestSharp;
using System.Drawing;
using System.Drawing.Imaging;
using trdb.api.Models;
using trdb.entity.Movies;
using trdb.entity.Returns;
using trdb.entity.Users;

namespace trdb.api.Helpers
{
    public class CustomHelpers
    {
        public static string tmdb_key = "c33e76a04be19de0f46ae6301aec3a6a";
        public static async Task<string> SendRequest(string url, Method method)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(url, method);
                RestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode != System.Net.HttpStatusCode.NotModified)
                {
                    return JsonConvert.DeserializeObject(response.Content).ToString();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsResponseSuccessful(string jsonString)
        {
            try
            {
                var status = JsonConvert.DeserializeObject<TMDB_Error>(jsonString);
                if (status.Code > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static WeeklyReturn CastMovieAsWeekly(Movies obj)
        {
            return JsonConvert.DeserializeObject<WeeklyReturn>(JsonConvert.SerializeObject(obj));
        }
        public static List<Movies> CastObjectsAsMovies(List<object> obj)
        {
            return JsonConvert.DeserializeObject<List<Movies>>(JsonConvert.SerializeObject(obj));
        }

        public static List<UserReturn> CastUsersAsUserReturns(List<Users> obj)
        {
            return JsonConvert.DeserializeObject<List<UserReturn>>(JsonConvert.SerializeObject(obj));
        }

        public static List<MovieReturn> CastObjectsAsSimpleMovies(List<object> obj)
        {
            return JsonConvert.DeserializeObject<List<MovieReturn>>(JsonConvert.SerializeObject(obj));
        }

        public static List<MovieReturn> CastMoviesAsSimpleMovies(List<Movies> movies)
        {
            return JsonConvert.DeserializeObject<List<MovieReturn>>(JsonConvert.SerializeObject(movies));
        }

        public static Bitmap Base64ToBitmap(IFormFile file)
        {
            try
            {
                Bitmap bmpReturn = null;
                string base64String = "";
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    base64String = stream.ReadToEnd();
                    base64String = base64String.Split(',')[1];
                }
                byte[] byteBuffer = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(byteBuffer))
                {
                    bmpReturn = (Bitmap)Bitmap.FromStream(ms);
                }
                return bmpReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<Bitmap> ResizeImage(Bitmap original, int width, int height)
        {
            try
            {
                Bitmap resizedImage;

                int rectHeight = width;
                int rectWidth = height;

                if (original.Height == original.Width)
                {
                    resizedImage = new Bitmap(original, rectHeight, rectHeight);
                }
                else
                {
                    float aspect = original.Width / (float)original.Height;
                    int newWidth, newHeight;
                    newWidth = (int)(rectWidth * aspect);
                    newHeight = (int)(newWidth / aspect);

                    if (newWidth > rectWidth || newHeight > rectHeight)
                    {
                        if (newWidth > newHeight)
                        {
                            newWidth = rectWidth;
                            newHeight = (int)(newWidth / aspect);
                        }
                        else
                        {
                            newHeight = rectHeight;
                            newWidth = (int)(newHeight * aspect);
                        }
                    }
                    resizedImage = new Bitmap(original, newWidth, newHeight);
                }
                return resizedImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string PathFromUserID(string UserID)
        {
            return "/" + string.Join("/", UserID.ToArray()) + "/";
        }

        public static bool WriteImage(Bitmap bitmap, string path, string filename)
        {
            try
            {
                Directory.CreateDirectory(path);
                using (Bitmap bmp = new Bitmap(bitmap))
                {
                    bmp.Save(path + filename, ImageFormat.Jpeg);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<string> SaveUserAvatar(int UserID, string envPath, IFormFile file)
        {
            try
            {
                var original = Base64ToBitmap(file);
                var small = await ResizeImage(original, 220, 220);
                var large = await ResizeImage(original, 1000, 1000);
                var userPath = PathFromUserID(UserID.ToString());
                var savePath = envPath + "/media/avatars/user" + userPath;
                if (small != null && large != null)
                {
                    var writeSmall = WriteImage(small, savePath, "ua-small.jpg");
                    var writeLarge = WriteImage(large, savePath, "ua-large.jpg");

                    if (writeSmall && writeLarge)
                    {
                        return userPath;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}