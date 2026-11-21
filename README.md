# DocumentAPI: Backend trial assignment

## How to run the project

- Start the project from Visual Studio
- In the browser go to the url: https://localhost:7004/swagger/index.html
- Upload a text file using file control and clicking the execute button.
- The API result should contain the text at the end somthing like: 
	[Original file content]
	DateTime: 2026-11-21T11:35:42.9657404+00:00
	Random data: a77fa34f3fba443a940d00c50204065a

The updated content is returned in the result of the POST. It's also possible to download the file by calling the GET the Id in the response from the PSOT.