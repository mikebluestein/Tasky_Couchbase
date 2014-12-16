**Welcome to the Couchbase Workshop**

You will integrate Couchbase Mobile into the Tasky Shared project by following the steps below.  The ‘TaskManager.cs’ file has missing lines of Code that references each of the numbered steps below.  You will be able to have a complete Couchbase Mobile Database by the end of this Workshop.

1.  **Import the Couchbase Lite package** 

You need to import the required package to have the available Couchbase classes and methods  

**What you should have on top is:

using Couchbase.Lite;

 

2.  **Declare a ‘Database’ variable**

You will now create a database variable.  This is not the name of your database but how you will reference your database in code throughout the project. 

**What you should have is:

Database db;

 

3.  **Initialize the database**

The third step is to initialize the database inside the constructor.  Take a look at the API Reference and obtain a ‘[SharedInstance](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/manager/manager/index.html#manager-sharedinstance--get-)’  This is a shared and pre-process instance of the database Manager

This is a property belonging to the Manger class so in code what you will write is:

 db = Manager.SharedInstance

 

To initialize the database now you simply give the database a name.  Take a  look at the available methods that would allow you to [create a database](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/manager/manager/index.html#database-getdatabasestring-name).

The method ‘getDatabase’ will return the database that of the name you provided and if the database does not exist, it will be created for you with the name that is provided.   In this example, call the method and pass in a database name called "tasky"

 db = Manager.SharedInstance.GetDatabase("tasky");

 

Congrats you have now just created a database!  The database name is called ‘tasky’ or whatever name you provided. 

**What you should have in your constructor is:

        public TaskManager ()

        {

            db = Manager.SharedInstance.GetDatabase ("tasky");

        }

 

4.  **Create a GetTask method**

We already created the GetTask method for you in your code but it is missing a few things.  The parameter being passed into the GetTask method is a string value that represents the document id for a particular Task.  Each Task/Item created by the end user in the app will have a unique document ID in your database.  So when a Task is created, a document is created.  You can think of a Document as representing a Task here. 

**The method is

public Task GetTask (string id)

 

5.  **Obtain the document of interest**

In order to obtain the document of interest from the ‘String Id’ that is passed into the method, you will first create a document variable in code to capture the document of interest.  Looking at the [API Reference](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/database/database/index.html#document-getdocumentstring-id) again, there is ‘GetDocument’ method that takes a String value that will either create or return the document of interest.

This method belongs to the database class so we will use the database instance that we created to call the ‘GetDocument’ method

**What the GetDocument code looks like

var doc = db.GetDocument (id);

 

6.  **Obtain the document properties**

Now you will obtain the properties of the document.  The API that we will be using is ‘[UserProperties](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/document/document/index.html#mapstring-object-userproperties--get-)’ and gets the properties of the current Revision of the Document.

**What the UserProperties code looks like

var props = doc.UserProperties;

* *

*NOTE:  We have already created a Task class and object for you that include the ID, Name, Notes, and the Boolean value of ‘Done’ to be referenced to variables within the object.*

7.  **Return the object of interest**  

The GetTask Function is a type ‘Task’ so we must return a Task object.

**What you return in the GetTask function

return task;

**What you should have in your final GetTask method is:

        public Task GetTask (string id)

        {

            var doc = db.GetDocument (id);

            var props = doc.UserProperties;

            var task = new Task {

                ID = id,

                Name = props ["name"].ToString (),

                Notes = props ["notes"].ToString (),

                Done = (bool)props ["done"]

            };

            return task;

        }

** **

**8.  Obtain all the documents in the database **

‘[CreateAllDocumentsQuery](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/database/database/index.html#query-createalldocumentsquery)’ method will create a Query that matches all Documents in the Database.

**Your code should look like

var query = db.CreateAllDocumentsQuery ();

 

**9.  Return the Document ID**

From the set of results you obtain when you call the ‘Run()’ method on the query, you will loop through these results to obtain the properties of the document.

We will operate on a resultant row for a Coucbase Lite View, which is an index.

 

Obtain the ID by calling on the ‘[DocumentID](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/query/query-row/index.html#string-documentid--get-)’ property on a particular row.

**Your code should look like

ID = row.DocumentId,

 

**10.  Return the value in ‘notes’ key**

From the row instance, obtain the ‘notes’ key with the ‘UserProperties’ from within a Document.    

** Your code should look like

Notes = row.Document.UserProperties ["notes"].ToString (),

 

**11.  Create Document**

Within the ‘SaveTask’ method, you will first check if a particular Task object exists.  You do this by checking if the object ID property is null.  You will create a document by calling the ‘CreateDocument’ method on the database.  

** Your code should look like

doc = db.CreateDocument ();

 

**12. Add Task Item to Document**

With a new Task available to save and put into the document, you will call the ‘[PutProperties](http://developer.couchbase.com/mobile/develop/references/couchbase-lite/couchbase-lite/document/document/index.html#savedrevision-putpropertiesmapstring-object-properties)’ method, which will create and save a new Revision with the specified properties.  The method takes a Dictionary as an input parameter so we will call the ‘ToDictionary’ method on the Task item. ** **

**Your code should look like

doc.PutProperties (item.ToDictionary ());

 

**13.  Retrieve Document**

If in the case the Task object’s ID value is not null, you will first retrieve the document in order to update the values of the keys.  This is might look familiar as you have done this also in Step 5

**Your code should look like

doc = db.GetDocument (item.ID);

 

**14.  Update Document**

Copy and paste in the code that will update the document with the new key-values from the Task object before the end of the ‘else’ statement.

** Your code should look like

                doc.Update (newRevision => {

                    var props = newRevision.Properties;

                    props ["name"] = item.Name;

                    props ["notes"] = item.Notes;

                    props ["done"] = item.Done;

                    Console.WriteLine("You are accessing database named "+ db.Name);

                    Console.WriteLine("The current revision " + item.ID);

                    return true;

                });

 

**What you should have at the end for the SaveTask method

 

        public void SaveTask (Task item)

        {

            Document doc;

            if (item.ID == null) {

                doc = db.CreateDocument ();

                doc.PutProperties (item.ToDictionary ());

                item.ID = doc.Id;

                Console.WriteLine("You have created a new Document: " + doc);

                Console.WriteLine("The Document ID is: " + item.ID);

            } else {

                doc = db.GetDocument (item.ID);

                Console.WriteLine("The previous revision " + item.ID);

                doc.Update (newRevision => {

                    var props = newRevision.Properties;

                    props ["name"] = item.Name;

                    props ["notes"] = item.Notes;

                    props ["done"] = item.Done;

                    Console.WriteLine("You are accessing database named "+ db.Name);

                    Console.WriteLine("The current revision " + item.ID);

                    return true;

                });

            }

        }

 

**15.  Delete Document**

To delete a document, you will first obtain the document you wish to delete from a given Task object’s database.  Then you will call the ‘Delete’ method on the document to delete. 

**Your Code should look like

doc.Delete ();

 

