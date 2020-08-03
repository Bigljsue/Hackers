using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF_HackersList.Classes.DataBaseConfiguration;
using WPF_HackersList.Models;

namespace WPF_HackersList.DataBaseClasses.DataBaseMethods
{
    public class DataBaseMethods : IDataBaseGetMethods, IDataBaseAddMethods, IDataBaseDeleteMethods, IDataBaseUpdateMethods
    {
        public List<PersonModel> GetPeopleList()
        {
            List<PersonModel> people = new List<PersonModel>();

            using (var dataBase = new DataBaseConfiguration())
                people = dataBase.People.ToList();

            return people;
        }

        public void AddPerson(string name)
        {
            PersonModel person = new PersonModel { Name = name };

            using (var dataBase = new DataBaseConfiguration())
            {
                dataBase.Add(person);
                dataBase.SaveChanges();
            }
        }

        public void DeletePerson(int id)
        {
            PersonModel person;

            using (var dataBase = new DataBaseConfiguration())
            {
                person = dataBase.People.Where(x => x.Id == id).FirstOrDefault();
                dataBase.Remove(person);
                dataBase.SaveChanges();
            }
        }

        public void UpdateChanges(List<PersonModel> people)
        {
            PersonModel dataBasePerson;
            bool updatePerson = false;

            using (var dataBase = new DataBaseConfiguration())
            {
                foreach (PersonModel person in people) 
                {
                    dataBasePerson = dataBase.People.AsNoTracking().Where(x => x.Id == person.Id).FirstOrDefault();
                    if (person.Name != dataBasePerson.Name)                    
                        updatePerson = true;

                    if (updatePerson == true)
                    {
                        dataBase.Update(person);
                    }
                        dataBase.SaveChanges();                    
                }
            }
        }

        //public void AddKandidatTestWork(int id, TestModel test)
        //{
        //    var Kandidat = GetKandidat(id);
        //    var testData = String.Format("{0},{1}||", test.QuestionId, test.Points);
        //    var commentary = String.Format("{0},{1}||", test.QuestionId, test.Commentary);

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //    {
        //        Kandidat.Tests += testData;
        //        Kandidat.Commentaries += commentary;
        //        Kandidat.AllPoints += test.Points;
        //        Kandidat.GeneralPoints += test.GeneralPoints;
        //        Kandidat.LastQuestionId = test.QuestionId;

        //        dataBase.Update(Kandidat);
        //        dataBase.SaveChanges();
        //    }
        //}

        //public int DeleteKandidat(int id)
        //{
        //    if (GetKandidat(id) == null)
        //        return 0;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //    {
        //        Kandidat Kandidat = dataBase.Kandidats.Where(x => x.Id == id).First();
        //        dataBase.Remove(Kandidat);
        //        dataBase.SaveChanges();
        //    }
        //    return id;
        //}

        //public bool IsKandidatExist(int id)
        //{
        //    Kandidat Kandidat;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        Kandidat = dataBase.Kandidats.Where(x => x.Id == id).FirstOrDefault();

        //    if (Kandidat == null)
        //        return false;

        //    return true;
        //}

