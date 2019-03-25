# dotnetcore-blog

[![Build Status](https://www-martinstromberg.visualstudio.com/dotnetcore-blog/_apis/build/status/martinstromberg.dotnetcore-blog?branchName=master)](https://www-martinstromberg.visualstudio.com/dotnetcore-blog/_build/latest?definitionId=5&branchName=master)

## What's this project?

In order to keep up with the new things in .NET Core, I decided to use my go-to method for getting started in any new language/technique - coding a blog. I've also decided to get some more experience working with Microsoft Orleans and GraphQL.NET.  There's not going to be anything special or revolutionary here. Just a (somewhat) simple blog. 

## So what's the setup?

While this might be subject to change, sooner or later, we're going to have a TypeScript node.js front-facing server that does some nice ReactDOMServer stuff. The node server sends requests to the GraphQL API which fills the requests with data it reads from the Orleans Grains. The SiloHost makes sure that the grains can access the data storage.

## Can I help?

Well... while I wouldn't normally turn down help, this is an educational project and I'd like to do the lion's share myself. What I could need some assistance with is thinking of stuff that a blog should be capable of handling so if you have any ideas or thoughts on what I should add, feel free to add an issue about it - just make sure you provide some details about the feature in the form of a user story or something like that.
