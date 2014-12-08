using System;
using System.Collections.Generic;
using Couchbase.Lite;

namespace TaskyShared
{
    public class TaskManager
    {
        Database db;

        public TaskManager ()
        {
            db = Manager.SharedInstance.GetDatabase ("tasky");
        }

        public Task GetTask (string id)
        {
            throw new NotImplementedException ("To Do: need to query couchbase for this task");
        }

        public IList<Task> GetTasks ()
        {
            var query = db.CreateAllDocumentsQuery ();
            var results = query.Run ();
            var tasks = new List<Task> ();

            foreach (var row in results) {
                var task = new Task {
                    ID = row.DocumentId,
                    Name = row.Document.UserProperties ["name"].ToString (),
                    Notes = row.Document.UserProperties ["notes"].ToString (),
                    Done = (bool)row.Document.UserProperties ["done"]
                };
                tasks.Add (task);
            }
            return tasks;
        }

        public void SaveTask (Task item)
        {
            Document doc;

            if (item.ID == null) {
                doc = db.CreateDocument ();
                doc.PutProperties (item.ToDictionary ());
                item.ID = doc.Id;
            } else {
                doc = db.GetDocument (item.ID);
                doc.Update (newRevision => {
                    var props = newRevision.Properties;
                    props ["name"] = item.Name;
                    props ["notes"] = item.Notes;
                    props ["done"] = item.Done;
                    return true;
                });
            }
        }

        public void DeleteTask (Task task)
        {
            var doc = db.GetDocument (task.ID);
            doc.Delete ();
        }
    }
}