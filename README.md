# Neukeeper

This is a reimplementation of Nukeeper using dotnet-outdated underneath.

*This project is still under development and doesn't yet support full functionality*

Supported providers:

| Provider | Support |
| -------- | ------- |
| Github   | :hammer:|
| Bitbucket Server | :hammer:|

# How to use:

## Github:
Add an environment variable REPO_TOKEN with a PAT for the repo
Then run:
`neukeeper https://github.com/evolvedlight/sample-outdated -pr -u --username="evolvedlight" --commitEmail="steve@brown.bg"`

## BitBucket:
Add an environment variable REPO_TOKEN 
Then run:
```neukeeper http://localhost:17990/projects/TEST/repos/sample-outdated/browse -pr -u --username="evo" --commitEmail="steve@brown.bg"```
