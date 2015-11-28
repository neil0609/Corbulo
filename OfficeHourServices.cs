using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Sabio.Web.Domain;
using Sabio.Data;
using Sabio.Web.Services.Interfaces;


namespace Sabio.Web.Services
{
    public class OfficeHourServices : BaseService, IOfficeHourServices
    {

        public int Add(OfficeHourAddRequest model, string userId)
        {
            var id = 0;
            var ohqId = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHours_Insert",
                inputParamMapper: delegate(SqlParameterCollection addParameterCollection)
                {
                    addParameterCollection.AddWithValue("@InstructorId", model.InstructorId);
                    addParameterCollection.AddWithValue("@Date", model.Date);
                    addParameterCollection.AddWithValue("@StartTime", model.StartTime);
                    addParameterCollection.AddWithValue("@EndTime", model.EndTime);
                    addParameterCollection.AddWithValue("@TimeZone", model.TimeZone);
                    addParameterCollection.AddWithValue("@Topic", model.Topic);
                    addParameterCollection.AddWithValue("@Instructions", model.Instructions);
                    addParameterCollection.AddWithValue("@SectionId", model.SectionId);
                    addParameterCollection.AddWithValue("@UserId", userId);

                    SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    addParameterCollection.Add(p);
                },
                returnParameters: delegate(SqlParameterCollection para)
                {
                    int.TryParse(para["@Id"].Value.ToString(), out id);
                }
                );

            if (model.Questions != null)
            {
                foreach (var Question in model.Questions)
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHourQuestions_Insert",
                        inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                        {
                            parameterCollection.AddWithValue("@UserId", userId);
                            parameterCollection.AddWithValue("@OfficeHourId", id);
                            parameterCollection.AddWithValue("@Question", Question.Question);
                            parameterCollection.AddWithValue("@Response", Question.Response);
                            parameterCollection.AddWithValue("@Grouping", Question.Grouping);
                            parameterCollection.AddWithValue("@QuestionStatusId", Question.QuestionStatusId);

                            SqlParameter q = new SqlParameter("@Id", System.Data.SqlDbType.Int)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            parameterCollection.Add(q);
                        },
                    returnParameters: delegate(SqlParameterCollection para)
                    {
                        int.TryParse(para["@Id"].Value.ToString(), out ohqId);
                    }
                    );

