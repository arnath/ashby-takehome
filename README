# Ashby Engineer Technical Challenge
This repo is for the [Ashby Take-home Engineer Technical Challenge](https://www.ashbyhq.com/resources/engineer-technical-challenge). It contains a proof of concept API and data model for defining, sending, and responding to custom forms (like Google Forms). 
## Code Layout
- `Ashby/ApiController.cs` - This contains a very simple version of what the API logic might look like with most of the calls not actually doing anything. 
- `Ashby/Models/` - This folder contains the various model files for a form, a form field, and a form response. 
## Design Considerations
The first step I took when thinking about the design for this challenge was to put together a list of functional requirements:
- Users can create a form with a set of fields.
- Users can submit responses to a form.
- Form fields can be of types text, email, select, boolean, and file.
- Form fields can be conditionally visible based on other fields having specific values. 
There were also two additional requirements that weren't listed in the spec but I thought were reasonable additions for a system like this:
- Fields can be marked as required.
- Fields can specify a set of allowed values.

After understanding the requirements, I took a look at the Google Forms API. I think when starting a new project, one of the best things you can do is to look at similar projects internally or externally, understand what they've done, and try to figure out why. This can be really helpful in understanding the challenges with the given space. 

After all this, there were a few interesting decision points during the design process. Each of these is discussed below.
### Single class vs subclasses for fields
To represent different field types, there were two reasonable options: a single class with a type field or subclasses for each field. I went with single classes for two reasons:
1. The prompt asked me to design a REST API (which means JSON serialization/deserialization) and JSON doesn't handle subclasses very well. If the client sent a `Form` object to the create API, I'd have to deserialize each field to the base class then re-deserialize them to the appropriate subclass after figuring out the type. 
2. All of the subclasses have the same set of fields. As such, the only method that would be different is `Validate`, which was easy enough to implement for the small set of types we have.
### Validation
I think when designing a user-facing API like this, validation is _really_ important. We're even getting two layers of user input instead of just one: an end user designs the form and then other end users submit responses. Therefore, I added detailed validation for both form creation and response submission to make sure that the user provides values that make sense.
### Names as ID
The API requires users to be able to refer to various fields when creating visibility conditions. Therefore, it didn't make sense to have the database generated ID be the only key. To solve this, I gave fields a `Name` property and enforced uniqueness within the `Form` so they could be referenced.
### Dependent Fields
There's a lot of different ways I could have modeled dependent fields:
- I could have only allowed a field to depend on one other field. I decided to allow multiple because it was an easy extension with the `Dictionary` for storing dependencies. A single field would be limiting the user for no gain on our side.
- I could have done a `Conditional` class similar to what was done in the prompt. However, I think a `Dictionary` makes a lot of sense here because it provides an easy to lookup mapping between the field names and expected values.
- I considered allowing multiple field values for each conditional. In other words, make field B appear when field A has value 1 or 2. I decided not to do this because it adds complexity for not much gain in user scenarios. It would also be very easy to add in the future.
## Future Considerations
There's a bunch of potential additions to a Form API like this that would likely be useful to add in the future. Some of these are discussed below.
### More field types
There's a lot more potential field types that could be added. For example, it might be useful to have a select field that can hold multiple selections at the same time.
### Cutoff date for responses
I think it would be useful to implement a cutoff date for responses. After a certain date, the submit response API would return an error saying that it's past the cutoff. This would allow timed forms or quizzes. 
### Authentication
Supporting anonymous access is great but users would also benefit from the ability to limit who has access to a form. This would require adding an authentication and access management layer to the service.