        //public bool IsKandidatExist(Kandidat Kandidat)
        //{
        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        return dataBase.Kandidats.FirstOrDefault(man =>
        //              man.NameSurName == Kandidat.NameSurName &&
        //              man.Vacancy == Kandidat.Vacancy &&
        //              man.Recruter == Kandidat.Recruter
        //        ) != null;
        //}

        //public void UpdateKandidat(Kandidat Kandidat)
        //{
        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //    {
        //        dataBase.Update(Kandidat);
        //        dataBase.SaveChanges();
        //    }
        //}

        //public Kandidat GetKandidat(Kandidat kandidatToCheck)
        //{
        //    Kandidat Kandidat;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        Kandidat = dataBase.Kandidats.FirstOrDefault(man =>
        //              man.NameSurName == kandidatToCheck.NameSurName &&
        //              man.Vacancy == kandidatToCheck.Vacancy &&
        //              man.Recruter == kandidatToCheck.Recruter);

        //    return Kandidat;
        //}

        //public Question GetQuestion(int id)
        //{
        //    Question question;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        question = dataBase.Questions.FirstOrDefault(q => q.Id == id);

        //    return question;
        //}

        //public Question GetPreviousQuestion(Question question)
        //{
        //    Question previousQuestion = question;

        //    if (question == GetQuestionParent(question))
        //        return GetFirstChild(previousQuestion);

        //    previousQuestion = GetPreviousQuestionWithAnswers(previousQuestion);

        //    if (previousQuestion != null)
        //        return previousQuestion;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //    {
        //        Question questionParent = GetQuestionParent(question);
        //        while (previousQuestion == null)
        //        {
        //            previousQuestion = dataBase.Questions
        //                .Where(
        //                 x =>
        //                 x.QuestionParentId == questionParent.QuestionParentId
        //                 )
        //                .Select(x => x)
        //                .OrderByDescending(x => x.QuestionNumber)
        //                .FirstOrDefault(x => x.QuestionNumber < questionParent.QuestionNumber);

        //            if (previousQuestion == null)
        //                if (questionParent == GetQuestionParent(questionParent))
        //                    previousQuestion = question;
        //                else
        //                    questionParent = GetQuestionParent(questionParent);
        //        }
        //    }

        //    if (previousQuestion == question)
        //    {
        //        return question;
        //    }

        //    previousQuestion = GetLastChild(previousQuestion);

        //    if (previousQuestion != null)
        //        return previousQuestion;

        //    return question;
        //}

        //public Question GetNextQuestion(Question question)
        //{
        //    Question nextQuestion = question;

        //    if (question == GetQuestionParent(question))
        //        return GetFirstChild(nextQuestion);

        //    nextQuestion = GetNextQuestionWithAnswers(nextQuestion);

        //    if (nextQuestion != null)
        //        return nextQuestion;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //    {
        //        Question questionParent = GetQuestionParent(question);
        //        while (nextQuestion == null)
        //        {
        //            nextQuestion = dataBase.Questions
        //                .Where(
        //                 x =>
        //                 x.QuestionParentId == questionParent.QuestionParentId
        //                 )
        //                .Select(x => x)
        //                .OrderBy(x => x.QuestionNumber)
        //                .FirstOrDefault(x => x.QuestionNumber > questionParent.QuestionNumber);

        //            if (nextQuestion == null)
        //                if (questionParent == GetQuestionParent(questionParent))
        //                    nextQuestion = question;
        //                else
        //                    questionParent = GetQuestionParent(questionParent);
        //        }
        //    }

        //    if (nextQuestion == question)
        //    {
        //        return question;
        //    }

        //    nextQuestion = GetFirstChild(nextQuestion);

        //    if (nextQuestion != null)
        //        return nextQuestion;

        //    return question;
        //}

        //public Question GetFirstChild(Question question)
        //{
        //    if (question == null)
        //        question = GetQuestionsMainParentByNumber(1);

        //    Question parentQuestion = GetQuestionParent(question);
        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration()) // Пытаемся дойти до первого вопроса с ответами в каждом следущем пункте. Пробиваем дно всех первых пунктов до вопросов
        //    {
        //        while (dataBase.Questions.Count(x => x.QuestionParentId == question.Id) > 0)
        //            question = dataBase.Questions
        //                .Where(
        //                 x =>
        //                 x.QuestionParentId == question.Id
        //                 )
        //                .Select(x => x)
        //                .OrderBy(x => x.QuestionNumber)
        //                .FirstOrDefault();
        //    }

        //    return question;
        //}

        //public Question GetLastChild(Question question)
        //{
        //    if (question == null)
        //        question = GetQuestionsMainParentByNumber(1);

