using Blogpp.data;
using Blogpp.Models;

namespace Blogpp.Services
{
    public class TopicService : ITopicService
    {
        private readonly BlogContext context;

        public TopicService(BlogContext _context)
        {
            context = _context;
        }
        //public Topic add(TopicDTO topic)
        //{
        //    var isFind = context.Topics.Where(p=>p.Name == topic.Name).FirstOrDefault();
        //    if (isFind != null)
        //    {
        //        return isFind;
        //    }
        //    else
        //    {
        //        Topic topic1 = new Topic();
        //        topic1.Name = topic.Name;
        //        context.Topics.Add(topic1);
        //        var result = context.SaveChanges();
        //        return topic1;
        //    }

        //}

        public List<Topic> add(TopicDTO topic)
        {
            // تقسيم الاسم إلى أجزاء حسب الفاصلة أو أي فاصل آخر
            var topicNames = topic.Name.Split(' ');

            // قائمة لتخزين المواضيع التي تم إضافتها
            List<Topic> addedTopics = new List<Topic>();

            foreach (var name in topicNames)
            {
                // إزالة أي مسافات غير ضرورية حول الاسم
                var trimmedName = name.Trim();

                // التحقق مما إذا كان الموضوع موجودًا بالفعل في قاعدة البيانات
                var isFind = context.Topics.Where(p => p.Name == trimmedName).FirstOrDefault();

                if (isFind == null)
                {
                    // إذا لم يتم العثور على الموضوع، نقوم بإنشاء موضوع جديد وإضافته إلى قاعدة البيانات
                    Topic topic1 = new Topic();
                    topic1.Name = trimmedName;
                    context.Topics.Add(topic1);

                    // إضافة الموضوع إلى قائمة المواضيع التي تم إضافتها
                    addedTopics.Add(topic1);
                }
                else
                {
                    // إذا كان الموضوع موجودًا بالفعل، نقوم بإضافته إلى القائمة أيضًا
                    addedTopics.Add(isFind);
                }
            }

            // حفظ جميع التغييرات في قاعدة البيانات
            context.SaveChanges();

            // إرجاع قائمة المواضيع التي تم إضافتها أو العثور عليها
            return addedTopics;
        }

        public void insertBlogTopic(BlogPostTopic topic)
        {
            context.PostTopic.Add(topic);
            context.SaveChanges();
        }
        public List<TopicDTO> getTopics()
        {
           List<Topic> topics = context.Topics.ToList();
            List<TopicDTO> topicDTOs = new List<TopicDTO>();
            foreach (var topic in topics)
            {
                TopicDTO topicDTO = new TopicDTO();
                topicDTO.Id = topic.Id;
                topicDTO.Name = topic.Name;
                topicDTOs.Add(topicDTO);
            }
            return topicDTOs;
        }
    }
}
