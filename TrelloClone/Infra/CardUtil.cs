using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloClone.Dtos;

namespace TrelloClone.Infra
{
    public static class CardUtil
    {
        public static IEnumerable<T> SetTreeOrder<T>(this IEnumerable<T> list)
            where T : TreeBaseDto
        {
            var result = RecursiveTreeOrder(list.ToList(), null, 0);
            return result.OrderBy(x => x.order);
        }

        private static List<T> RecursiveTreeOrder<T>(List<T> list, int? prevSeq, int order)
            where T : TreeBaseDto
        {
            foreach (var item in list.Where(x => x.prevSeq == prevSeq)) {
                item.order = order;
                RecursiveTreeOrder(list, item.seq, ++order);
            }

            return list;
        }

        public static string GetHashId(string email, int boardSeq)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("{0}?{1}", email, boardSeq));
            int[] ints = Array.ConvertAll(bytes, Convert.ToInt32);

            var hashids = new Hashids("trelloclone");
            return hashids.Encode(ints);
        }

        public static HashIdKeyDto DecodeHashId(string hashId)
        {
            var hashids = new Hashids("trelloclone");
            var numbers = hashids.Decode(hashId);
            var restoreByte = Array.ConvertAll(numbers, Convert.ToByte);
            string restoreText = Encoding.ASCII.GetString(restoreByte);

            var arr = restoreText.Split('?');
            return new HashIdKeyDto {
                email = arr[0],
                boardSeq = int.Parse(arr[1])
            };
        }
    }
}
