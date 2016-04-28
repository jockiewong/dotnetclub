﻿using Discussion.Web.Models;
using Microsoft.AspNet.Mvc;
using System.Linq;
using Discussion.Web.Data;
using Discussion.Web.ViewModels;
using System;
using MarkdownSharp;

namespace Discussion.Web.Controllers
{
    public class TopicController : Controller
    {

        private readonly IDataRepository<Topic> _topicRepo;
        public TopicController(IDataRepository<Topic> topicRepo)
        {
            _topicRepo = topicRepo;
        }


        [Route("/Topic/{id}")]
        public ActionResult Index(int id)
        {
            var topic = _topicRepo.Retrive(id);
            if(topic == null)
            {
                return HttpNotFound();
            }


            // Create new markdown instance
            var markdownConverter = new Markdown();
            var showModel = new TopicShowModel
            {
                Id = topic.Id,
                Title = topic.Title,
                MarkdownContent = topic.Content,
                HtmlContent = markdownConverter.Transform(topic.Content)
            };

            return View(showModel);
        }


        [Route("/")]
        [Route("/Topic/List")]
        public ActionResult List()
        {
            var topicList = _topicRepo.All.ToList();

            return View(topicList);
        }


        [Route("/Topic/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/Topic/CreateTopic")]
        public void CreateTopic(TopicCreationModel model)
        {
            var topic = new Topic
            {
                Title = model.Title,
                Content = model.Content,
                CreatedAt = DateTime.UtcNow
            };


            _topicRepo.Create(topic);
        }
    }
}