                foreach (var Question in model.Questions)
                    if (Question.Tag != null)
                    {
                        foreach (var TagId in Question.Tag)
                            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHourQuestionTags_Insert",
                                inputParamMapper: delegate(SqlParameterCollection parameter)
                                {
                                    parameter.AddWithValue("@OfficeHourQuestionId", ohqId);
                                    parameter.AddWithValue("@TagId", TagId);
                                });
                    }
            }
            return id;
        }

        public void Update(OfficeHourUpdateRequest model, string userId)
        {
            var ohqId = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHours_Update",
                inputParamMapper: delegate(SqlParameterCollection updateParameterCollection)
                {
                    updateParameterCollection.AddWithValue("@Id", model.Id);
                    updateParameterCollection.AddWithValue("@InstructorId", model.InstructorId);
                    updateParameterCollection.AddWithValue("@Date", model.Date);
                    updateParameterCollection.AddWithValue("@StartTime", model.StartTime);
                    updateParameterCollection.AddWithValue("@EndTime", model.EndTime);
                    updateParameterCollection.AddWithValue("@TimeZone", model.TimeZone);
                    updateParameterCollection.AddWithValue("@Topic", model.Topic);
                    updateParameterCollection.AddWithValue("@Instructions", model.Instructions);
                    updateParameterCollection.AddWithValue("@SectionId", model.SectionId);
                    updateParameterCollection.AddWithValue("@UserId", userId);
                });
            if (model.Questions != null)
            {
                foreach (var Question in model.Questions)
                    if (Question.Question != null)
                    {
                        DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHourQuestions_Update",
                            inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                            {
                                parameterCollection.AddWithValue("@OfficeHourId", model.Id);
                                parameterCollection.AddWithValue("@Question", Question.Question);
                                parameterCollection.AddWithValue("@Response", Question.Response);
                                parameterCollection.AddWithValue("@Grouping", Question.Grouping);
                                parameterCollection.AddWithValue("@QuestionStatsId", Question.QuestionStatusId);

                                //SqlParameter q = new SqlParameter("@Id", System.Data.SqlDbType.Int)
                                //    {
                                //        Direction = System.Data.ParameterDirection.Output
                                //    };
                                //parameterCollection.Add(q);
                            },
                            returnParameters: delegate(SqlParameterCollection para)
                            {
                                int.TryParse(para["@Id"].Value.ToString(), out ohqId);
                            });
                    }
                foreach (var Question in model.Questions)
                    if (Question.Tag != null)
                    {
                        DataProvider.ExecuteNonQuery(GetConnection, "OfficeHourQuestionTags_DeleteByOfficeHourQuestionId",
                           inputParamMapper: delegate(SqlParameterCollection paramCollection)
                           {
                               paramCollection.AddWithValue("@OfficeHourQuestionId", ohqId);
                           });

                        foreach (var TagId in Question.Tag)
                        {

                            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHourQuestionTags_Insert",
                            inputParamMapper: delegate(SqlParameterCollection parameterCollection)
                            {
                                parameterCollection.AddWithValue("@OfficeHourQuestionId", ohqId);
                                parameterCollection.AddWithValue("@TagId", TagId);
                            });
                        }
                    }
            }
        }

        public OfficeHour Get(int Id)
        {
            OfficeHour session = null;
            OfficeHourQuestion question = null;
            Dictionary<int, OfficeHourQuestion> book = new Dictionary<int, OfficeHourQuestion>();
            OfficeHourQuestionTag tag = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.OfficeHours_SelectById"
               , inputParamMapper: delegate(SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", Id);

               }, map: delegate(IDataReader reader, short set)
               {
                   if (set == 0)
                   {
                       session = OfficeHourMap(reader);
                   }
                   else if (set == 1)
                   {
                       question = OfficeHourQuestionMap(reader);
                       if (session.Questions == null)
                       {
                           session.Questions = new List<OfficeHourQuestion>();
                       }
                       session.Questions.Add(question);

                       book.Add(question.Id, question);

                   }
                   else if (set == 2)
                   {
                       tag = OfficeHourQuestionTagMap(reader);

                       var parent = book[tag.OfficeHourQuestionId];

                       if (parent.Tags == null)
                       {
                           parent.Tags = new List<Tag>();
                       }

                       Tag t = new Tag();
                       t.Id = tag.TagId;
                       t.TagName = tag.TagName;
                       parent.Tags.Add(t);

                   }
               }
               );
            return session;
        }

        public List<OfficeHour> GetList()
        {
            List<OfficeHour> session = null;
            List<OfficeHourQuestion> question = null;
            Dictionary<int, OfficeHour> book = null;
            Dictionary<int, OfficeHourQuestion> diary = null;


            DataProvider.ExecuteCmd(GetConnection, "dbo.OfficeHours_SelectAll"
                , inputParamMapper: null,
                map: delegate(IDataReader reader, short set)
                {
                    if (set == 0)
                    {
                        if (session == null)
                        {
                            session = new List<OfficeHour>();
                        }

                        if (book == null)
                        {
                            book = new Dictionary<int, OfficeHour>();
                        }

                        ProcessOfficeHourData(reader, session, book);
                    }
                    else if (set == 1)
                    {
                        OfficeHourQuestion item = OfficeHourQuestionMap(reader);

                        var parent = book[item.OfficeHourId];

                        if (parent.Questions == null)
                        {
                            parent.Questions = new List<OfficeHourQuestion>();
                        }

                        if (diary == null)
                        {
                            diary = new Dictionary<int, OfficeHourQuestion>();
                        }

                        parent.Questions.Add(item);

                        diary.Add(item.Id, item);

                    }
                    else if (set == 2)
                    {

                        ProcessOfficeHourQuestionTagData(reader, diary, question);
                    }
                }
                );

            return session;
        }

        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.OfficeHours_DeleteById",
                inputParamMapper: delegate(SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                });
        }

        private void ProcessOfficeHourQuestionTagData(IDataReader reader, Dictionary<int, OfficeHourQuestion> Diary, List<OfficeHourQuestion> question)
        {
            OfficeHourQuestionTag label = OfficeHourQuestionTagMap(reader);

            var parentQuestion = Diary[label.OfficeHourQuestionId];

            Tag t = new Tag();
            t.Id = label.TagId;
            t.TagName = label.TagName;

            if (parentQuestion.Tags == null)
            {
                parentQuestion.Tags = new List<Tag>();
            }

            parentQuestion.Tags.Add(t);

        }

        private void ProcessOfficeHourData(IDataReader reader, List<OfficeHour> session, Dictionary<int, OfficeHour> book)
        {
            OfficeHour lecture = OfficeHourMap(reader);

            session.Add(lecture);

            book.Add(lecture.Id, lecture);

        }

        private OfficeHour OfficeHourMap(IDataReader reader)
        {
            OfficeHour item = new OfficeHour();
            int startingIndex = 0;

            item.Id = reader.GetSafeInt32(startingIndex++);
            item.InstructorId = reader.GetSafeInt32(startingIndex++);
            item.Date = reader.GetSafeDateTime(startingIndex++);
            item.StartTime = reader.GetSafeInt32(startingIndex++);
            item.EndTime = reader.GetSafeInt32(startingIndex++);
            item.TimeZone = reader.GetSafeInt32(startingIndex++);
            item.Topic = reader.GetSafeString(startingIndex++);
            item.Instructions = reader.GetSafeString(startingIndex++);
            item.SectionId = reader.GetSafeInt32(startingIndex++);

            return item;
        }

        private OfficeHourQuestion OfficeHourQuestionMap(IDataReader reader)
        {
            OfficeHourQuestion item = new OfficeHourQuestion();
            int startingIndex = 0;

            item.Id = reader.GetSafeInt32(startingIndex++);
            item.OfficeHourId = reader.GetSafeInt32(startingIndex++);
            item.Question = reader.GetSafeString(startingIndex++);
            item.Response = reader.GetSafeString(startingIndex++);
            item.Grouping = reader.GetSafeString(startingIndex++);
            item.QuestionStatusId = reader.GetSafeInt32(startingIndex++);

            return item;
        }

        private OfficeHourQuestionTag OfficeHourQuestionTagMap(IDataReader reader)
        {
            OfficeHourQuestionTag item = new OfficeHourQuestionTag();
            int startingIndex = 0;

            item.OfficeHourQuestionId = reader.GetSafeInt32(startingIndex++);
            item.TagId = reader.GetSafeInt32(startingIndex++);
            item.TagName = reader.GetSafeString(startingIndex++);

            return item;
        }

        public List<OfficeHour> GetByDate()
        {
            List<OfficeHour> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.OfficeHours_SelectAllBYDate"
                , inputParamMapper: null,
                map: delegate (IDataReader reader, short set)
                {
                    OfficeHour item = OfficeHourMap(reader);

                    if (list == null)
                    {
                        list = new List<OfficeHour>();
                    }

                    list.Add(item);
                }
               );
            return list;
        }
    }
}