        //    Question parentQuestion = GetQuestionParent(question);
        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration()) // Пытаемся дойти до первого вопроса с ответами в каждом следущем пункте. Пробиваем дно всех первых пунктов до вопросов
        //    {
        //        while (dataBase.Questions.Count(x => x.QuestionParentId == question.Id) > 0)
        //            question = dataBase.Questions
        //                .Where(
        //                 x =>
        //                 x.QuestionParentId == question.Id
        //                 )
        //                .Select(x => x)
        //                .OrderByDescending(x => x.QuestionNumber)
        //                .FirstOrDefault();
        //    }

        //    return question;
        //}

        //public Question GetQuestionParent(Question question)
        //{
        //    if (question.QuestionParentId == 0)
        //        return question;

        //    return GetQuestion(question.QuestionParentId);
        //}

        //public Question GetQuestionTopParent(Question question)
        //{
        //    while (GetQuestionParent(question).QuestionParentId != GetQuestionsMainParentByNumber(1).QuestionParentId)
        //        question = GetQuestion(question.QuestionParentId);

        //    return question;
        //}

        //public Question GetQuestionByText(string questionText)
        //{
        //    Question question;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        question = dataBase.Questions.Where(
        //            x => x.QuestionText.Contains(questionText)
        //            ).FirstOrDefault();

        //    return GetFirstChild(question);
        //}

        //public Question GetQuestionsMainParentByNumber(int mainParentNumber)
        //{
        //    Question mainQuestionsParent;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        mainQuestionsParent = dataBase.Questions.Where(x => x.QuestionNumber == mainParentNumber).FirstOrDefault();

        //    return mainQuestionsParent;
        //}

        //public Question GetQuestionMainParent(Question question)
        //{
        //    return GetQuestionParent(GetQuestionTopParent(question));
        //}

        //public Question GetLastQuestionWithAnswersOfMainParent()
        //{
        //    Question newQuestion = GetQuestionsMainParentByNumber(1);

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        while (newQuestion.FirstAnswer == null)
        //        {
        //            newQuestion = dataBase.Questions.Where(
        //                x => x.QuestionParentId == newQuestion.Id)
        //                .OrderByDescending(x => x)
        //                .FirstOrDefault();
        //        }

        //    return newQuestion;
        //}

        //public int GetQuestionsCount(Question question)
        //{
        //    var questionParent = GetQuestionParent(question);
        //    int questionsCount = 0;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        questionsCount = dataBase.Questions.Where(x => x.QuestionParentId == questionParent.Id).Count();
        //    return questionsCount;
        //}



        //private Question GetNextQuestionWithAnswers(Question question)
        //{
        //    Question nextQuestion;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        nextQuestion = dataBase.Questions
        //            .Where(
        //                x =>
        //                x.QuestionParentId == question.QuestionParentId &&
        //                (x.FirstAnswer != null ||
        //                x.SecondAnswer != null ||
        //                x.ThirdAnswer != null)
        //                )
        //            .Select(x => x)
        //            .OrderBy(x => x.QuestionNumber)
        //            .FirstOrDefault(x => x.QuestionNumber > question.QuestionNumber);
        //    return nextQuestion;
        //}
        //private Question GetPreviousQuestionWithAnswers(Question question)
        //{
        //    Question previousQuestion;

        //    using (var dataBase = new DataBaseConfiguration.DataBaseConfiguration())
        //        previousQuestion = dataBase.Questions
        //            .Where(
        //                x =>
        //                x.QuestionParentId == question.QuestionParentId &&
        //                (x.FirstAnswer != null ||
        //                x.SecondAnswer != null ||
        //                x.ThirdAnswer != null)
        //                )
        //            .Select(x => x)
        //            .OrderByDescending(x => x.QuestionNumber)
        //            .FirstOrDefault(x => x.QuestionNumber < question.QuestionNumber);
        //    return previousQuestion;
        //}
    }
}
